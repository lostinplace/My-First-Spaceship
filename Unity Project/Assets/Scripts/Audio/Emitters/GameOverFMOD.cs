using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFMOD : MonoBehaviour
{
    private PlayerState playerState => SceneChanger.playerState;

    private FMODUnity.StudioEventEmitter GameoverMusicEmitter;

    [FMODUnity.EventRef]
    public string GameWonChimeEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        GameoverMusicEmitter = gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
        //TODO: Set FMOD parameter for has_won

        if (playerState && playerState.HasWon)
        {
            PlayGameWonEvent();
        }
    }

    private void PlayGameWonEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(GameWonChimeEvent);
    }

    public void FadeOutGameOverMusic()
    {
        GameoverMusicEmitter.Stop();
    }
}
