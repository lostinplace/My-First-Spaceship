using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEmitter : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter ComponentEmitter;
    protected PlayerState playerState => SceneChanger.playerState;

    void Start()
    {
        ComponentEmitter = gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
    }

    protected void setOffline()
    {
        ComponentEmitter.SetParameter("is_offline", 1);
    }

    protected void setOnline()
    {
        ComponentEmitter.SetParameter("is_offline", 0);
    }
}
