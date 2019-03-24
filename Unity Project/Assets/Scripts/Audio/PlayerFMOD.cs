using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFMOD : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string DangerAirEvent;
    private bool HasTriggeredSuffocation;

    [FMODUnity.EventRef]
    public string DangerHungerEvent;
    private bool HasTriggeredHunger;

    [FMODUnity.EventRef]
    public string GameOverSnapshotEvent;
    private bool HasTriggeredGameOver;

    [FMODUnity.EventRef]
    public string GameWonEvent;
    private bool HasTriggeredGameWon;

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
            HasTriggeredGameWon = false;
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

        if (playerState && playerState.IsHungry && !HasTriggeredHunger)
        {
            FMODUnity.RuntimeManager.PlayOneShot(DangerHungerEvent);
            HasTriggeredHunger = true;
        }
        else if (playerState && !playerState.IsHungry)
        {
            HasTriggeredHunger = false;
        }

        if (playerState && playerState.IsGameOver && !HasTriggeredGameOver)
        {
            FMODUnity.RuntimeManager.PlayOneShot(GameOverSnapshotEvent);
            HasTriggeredGameOver = true;
        }
        else if (playerState && playerState.HasWon && !HasTriggeredGameWon)
        {
            if (playerState.MonitorIsActive)
            {
                FMODUnity.RuntimeManager.PlayOneShot(GameWonEvent);
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(GameOverSnapshotEvent);
            }

            HasTriggeredGameWon = true;
        }
    }
}
