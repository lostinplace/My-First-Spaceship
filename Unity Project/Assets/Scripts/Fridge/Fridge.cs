using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    [SerializeField] public GameObject foodPrefab, foodOrienter;
    [SerializeField] public Lockable door;
    [SerializeField] public PlayerState player;
    protected DeviceBase refrigerator;
    protected bool lastDoorState, doorIsClosed, foodIsPresent;
    protected GameObject food;

    void Start()
    {
        refrigerator = GetComponent<DeviceBase>();
        lastDoorState = door.IsLocked;
        foodIsPresent = false;
        doorIsClosed = true;
        if (player == null)
            player = GameObject.FindObjectOfType<PlayerState>();
        refrigerator.ItemProduced += ProduceFood;
    }

    void Update()
    {
        if (doorIsClosed == true && ( foodIsPresent == false || food == null ) )
            refrigerator.hasItem = false;
        else
            refrigerator.hasItem = true;
    }
    
    void OnDestroy() {
        //Cleanup.//
        refrigerator.ItemProduced -= ProduceFood;
    }

    void ProduceFood(object sender, EventArgs e)
    {
        food = GameObject.Instantiate(foodPrefab, 
                foodOrienter.transform.position, foodOrienter.transform.rotation);
        food.GetComponent<Food>().ps = player;
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
