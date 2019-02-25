using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] public DeviceBase engine;

    protected bool previousState = true;

    void Update()
    {
        if (engine.isActive == false)
        {
            Debug.Log("Engine not active!");
            Rigidbody[] rigidBodies = Resources.FindObjectsOfTypeAll<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidBodies)
                rigidbody.useGravity = false;
            previousState = false;
        }
        else if (previousState == false )
        {
            Debug.Log("Engine on");
            previousState = true;
            Rigidbody[] rigidBodies = Resources.FindObjectsOfTypeAll<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidBodies)
                rigidbody.useGravity = true;
        }
    }
}
