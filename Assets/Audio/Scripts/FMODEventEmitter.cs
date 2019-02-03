using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventEmitter : MonoBehaviour
{
	[FMODUnity.EventRef]
    public string FMODEvent;
    FMOD.Studio.EventInstance FMOD;

    public bool IsOffline;

    // Start is called before the first frame update
    void Start()
    {
    	this.IsOffline = false;

        this.FMOD = FMODUnity.RuntimeManager.CreateInstance(FMODEvent);
        this.FMOD.setParameterValue("is_offline", this.IsOffline ? 1 : 0);
        this.FMOD.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject)); 
        this.FMOD.start();
    }

    // Update is called once per frame
    void Update()
    {
        this.FMOD.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject)); 
    }

    public void SetOffline()
    {
    	this.IsOffline = true;
    	this.FMOD.setParameterValue("is_offline", 1);
    }

    public void SetOnline()
    {
    	this.IsOffline = false;
    	this.FMOD.setParameterValue("is_offline", 0);
    }
}
