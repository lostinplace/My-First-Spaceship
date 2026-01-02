using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMessageEmitter : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string AlertsEvent;
    FMOD.Studio.EventInstance Alerts;
    FMOD.Studio.PLAYBACK_STATE PlaybackState;

    private PlayerState playerState => SceneChanger.playerState;

    private void SetAlarmStates()
    {
        Alerts.setParameterValue("is_engine_offline", playerState && playerState.EngineIsActive ? 0 : 1);
        Alerts.setParameterValue("is_air_offline", playerState && playerState.AirIsActive ? 0 : 1);
        Alerts.setParameterValue("is_food_offline", playerState && playerState.FridgeIsActive ? 0 : 1);
        Alerts.setParameterValue("is_hungry", playerState && playerState.IsHungry ? 1 : 0);

        if (playerState)
        {
            //TODO: make better boolean logic...
            if ((!playerState.EngineIsActive && !playerState.AirIsActive) || (!playerState.EngineIsActive && !playerState.FridgeIsActive) || (!playerState.AirIsActive && !playerState.FridgeIsActive))
            {
                Alerts.setParameterValue("is_plural", 1);
            }
            else
            {
                Alerts.setParameterValue("is_plural", 0);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Alerts = FMODUnity.RuntimeManager.CreateInstance(AlertsEvent);
        Alerts.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Alerts.start();
    }

    public void StopSounds() {
      this.Alerts.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // Update is called once per frame
    void Update()
    {
        this.SetAlarmStates();

        if (playerState && playerState._exited)
        {
            this.Alerts.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
