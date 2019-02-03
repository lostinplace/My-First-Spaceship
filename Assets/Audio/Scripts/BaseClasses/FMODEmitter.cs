using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEmitter : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string EmitterEvent;
    FMOD.Studio.EventInstance Emitter;
    FMOD.Studio.PLAYBACK_STATE PlaybackState;

    public bool IsOffline;
    private bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        this.isPlaying = false;
        this.StartEmitter();
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdatePlaybackState();
    }

    private void UpdatePlaybackState()
    {
        this.Emitter.getPlaybackState(out PlaybackState);
        this.isPlaying = this.PlaybackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

    protected void StartEmitter()
    {
        this.Emitter = FMODUnity.RuntimeManager.CreateInstance(EmitterEvent);
        this.Emitter.setParameterValue("is_offline", this.IsOffline ? 1 : 0);
        this.Emitter.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        this.Emitter.start();
    }

    protected void KillEmitter()
    {
        if (this.isPlaying)
        {
            Emitter.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            Emitter.release();
        }
    }
}
