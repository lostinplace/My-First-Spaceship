using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FailstateIndices
{
    ENGINE,
    AIR,
    FOOD
}

public class PlayerFMOD : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string AmbienceEvent;
    FMOD.Studio.EventInstance Ambience;
    FMOD.Studio.PLAYBACK_STATE AmbiencePlaybackState;

    [FMODUnity.EventRef]
    public string FailstateEvent;
    FMOD.Studio.EventInstance Failstate;


    FMOD.Studio.Bus MachinesBus = FMODUnity.RuntimeManager.GetBus("Bus:/machines");
    FMOD.Studio.Bus AmbienceBus = FMODUnity.RuntimeManager.GetBus("Bus/ambience");


    // Start is called before the first frame update
    void Start()
    {
        this.Ambience = FMODUnity.RuntimeManager.CreateInstance(AmbienceEvent);
        this.Ambience.start();
        this.Ambience.release();
    }

    // Update is called once per frame
    void Update() {}

    public void setFailstate(string failType)
    {
        this.Failstate = FMODUnity.RuntimeManager.CreateInstance(FailstateEvent);

        switch (failType)
        {
            case "engine":
                this.Failstate.setParameterValue("failstate", (int)FailstateIndices.ENGINE);
                break;
            case "air":
                this.Failstate.setParameterValue("failstate", (int)FailstateIndices.AIR);
                break;
            case "food":
                this.Failstate.setParameterValue("failstate", (int)FailstateIndices.FOOD);
                break;
            default:
                this.Failstate.setParameterValue("failstate", 0);
                break;
        }

        this.Failstate.start();
        this.Ambience.release();
    }

    public void KillAllInstances()
    {
        this.MachinesBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        this.AmbienceBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void ResetGameSounds()
    {

    }
}
