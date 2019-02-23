﻿using System.Collections;
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

    public bool isEngineOffline;
    public bool isAirOffline;
    public bool isFoodOffline;
    public bool isHUDOffline;

    private bool HasOfflineComponent()
    {
        return this.isEngineOffline || this.isAirOffline || this.isFoodOffline;
    }

    // TODO: pull in Chris W alarm state properties from PlayerState
    private void SetAlarmStates() {

    }

    // Start is called before the first frame update
    void Start()
    {
        this.Alerts = FMODUnity.RuntimeManager.CreateInstance(AlertsEvent);
        this.IsPlayingAlert = false;
        this.playerState = this.GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        Alerts.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        this.SetAlarmStates();

        Alerts.getPlaybackState(out PlaybackState);
        this.IsPlayingAlert = this.PlaybackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;

        if (this.HasOfflineComponent() && !this.IsPlayingAlert && !this.isHUDOffline)
        {
            this.PlayMessages();
            this.IsPlayingAlert = true;
        }
    }

    void PlayMessages()
    {
        this.Alerts.setParameterValue("is_engine_offline", this.isEngineOffline ? 1 : 0);
        this.Alerts.setParameterValue("is_air_offline", this.isAirOffline ? 1 : 0);
        this.Alerts.setParameterValue("is_food_offline", this.isFoodOffline ? 1 : 0);

        this.Alerts.start();
    }
}
