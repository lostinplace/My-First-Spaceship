using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMessageEmitter : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string AlertsEvent;
    FMOD.Studio.EventInstance Alerts;
    FMOD.Studio.PLAYBACK_STATE PlaybackState; 

    public int EngineSeverity;
    public int FoodSeverity;
    public int AirSeverity;
    public bool IsHUDDown;
    bool IsPlayingAlert;

    // Start is called before the first frame update
    void Start()
    {
        this.Alerts = FMODUnity.RuntimeManager.CreateInstance(AlertsEvent);

        this.EngineSeverity = 0;
        this.FoodSeverity = 0;
        this.AirSeverity = 0;

        this.IsHUDDown = false;
        this.IsPlayingAlert = false;
    }

    // Update is called once per frame
    void Update()
    {
        Alerts.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        Alerts.getPlaybackState(out PlaybackState);
        this.IsPlayingAlert = this.PlaybackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;

        if ((this.EngineSeverity > 0 || this.FoodSeverity > 0 || this.AirSeverity > 0) && !this.IsPlayingAlert && !this.IsHUDDown)
        {
            this.PlayMessages();
            this.IsPlayingAlert = true;
        }
    }

    void IncrementAlertSeverity(string AlertKey)
    {
        switch (AlertKey)
        {
            case "engine":
                this.Alerts.setParameterValue("engine_severity", ++this.EngineSeverity);
                break;
            case "food":
                this.Alerts.setParameterValue("food_severity", ++this.FoodSeverity);
                break;
            case "air":
                this.Alerts.setParameterValue("air_severity", ++this.AirSeverity);
                break;
            default:
                break;
        }
    }

    void PlayMessages()
    {
        this.Alerts.setParameterValue("engine_severity", this.EngineSeverity);
        this.Alerts.setParameterValue("food_severity", this.FoodSeverity);
        this.Alerts.setParameterValue("air_severity", this.AirSeverity);

        this.EngineSeverity = 0;
        this.FoodSeverity = 0;
        this.AirSeverity = 0;

        this.Alerts.start();
    }

    public void SetAlertActive(string AlertKey)
    {
        switch (AlertKey)
        {
            case "engine":
                this.EngineSeverity = 1;
                this.Alerts.setParameterValue("engine_severity", this.EngineSeverity);
                break;
            case "food":
                this.FoodSeverity = 1;
                this.Alerts.setParameterValue("food_severity", this.FoodSeverity);
                break;
            case "air":
                this.AirSeverity = 1;
                this.Alerts.setParameterValue("air_severity", this.AirSeverity);
                break;
            default:
                break;
        }
    }

    public void UnsetAlert(string AlertKey)
    {
        switch (AlertKey)
        {
            case "engine":
                this.EngineSeverity = 0;
                this.Alerts.setParameterValue("engine_severity", this.EngineSeverity);
                break;
            case "food":
                this.FoodSeverity = 0;
                this.Alerts.setParameterValue("food_severity", this.FoodSeverity);
                break;
            case "air":
                this.AirSeverity = 0;
                this.Alerts.setParameterValue("air_severity", this.AirSeverity);
                break;
            default:
                break;
        }
    }
}
