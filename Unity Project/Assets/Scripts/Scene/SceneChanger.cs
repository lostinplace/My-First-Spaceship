using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Valve.VR;

public static class SceneChanger
{
  // scene filenames
  private static string MAIN_MENU = "main_menu";
  private static string MAIN_SCENE = "main_scene";
  private static string GAME_OVER = "game_over";

  public static string gameOverMessage;

  private static PlayerState _playerState;
  private static bool blockPlayerState = false;

  public static bool hasWon
  {
    get => _playerState.HasWon;
  }

  public static HashSet<GameObject> CleanupList { get; set; } = new HashSet<GameObject>();

  private static bool _isFadingTitleMusic = false;
  public static bool isFadingTitleMusic
  {
    get => _isFadingTitleMusic;
  }

  public static bool isSceneTitle
  {
    get => SceneManager.GetActiveScene().name == MAIN_MENU;
  }

  public static bool isSceneGameEnv
  {
    get => SceneManager.GetActiveScene().name == MAIN_SCENE;
  }

  public static bool isSceneGameOver
  {
    get => SceneManager.GetActiveScene().name == GAME_OVER;
  }

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

  public static void LoadGame()
  {  
    _isFadingTitleMusic = true;
    Valve.VR.SteamVR_LoadLevel.Begin(MAIN_SCENE);
  }

  public static void GameOver( string message )
  {
    gameOverMessage = message;
    blockPlayerState = true;
    var leftovers = SceneManager.GetActiveScene().GetRootGameObjects();

    Valve.VR.SteamVR_LoadLevel.Begin(GAME_OVER);
    
    GameObject.Destroy(playerState);
    SceneChanger.playerState = null;
    
    blockPlayerState = false;
    CleanupList.ToList().ForEach(GameObject.DestroyImmediate);
  }

  public static void GoToMainMenu() {

    Valve.VR.SteamVR_LoadLevel.Begin(MAIN_MENU);
  }
}
