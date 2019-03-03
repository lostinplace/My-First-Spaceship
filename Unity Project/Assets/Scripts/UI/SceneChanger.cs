﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

public static class SceneChanger
{
  public static string gameOverMessage;

  private static PlayerState _playerState;
  private static bool blockPlayerState = false;

  public static List<GameObject> CleanupList = new List<GameObject>();

  public static PlayerState playerState
  {
    get
    {
      if (blockPlayerState) return null;
      if (!_playerState)
      {
        playerState = GameObject.FindObjectOfType<PlayerState>();
      }
      return _playerState;
    }
    private set => SceneChanger._playerState = value;
  }

  private static SpaceshipSettings _settings;
  public static SpaceshipSettings settings
  {
    get
    {
      if (!_settings)
      {
        _settings = GameObject.FindObjectOfType<SpaceshipSettings>();
      }
      return _settings;
    }
    private set => SceneChanger._settings = value;
  }

  public static void LoadGame() {
    
    Valve.VR.SteamVR_LoadLevel.Begin("new_layout");
  }

  public static void GameOver( string message )
  {
    gameOverMessage = message;
    blockPlayerState = true;
    var leftovers = SceneManager.GetActiveScene().GetRootGameObjects();
    foreach (var item in CleanupList)
    {
      GameObject.DestroyImmediate(item);
    }
    CleanupList.Clear();

    Valve.VR.SteamVR_LoadLevel.Begin("GameOver");
    
    GameObject.Destroy(playerState);
    SceneChanger.playerState = null;
    
    blockPlayerState = false;
  }

  public static void GoToMainMenu() {

    Valve.VR.SteamVR_LoadLevel.Begin("main_menu");
  }
}
