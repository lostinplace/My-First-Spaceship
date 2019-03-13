using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudFMOD : FMODEmitter
{
    // Update is called once per frame
    void Update()
    {
        if (playerState && !playerState.MonitorIsActive)
        {
            base.setOffline();
        }
        else
        {
            base.setOnline();
        }
    }
}
