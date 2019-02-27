using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMessageEmitter : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string AlertsEvent;
    FMOD.Studio.EventInstance Alerts;
    FMOD.Studio.PLAYBACK_STATE PlaybackState;

    bool IsPlayingAlert;

    private PlayerState playerState;
    private Dictionary<string, bool> componentStates;

    private bool HasOfflineComponent()
    {
        return !this.componentStates["engine"]
            || !this.componentStates["air"]
            || !this.componentStates["food"];
    }

    private void SetAlarmStates()
    {
        this.componentStates["air"] = this.playerState.AirIsActive;
        this.componentStates["engine"] = this.playerState.EngineIsActive;
        this.componentStates["food"] = this.playerState.FridgeIsActive;
        this.componentStates["monitor"] = this.playerState.MonitorIsActive;

        this.Alerts.setParameterValue("is_engine_offline", !this.componentStates["engine"] ? 1 : 0);
        this.Alerts.setParameterValue("is_air_offline", !this.componentStates["air"] ? 1 : 0);
        this.Alerts.setParameterValue("is_food_offline", !this.componentStates["food"] ? 1 : 0);
        this.Alerts.setParameterValue("is_monitor_offline", 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Alerts = FMODUnity.RuntimeManager.CreateInstance(AlertsEvent);
        this.IsPlayingAlert = false;
        playerState = GameObject.FindObjectOfType<PlayerState>();

        this.componentStates = new Dictionary<string, bool>();
        this.componentStates.Add("air", true);
        this.componentStates.Add("engine", true);
        this.componentStates.Add("food", true);
        this.componentStates.Add("monitor", true);
    }

    // Update is called once per frame
    void Update()
    {
        Alerts.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        this.SetAlarmStates();

        Alerts.getPlaybackState(out PlaybackState);
        this.IsPlayingAlert = this.PlaybackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;

        if (this.HasOfflineComponent() && !this.IsPlayingAlert && this.componentStates["monitor"])
        {
            this.PlayMessages();
            this.IsPlayingAlert = true;
        }
    }

    void PlayMessages()
    {
        this.Alerts.start();
    }
}
