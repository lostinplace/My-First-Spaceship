using System;
using UnityEngine;
using Valve.VR;

public enum HoldingTypes
{
  BATTERY,
  PIPE
}

public class PlayerState : MonoBehaviour
{  
  private bool gameOver = false;
  private bool hasWon = false;
  private string gameOverMessage;
 
  public float currentTripTime
  {
    get;
    set;
  }

  public float foodSupplyInSeconds
  {
    get;
    set;
  }

  public float airSupplyInSeconds
  {
    get;
    set;
  }

  public float distanceTraveledInSeconds
  {
    get;
    set;
  }

  private SpaceshipSettings settings;

  public DeviceBase Air
  {
    get => SceneChanger.settings ? SceneChanger.settings.air : null;
  }

  public  DeviceBase Engine
  {
    get => SceneChanger.settings ? SceneChanger.settings.engine : null;
  }

  public  DeviceBase Fridge
  {
    get => SceneChanger.settings ? SceneChanger.settings.fridge : null;
  }

  public  DeviceBase Monitor
  {
    get => SceneChanger.settings ? SceneChanger.settings.monitor : null;
  }

  private short _pipesHeld;

  public short PipesHeld
  {
    get => _pipesHeld;
    set => _pipesHeld = (short)Mathf.Clamp(value, 0, 2);
  }

  private short _batteriesHeld;
  public short BatteriesHeld
  {
    get => _batteriesHeld;
    set => _batteriesHeld= (short)Mathf.Clamp(value, 0, 2);
  }

  private bool exited
  {
    get;
    set;
  }

  public bool _exited
  {
    get => exited;
  }

  public bool IsGameOver
  {
    get => gameOver;
  }

  public bool HasWon
  {
    get => hasWon;
  }
  
  private float curAirlessTime;

  public bool AirIsActive
  {
    get => Air && Air.isActive;
  }

  public bool FridgeIsActive
  {
    get => Fridge && Fridge.isActive;
  }

  public bool EngineIsActive
  {
    get => Engine && Engine.isActive;
  }

  public bool MonitorIsActive
  {
    get => Monitor && Monitor.isActive;
  }

  public bool IsSuffocating
  {
    get => suffocating;
  }

  public bool IsHungry
  {
    get => hungry;
  }

  // todo: subscribe to some player controller event
  public void ReceiveFood()
  {
    if (!SceneChanger.settings) return;
    foodSupplyInSeconds = SceneChanger.settings.startingFoodSupplyInSeconds;
  }

  private bool suffocating = false;
  private bool hungry = false;

  // Start is called before the first frame update
  void Start()
  {
    if (!SceneChanger.settings) return;
    exited = false;

    ReceiveFood();
    airSupplyInSeconds = SceneChanger.settings.airSupplyInSeconds;
    currentTripTime = 0f;
  }
  
  // Update is called once per frame
  void Update()
  {
    if (gameOver || !SceneChanger.settings) {
      return;
    }

    var delta = Time.deltaTime;

    if (AirIsActive)
    {
      airSupplyInSeconds += delta;
      airSupplyInSeconds = Mathf.Min(airSupplyInSeconds, SceneChanger.settings.airSupplyInSeconds);
      SteamVR_Fade.Start(Color.clear, 0.5f);
      suffocating = false;
    }
    else
    {
      airSupplyInSeconds -= delta;
      if(airSupplyInSeconds <= 0 - SceneChanger.settings.lungCapacityInSeconds)
      {
        TriggerEndgame("You passed out from lack of oxygen", true);
      }

      if (airSupplyInSeconds < 0)
      {
        if (!suffocating) SteamVR_Fade.Start(SceneChanger.settings.SuffocationColor, SceneChanger.settings.lungCapacityInSeconds);
        suffocating = true;
      }
    }

    foodSupplyInSeconds -= delta;

    hungry = foodSupplyInSeconds < (SceneChanger.settings.startingFoodSupplyInSeconds / 4);

    if(foodSupplyInSeconds <= 0)
      TriggerEndgame("You passed out from hunger", true);

    if (EngineIsActive)
    {
      distanceTraveledInSeconds += delta;
      if(distanceTraveledInSeconds >= SceneChanger.settings.destinationDistanceInSeconds)
      {
        TriggerEndgame("You successfully kept your \"new\" spaceship together long enough to get home!", false);
      }
    }

    currentTripTime += delta;
    if(currentTripTime >= SceneChanger.settings.timeLimitInSeconds)
      TriggerEndgame("Your engine gave out", true);
  }

  void TriggerEndgame(string message, bool hasLost = true)
  {
    if (exited) return;
    exited = true;
    gameOver = hasLost;
    hasWon = !hasLost;
    gameOverMessage = message;
    SteamVR_Fade.Start(Color.black, 2.0f);
    SceneChanger.GameOver(gameOverMessage);
  }

  
}
