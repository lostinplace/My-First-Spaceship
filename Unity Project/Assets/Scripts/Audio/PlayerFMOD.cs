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

    public bool IsTitleScene, IsGameEnvironment, IsGameOver;
    private bool IsPlayingTitleMusic, IsPlayingAmbience;

    // Start is called before the first frame update
    void Start()
    {
        if (this.IsGameEnvironment)
        {
            this.PlayShipAmbience();
        }
        else if (this.IsTitleScene)
        {
            this.PlayTitleMusic();
        }
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

    public void PlayTitleMusic()
    {
        this.TitleMusic = FMODUnity.RuntimeManager.CreateInstance(TitleMusicEvent);
        this.TitleMusic.start();
    }

    public void StopTitleMusic()
    {
        this.TitleMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        this.TitleMusic.release();
    }

    public void PlayShipAmbience()
    {
        this.Ambience = FMODUnity.RuntimeManager.CreateInstance(AmbienceEvent);
        this.Ambience.start();
    }

    public void StopShipAmbience()
    {
        this.Ambience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        this.Ambience.release();
    }
}
