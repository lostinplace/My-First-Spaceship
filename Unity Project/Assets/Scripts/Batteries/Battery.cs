using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Battery : Lockable, Handleable.HandleableItem
{
    public Plug currentPlug = null;
    public Plug potentialPlug = null;
    
    public bool isBeingHeld = false;
    void Start() {
        Handleable.InitializeHandleableItem(this);
    }
    public void OnPickup()
    {
        Unlock();
        isBeingHeld = true;
        if (currentPlug) currentPlug.DetachBattery();
    }

    public void OnDrop()
    {
        isBeingHeld = false;
        if (potentialPlug) potentialPlug.ProcessCollision(this);
    }
    public GameObject GetGameObject() {
        return gameObject;
    }
}
