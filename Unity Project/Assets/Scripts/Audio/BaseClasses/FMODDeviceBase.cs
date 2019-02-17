using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODDeviceBase : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string InEvent;

    [FMODUnity.EventRef]
    public string OutEvent;

    public void PlayIn()
    {
    	FMODUnity.RuntimeManager.PlayOneShot(InEvent, gameObject.transform.position);
    }

    public void PlayOut()
    {
    	FMODUnity.RuntimeManager.PlayOneShot(OutEvent, gameObject.transform.position);
    }
}
