using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Valve.VR;
using System.Diagnostics;

public static class SceneChanger
{
  // scene filenames

  private static readonly string MAIN_MENU_VR = "main_menu";
  private static readonly string MAIN_SCENE_VR = "main_scene";
  private static readonly string GAME_OVER_VR = "game_over";

  private static readonly string MAIN_MENU_NO_VR = "no_vr_main_menu";
  private static readonly string MAIN_SCENE_NO_VR = "no_vr_main_scene";
  private static readonly string GAME_OVER_NO_VR = "no_vr_game_over";
    
  private static string MAIN_MENU = MAIN_MENU_VR;
  private static string MAIN_SCENE = MAIN_SCENE_VR;
  private static string GAME_OVER = GAME_OVER_VR;

  public static string gameOverMessage;

  private static PlayerState _playerState;
  private static bool blockPlayerState = false;
  private static bool _isNoVR = false;

  public static HashSet<GameObject> CleanupList { get; set; } = new HashSet<GameObject>();
  private static bool _isFadingTitleMusic = false;

  public static bool isNoVR
  {
     get => _isNoVR;
     set
     {
       _isNoVR = value;
       if(_isNoVR == true)
       {
          MAIN_MENU = MAIN_MENU_NO_VR;
          MAIN_SCENE = MAIN_SCENE_NO_VR;
          GAME_OVER = GAME_OVER_NO_VR;
       }
     }
  }

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
    if (isNoVR == false)
      Valve.VR.SteamVR_LoadLevel.Begin(MAIN_SCENE);
    else
      SceneManager.LoadScene(MAIN_SCENE);
  }

  public static void GameOver( string message )
  {
    gameOverMessage = message;
    blockPlayerState = true;
    var leftovers = SceneManager.GetActiveScene().GetRootGameObjects();

    if (isNoVR == false)
      Valve.VR.SteamVR_LoadLevel.Begin(GAME_OVER);
    else
      SceneManager.LoadScene(GAME_OVER);
    
    GameObject.Destroy(playerState);
    SceneChanger.playerState = null;
    
    blockPlayerState = false;
    CleanupList.ToList().ForEach(GameObject.DestroyImmediate);
  }

  public static void GoToMainMenu() {

        if (isNoVR == false)
            Valve.VR.SteamVR_LoadLevel.Begin(MAIN_MENU);
        else
        {
            UnityEngine.Debug.Log("Going to main menu via LoadScene");
            SceneManager.LoadScene(MAIN_MENU);
        }
  }
}
