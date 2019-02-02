using System.Collections;
using System.Collections.Generic;
using Valve;
using UnityEngine;

public class Battary : Lockable, Handleable.HandleableItem
{
    public Plug currentPlug = null;
    public Plug potentialPlug = null;
    public int plugIndex = -1;
    public bool isBeingHeld = false;
    void Start() {
        Handleable.InitializeHandleableItem(this);
    }
    public void OnPickup()
    {
        Unlock();
        isBeingHeld = true;
        if (currentPlug != null ) {
            currentPlug.IsOccupied[plugIndex] = false;
            currentPlug = null;
        }
    }
    public void OnDrop()
    {
        isBeingHeld = false;
        if (potentialPlug != null)
            potentialPlug.Trigger(this.GetComponent< BoxCollider >());
    }
    public GameObject GetGameObject() {
        return gameObject;
    }
}
