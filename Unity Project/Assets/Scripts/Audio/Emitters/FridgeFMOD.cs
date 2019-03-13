using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeFMOD : FMODEmitter
{
    // Update is called once per frame
    void Update()
    {
        if (playerState && !playerState.FridgeIsActive)
        {
            base.setOffline();
        }
        else
        {
            base.setOnline();
        }
    }
}
