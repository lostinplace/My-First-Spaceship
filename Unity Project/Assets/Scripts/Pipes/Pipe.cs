using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public partial class Pipe : Lockable, Handleable.HandleableItem
{
    protected bool isBeingHeld = false;
    public Cradle currentCradle, potentailCradle;

    public bool IsBeingHeld { get => isBeingHeld; }



    public void OnPickup()
    {
        Unlock();
        isBeingHeld = true;
        if (currentCradle != null) {
            currentCradle.isOccupied = false;
            currentCradle = null;
        }
    }

    public void OnDrop()
    {
        isBeingHeld = false;
        if (potentailCradle != null)
        {
            Collider trigger = gameObject.GetComponent<Collider>();
            if (trigger != null)
            {
                potentailCradle.Trigger(trigger);
            }
        }
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    private void Start() {
        Handleable.InitializeHandleableItem(this);
    }
}
