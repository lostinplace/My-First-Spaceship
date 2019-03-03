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

  private PlayerState playerState => SceneChanger.playerState;

  private bool HasOfflineComponent => playerState && !(playerState.EngineIsActive && playerState.FridgeIsActive && playerState.AirIsActive);

    private void SetAlarmStates()
    {
        Alerts.setParameterValue("is_engine_offline", playerState && playerState.EngineIsActive ? 0 : 1);
        Alerts.setParameterValue("is_air_offline", playerState && playerState.AirIsActive ? 0 : 1);
        Alerts.setParameterValue("is_food_offline", playerState && playerState.FridgeIsActive ? 0 : 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        Alerts = FMODUnity.RuntimeManager.CreateInstance(AlertsEvent);
        IsPlayingAlert = false;
    }

  // Update is called once per frame
  void Update()
  {
    if (!playerState) this.Alerts.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    Alerts.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

    this.SetAlarmStates();

    Alerts.getPlaybackState(out PlaybackState);
    this.IsPlayingAlert = this.PlaybackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;

    if (this.HasOfflineComponent && !this.IsPlayingAlert)
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
