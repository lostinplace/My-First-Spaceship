using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineFMOD : FMODEmitter
{
    // Update is called once per frame
    void Update()
    {
        if (playerState && !playerState.EngineIsActive)
        {
            base.setOffline();
        }
        else
        {
            base.setOnline();
        }
    }
}