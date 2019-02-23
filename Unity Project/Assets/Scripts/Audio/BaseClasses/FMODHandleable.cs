using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODHandleable : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string GrabEvent;

    public void PlayGrab()
    {
        FMODUnity.RuntimeManager.PlayOneShot(GrabEvent, gameObject.transform.position);
    }
}
