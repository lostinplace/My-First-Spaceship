using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODHandleable : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string GrabEvent;

    [FMODUnity.EventRef]
    public string DropEvent;

    public void PlayGrab()
    {
        FMODUnity.RuntimeManager.PlayOneShot(GrabEvent, gameObject.transform.position);
    }

    public void PlayDrop()
    {
        FMODUnity.RuntimeManager.PlayOneShot(DropEvent, gameObject.transform.position);
    }
}
