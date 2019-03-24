using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFMOD : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter GameLostEmitter, GameWonEmitter;

    [FMODUnity.EventRef]
    public string GameWonChimeEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        GameLostEmitter = gameObject.transform.Find("GameLost").GetComponent<FMODUnity.StudioEventEmitter>();
        GameWonEmitter = gameObject.transform.Find("GameWon").GetComponent<FMODUnity.StudioEventEmitter>();

        if (SceneChanger.hasWon)
        {
            GameWonEmitter.Play();
            PlayGameWonEvent();
        }
        else
        {
            GameLostEmitter.Play();
        }
    }

    private void PlayGameWonEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(GameWonChimeEvent);
    }
}
