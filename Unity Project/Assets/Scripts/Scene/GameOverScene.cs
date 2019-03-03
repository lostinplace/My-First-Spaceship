using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameOverScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SteamVR_Fade.Start(Color.clear, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
