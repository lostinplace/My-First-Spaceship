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

    [FMODUnity.EventRef]
    public string GameOverSnapshotEvent;
    private bool HasTriggeredGameOver;

    [FMODUnity.EventRef]
    public string GameOverMusicEvent;
    FMOD.Studio.EventInstance GameOverMusic;

    private bool IsPlayingTitleMusic, IsPlayingAmbience, IsPlayingGameOverMusic;

    private bool HasTriggeredAmbienceStart, HasTriggeredGameOverMusic;

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

        // Reset trigger states
        if (SceneChanger.isSceneTitle)
        {
            HasTriggeredGameOver = false;
            HasTriggeredAmbienceStart = false;
            HasTriggeredGameOverMusic = false;
        }

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

        if (playerState && playerState.IsGameOver && !HasTriggeredGameOver)
        {
            FMODUnity.RuntimeManager.PlayOneShot(GameOverSnapshotEvent);
            StopShipAmbience();
            HasTriggeredGameOver = true;
        }

        if (SceneChanger.isSceneGameOver && !HasTriggeredGameOverMusic)
        {
            PlayGameOverMusic();
            this.HasTriggeredGameOverMusic = true;
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

    public void PlayGameOverMusic()
    {
        this.GameOverMusic = FMODUnity.RuntimeManager.CreateInstance(GameOverMusicEvent);
        this.GameOverMusic.start();
        this.GameOverMusic.release();
    }
}
