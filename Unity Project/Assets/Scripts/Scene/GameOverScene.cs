using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class GameOverScene : MonoBehaviour
{
  public DeviceBase myDevice;

  public UnityEvent FadeGameOverMusic;
  // Start is called before the first frame update
  void Start()
  {
      SteamVR_Fade.Start(Color.clear, 0f);
  }

  // Update is called once per frame
  void Update()
  {
    if (!myDevice.isActive)
    {
      FadeGameOverMusic.Invoke();
      SceneChanger.GoToMainMenu();
    }
  }
}
