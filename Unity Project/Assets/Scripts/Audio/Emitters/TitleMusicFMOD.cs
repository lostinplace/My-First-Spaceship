using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMusicFMOD : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter TitleMusicEmitter;
    
    // Start is called before the first frame update
    void Start()
    {
        TitleMusicEmitter = gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void FadeOutTitleMusic()
    {
        TitleMusicEmitter.Stop();
    }
}
