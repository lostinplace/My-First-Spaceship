using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMachineFMOD : FMODEmitter
{
    // Update is called once per frame
    void Update()
    {
        if (playerState && !playerState.AirIsActive)
        {
            base.setOffline();
        }
        else
        {
            base.setOnline();
        }
    }
}
