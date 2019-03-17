using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFMOD : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string DangerAirEvent;
    private bool HasTriggeredSuffocation;

    [FMODUnity.EventRef]
    public string GameOverSnapshotEvent;
    private bool HasTriggeredGameOver;

    private PlayerState playerState => SceneChanger.playerState;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        // Reset trigger states
        if (SceneChanger.isSceneTitle)
        {
            HasTriggeredGameOver = false;
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
            HasTriggeredGameOver = true;
        }
    }
}
