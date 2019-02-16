using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public partial class Pipe : Lockable, Handleable.HandleableItem
{
    protected bool isBeingHeld = false;
    public Cradle currentCradle, potentialCradle;

    public bool IsBeingHeld { get => isBeingHeld; }

    public void OnPickup()
    {
        Unlock();
        isBeingHeld = true;
        if (currentCradle != null) {
            currentCradle.DetachPipe();
            currentCradle = null;
        }
    }

    public void OnDrop()
    {
        Debug.Log("pipe dropped");
        isBeingHeld = false;
        if (potentialCradle != null)
            potentialCradle.ProcessCollision(this);
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    private void Start() {
        Handleable.InitializeHandleableItem(this);
    }
}
