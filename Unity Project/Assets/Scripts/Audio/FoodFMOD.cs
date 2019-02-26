using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFMOD : FMODHandleable
{
	[FMODUnity.EventRef]
	public string foodEatEvent;

	public void PlayFoodEating() {
		FMODUnity.RuntimeManager.PlayOneShot(foodEatEvent);
	}

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}
}
