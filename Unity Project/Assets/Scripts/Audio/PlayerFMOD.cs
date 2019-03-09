using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFMOD : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string AmbienceEvent;
    FMOD.Studio.EventInstance Ambience;
    FMOD.Studio.PLAYBACK_STATE AmbiencePlaybackState;

    [FMODUnity.EventRef]
    public string TitleMusicEvent;
    FMOD.Studio.EventInstance TitleMusic;
    FMOD.Studio.PLAYBACK_STATE TitleMusicPlaybackState;

    [FMODUnity.EventRef]
    public string DangerAirEvent;
    private bool HasTriggeredSuffocation;

    private bool IsPlayingTitleMusic, IsPlayingAmbience;

    private bool HasTriggeredAmbienceStart;

    private PlayerState playerState => SceneChanger.playerState;

    // Start is called before the first frame update
    void Start()
    {
        this.TitleMusic = FMODUnity.RuntimeManager.CreateInstance(TitleMusicEvent);
        this.Ambience = FMODUnity.RuntimeManager.CreateInstance(AmbienceEvent);

        if (SceneChanger.isSceneGameEnv)
        {
            this.PlayShipAmbience();
        }
        else if (SceneChanger.isSceneTitle)
        {
            this.PlayTitleMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ambience.getPlaybackState(out AmbiencePlaybackState);
        this.IsPlayingAmbience = this.AmbiencePlaybackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;

        TitleMusic.getPlaybackState(out TitleMusicPlaybackState);
        this.IsPlayingTitleMusic = this.TitleMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;

        if (SceneChanger.isFadingTitleMusic && this.IsPlayingTitleMusic)
        {
            this.StopTitleMusic();
        }

        if (SceneChanger.isSceneGameEnv && !this.IsPlayingAmbience && !this.HasTriggeredAmbienceStart)
        {
            this.PlayShipAmbience();
            this.HasTriggeredAmbienceStart = true;
        }
        else if (SceneChanger.isSceneTitle && !this.IsPlayingTitleMusic && !SceneChanger.isFadingTitleMusic)
        {
            this.PlayTitleMusic();
        }

        if (playerState && playerState.IsSuffocating && !HasTriggeredSuffocation)
        {
          FMODUnity.RuntimeManager.PlayOneShot(DangerAirEvent);
          HasTriggeredSuffocation = true;
        }
        else if (playerState && !playerState.IsSuffocating)
        {
          HasTriggeredSuffocation = false;
        }
    }

    public void PlayTitleMusic()
    {
        this.TitleMusic.start();
    }

    public void StopTitleMusic()
    {
        this.TitleMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayShipAmbience()
    {
        this.Ambience.start();
    }

    public void StopShipAmbience()
    {
        this.Ambience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
