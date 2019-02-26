using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeFMOD : FMODHandleable
{
	[FMODUnity.EventRef]
	public string pipeBurstEvent;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public void playPipeBurst() {
    	FMODUnity.RuntimeManager.PlayOneShot(pipeBurstEvent, gameObject.transform.position);
    }
}
