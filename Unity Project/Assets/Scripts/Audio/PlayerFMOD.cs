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
    public string TitleMusicEvent;
    FMOD.Studio.EventInstance TitleMusic;
    FMOD.Studio.PLAYBACK_STATE TitleMusicPlaybackState;

    [FMODUnity.EventRef]
    public string FailstateEvent;
    FMOD.Studio.EventInstance Failstate;

    private bool IsPlayingTitleMusic, IsPlayingAmbience;

    private bool HasTriggeredAmbienceStart;

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
    }

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
