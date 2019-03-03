using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Monitor : MonoBehaviour
{
  private PlayerState playerState => SceneChanger.playerState ;
  private DeviceBase myDevice;

  protected StatusBar hungerBar, engineBar, airBar;

  void Start()
  {
    
    myDevice = this.GetComponent<DeviceBase>();
    var bars = this.GetComponentsInChildren<StatusBar>();

    hungerBar = bars.First(x => x.AssociatedState == StatusBarState.FOOD);
    hungerBar.scalingFunc = () => SceneChanger.playerState.foodSupplyInSeconds / SceneChanger.settings.startingFoodSupplyInSeconds;

    engineBar = bars.First(x => x.AssociatedState == StatusBarState.ENGINE);
    engineBar.scalingFunc = () => 1 - (SceneChanger.playerState.currentTripTime / SceneChanger.settings.timeLimitInSeconds);

    airBar = bars.First(x => x.AssociatedState == StatusBarState.AIR);
    airBar.scalingFunc = () => Math.Max(0, SceneChanger.playerState.airSupplyInSeconds) / SceneChanger.settings.airSupplyInSeconds;
  }

  void Update() {
    
    if (myDevice)
    {
      this.GetComponentInChildren<MeshRenderer>().enabled = myDevice.isActive;
    }
  }

}
