using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattaryFMOD : FMODHandleable
{
	[FMODUnity.EventRef]
	public string batteryChargeEvent;

	public void PlayBatteryCharge() {
		FMODUnity.RuntimeManager.PlayOneShot(batteryChargeEvent, gameObject.transform.position);
	}

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}
}
