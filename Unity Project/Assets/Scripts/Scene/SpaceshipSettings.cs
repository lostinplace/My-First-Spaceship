using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipSettings : MonoBehaviour
{

    #region Batteries
        public float defaultBatteryChargeInSeconds = 10;
        public float defaultBatteryLifetimeInSeconds = 20;
    #endregion

    #region Pipes
        public float defaultPipeIntegrity = 1500;
        public float defaultHeatDamageScalingFactor = 0.5f;
        public float maxPipeHeat = 300;
        public float HeatLossPerSecond = 5;
  #endregion

  #region Scene

  public float timeLimitInSeconds = 180f;

  public float destinationDistanceInSeconds = 60f;

  public float startingFoodSupplyInSeconds = 45f;

  public float airSupplyInSeconds = 10f;

  public float lungCapacityInSeconds = 10f;

  public DeviceBase engine;

  public DeviceBase air;

  public DeviceBase fridge;

  public DeviceBase monitor;

  public Color SuffocationColor;
  
  #endregion

  // Start is called before the first frame update
  void Start()
  {
    Pipe.MaxIntegrity = defaultPipeIntegrity;
    Pipe.damageScalingBase = defaultHeatDamageScalingFactor;
    Pipe.MaxHeat = maxPipeHeat;
    Pipe.HeatLossPerSecond = HeatLossPerSecond;

    if(SuffocationColor == null) SuffocationColor = Color.red;
    
    var playerState = SceneChanger.playerState; 
  }
}
