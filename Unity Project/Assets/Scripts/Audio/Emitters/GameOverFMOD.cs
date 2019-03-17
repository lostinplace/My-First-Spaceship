using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFMOD : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter GameoverMusicEmitter;
    
    // Start is called before the first frame update
    void Start()
    {
        GameoverMusicEmitter = gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void FadeOutGameOverMusic()
    {
        GameoverMusicEmitter.Stop();
    }
}
