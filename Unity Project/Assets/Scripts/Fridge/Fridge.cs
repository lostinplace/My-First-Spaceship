using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    public GameObject foodPrefab, foodOrienter;
    public Lockable door;

    protected MeshRenderer yellowIndicatorLight, greenIndicatorLight;
    
    protected DeviceBase refrigerator;
    protected bool foodIsPresent;
    
    protected Food food;
    
    public static readonly string GREEN_INDICATOR_LIGHT_TAG = "green_indicator";
    public static readonly string YELLOW_INDICATOR_LIGHT_TAG = "yellow_indicator";
    public static readonly string SHADER_KEYWORD_RO = "_EMISSION";

    void Start()
    {
        refrigerator = GetComponentInParent<DeviceBase>();
        yellowIndicatorLight = this.GetComponentsInChildren<MeshRenderer>()
            .First(x => x.CompareTag(YELLOW_INDICATOR_LIGHT_TAG));
        
        greenIndicatorLight = this.GetComponentsInChildren<MeshRenderer>()
            .First(x => x.CompareTag(GREEN_INDICATOR_LIGHT_TAG));
        
        foodIsPresent = false;
        
        refrigerator.ItemProduced += ProduceFood;
    }

    void Update()
    {
        if(!door.IsLocked)
            refrigerator.productionTime = 0;
        
        if (!foodIsPresent)
            refrigerator.hasItem = false;
        
        if (refrigerator.isActive)
        {
            if (foodIsPresent)
            {
                greenIndicatorLight.materials[1].EnableKeyword(SHADER_KEYWORD_RO);
                yellowIndicatorLight.materials[1].DisableKeyword(SHADER_KEYWORD_RO);
            }
            else
            {
                greenIndicatorLight.materials[1].DisableKeyword(SHADER_KEYWORD_RO);
                yellowIndicatorLight.materials[1].EnableKeyword(SHADER_KEYWORD_RO);
            }
        }
        else {
            yellowIndicatorLight.materials[1].DisableKeyword(SHADER_KEYWORD_RO);
            greenIndicatorLight.materials[1].DisableKeyword(SHADER_KEYWORD_RO);
        }
    }

    void OnDestroy() {
        refrigerator.ItemProduced -= ProduceFood;
    }

    void ProduceFood(object sender, EventArgs e)
    {
        var tmp = GameObject.Instantiate(foodPrefab, 
                foodOrienter.transform.position, foodOrienter.transform.rotation);
        
        food = tmp.GetComponent<Food>();
        
        door.Unlock();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Lockable>() == door)
        {
            if(!foodIsPresent) door.Lock();
        }
        else if (other.gameObject.GetComponent<Food>() == food )
            foodIsPresent = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() == food)
            foodIsPresent = false;
    }
}
