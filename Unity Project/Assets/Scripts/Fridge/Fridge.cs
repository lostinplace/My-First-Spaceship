using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    public GameObject foodPrefab, foodOrienter;
    public Lockable door;

    protected MeshRenderer yellowIndicatorLight, greenIndicatorLight;
    protected Material yellowIndicatorLightMaterial, greenIndicatorLightMaterial;
    protected DeviceBase refrigerator;
    protected bool lastDoorState, doorIsClosed, foodIsPresent;
    protected GameObject food;
    public static readonly string GREEN_INDICATOR_LIGHT_RO = "indicator_light_green";
    public static readonly string YELLOW_INDICATOR_LIGHT_RO = "indicator_light_yellow";
    public static readonly string SHADER_KEYWORD_RO = "_EMISSION";

    void Start()
    {
        refrigerator = GetComponentInParent<DeviceBase>();
        MeshRenderer[] childrenShaders = transform.parent.GetComponentsInChildren<MeshRenderer>();
        Regex yellowLightRegex = new Regex(YELLOW_INDICATOR_LIGHT_RO);
        Regex greenLightRegex = new Regex(GREEN_INDICATOR_LIGHT_RO);
        foreach( MeshRenderer current in childrenShaders )
        {
            if (current.gameObject.name.CompareTo(YELLOW_INDICATOR_LIGHT_RO) == 0)
            {
                foreach (Material currentMaterial in current.materials) {
                    if (yellowLightRegex.IsMatch( currentMaterial.name ) == true)
                        yellowIndicatorLightMaterial = currentMaterial;
                }
                yellowIndicatorLight = current;
            }
            else if (current.gameObject.name.CompareTo(GREEN_INDICATOR_LIGHT_RO) == 0)
            {
                foreach (Material currentMaterial in current.materials) {
                    if (greenLightRegex.IsMatch( currentMaterial.name ) == true)
                        greenIndicatorLightMaterial = currentMaterial;
                }
                greenIndicatorLight = current;
            }
            if (greenIndicatorLight != null && yellowIndicatorLight != null)
                break;
        }
        lastDoorState = door.IsLocked;
        foodIsPresent = false;
        doorIsClosed = true;
        refrigerator.ItemProduced += ProduceFood;
        yellowIndicatorLightMaterial.DisableKeyword(SHADER_KEYWORD_RO);
        greenIndicatorLightMaterial.DisableKeyword(SHADER_KEYWORD_RO);
    }

    void Update()
    {
        if (doorIsClosed == true && ( foodIsPresent == false || food == null ) )
            refrigerator.hasItem = false;
        else
            refrigerator.hasItem = true;
        if (refrigerator.isActive == true && doorIsClosed == true)
        {
            if (foodIsPresent == true)
            {
                greenIndicatorLightMaterial.EnableKeyword(SHADER_KEYWORD_RO);
                yellowIndicatorLightMaterial.DisableKeyword(SHADER_KEYWORD_RO);
            }
            else
            {
                greenIndicatorLightMaterial.DisableKeyword(SHADER_KEYWORD_RO);
                yellowIndicatorLightMaterial.EnableKeyword(SHADER_KEYWORD_RO);
            }
        }
        else {
            yellowIndicatorLightMaterial.DisableKeyword(SHADER_KEYWORD_RO);
            greenIndicatorLightMaterial.DisableKeyword(SHADER_KEYWORD_RO);
        }
    }

    void OnDestroy() {
        //Cleanup.//
        refrigerator.ItemProduced -= ProduceFood;
    }

    void ProduceFood(object sender, EventArgs e)
    {
        food = GameObject.Instantiate(foodPrefab, 
                foodOrienter.transform.position, foodOrienter.transform.rotation);
        door.Unlock();
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Lockable>() == door)
        {
            doorIsClosed = true;
            if (refrigerator.isActive == true)
            {
                if (foodIsPresent == true)
                    door.Unlock();
                else
                    door.Lock();
            }
            else
            {
                if (lastDoorState == true)
                    door.Lock();
                else
                    door.Unlock();
            }
            lastDoorState = door.IsLocked;
        }
        else if (other.gameObject.GetComponent<Food>() != food && food != null)
            foodIsPresent = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() != food && food != null)
            foodIsPresent = false;
        if (other.gameObject.GetComponent<Lockable>() == door)
            doorIsClosed = false;
    }
}
