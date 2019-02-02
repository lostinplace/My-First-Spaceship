using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public Vector3[] origins;
    public Quaternion[] directions;
    public float[] scale;
    protected bool[] isOccupied;
    protected BoxCollider[] triggers;

    public bool[] IsOccupied { get => isOccupied; }
    private void Start()
    {
        if (origins.Length == directions.Length && directions.Length == scale.Length)
        {
            isOccupied = new bool[scale.Length];
            BoxCollider currentBoxCollider = null;
            triggers = new BoxCollider[scale.Length];
            for (int i = 0; i < origins.Length; ++i)
            {
                currentBoxCollider = gameObject.AddComponent<BoxCollider>() as BoxCollider;
                currentBoxCollider.center = origins[i];
                currentBoxCollider.size *= scale[i];
                currentBoxCollider.isTrigger = true;
                triggers[i] = currentBoxCollider;
                isOccupied[i] = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        Trigger(other);
    }
    public void Trigger(Collider other)
    {
        Battary battary = other.gameObject.GetComponent<Battary>();
        if (battary != null && other is BoxCollider)
        {
            if (battary.isBeingHeld == false)
            {
                BoxCollider otherBox = (BoxCollider)other;
                Vector3 otherCenter = otherBox.transform.TransformPoint(otherBox.center);
                if (triggers.Length > 0)
                {
                    BoxCollider closest = null;
                    float smallestMagnitude = (transform.TransformPoint(triggers[0].center) - otherCenter).magnitude;
                    int foundIndex = 0;
                    //I dont feel like writing a bounding box collision routine right now... this will do.//
                    for (int i = 0; i < triggers.Length; ++i)
                    {
                        float currentMagnitude = (transform.TransformPoint(triggers[i].center) - otherCenter).magnitude;
                        if (currentMagnitude <= smallestMagnitude)
                        {
                            smallestMagnitude = currentMagnitude;
                            closest = triggers[i];
                            foundIndex = i;
                        }
                    }
                    if (closest != null && isOccupied[ foundIndex ] == false )
                    {
                        battary.Lock();
                        battary.transform.rotation = transform.rotation * directions[foundIndex] * battary.direction;
                        battary.transform.position = transform.TransformPoint(closest.center);
                        battary.transform.Translate(battary.transform.TransformVector(battary.origin));
                        isOccupied[foundIndex] = true;
                        battary.currentPlug = this;
                        battary.plugIndex = foundIndex;
                    }
                }
            }
            else
                battary.potentialPlug = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Battary battary = other.gameObject.GetComponent<Battary>();
        if (battary != null && other is BoxCollider)
        {
            if (battary.potentialPlug == this)
                battary.potentialPlug = null;
        }
    }
}
