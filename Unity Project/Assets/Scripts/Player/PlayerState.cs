using UnityEngine;


public class PlayerState : MonoBehaviour
{  
  private bool gameOver = false;
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
    get => SceneChanger.settings.air;
  }

  public  DeviceBase Engine
  {
    get => SceneChanger.settings.engine;
  }

  public  DeviceBase Fridge
  {
    get => SceneChanger.settings.fridge;
  }

  public  DeviceBase Monitor
  {
    get => SceneChanger.settings.monitor;
  }

  public short PipesHeld = 0;

  public short BatteriesHeld = 0;

  private bool exited
  {
    get;
    set;
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

  // todo: subscribe to some player controller event
  public void ReceiveFood()
  {
    foodSupplyInSeconds = SceneChanger.settings.startingFoodSupplyInSeconds;
  }


  // Start is called before the first frame update
  void Start()
  {
    exited = false;

    ReceiveFood();
    airSupplyInSeconds = SceneChanger.settings.airSupplyInSeconds;
    currentTripTime = 0f;
  }

  // Update is called once per frame
  void Update()
  {
    if (gameOver) {
      return;
    }

    var delta = Time.deltaTime;

    if (AirIsActive)
    {
      airSupplyInSeconds += delta;
      airSupplyInSeconds = Mathf.Min(airSupplyInSeconds, SceneChanger.settings.airSupplyInSeconds);
    }
    else
    {
      airSupplyInSeconds -= delta;
      if(airSupplyInSeconds <= 0 - SceneChanger.settings.lungCapacityInSeconds)
      {
        TriggerEndgame("You passed out from lack of oxygen");
      }
    } 

    foodSupplyInSeconds -= delta;

    if(foodSupplyInSeconds <= 0)
      TriggerEndgame("You passed out from hunger");

    if (EngineIsActive)
    {
      distanceTraveledInSeconds += delta;
      if(distanceTraveledInSeconds >= SceneChanger.settings.destinationDistanceInSeconds)
        TriggerEndgame("You successfully kept your \"new\" spaceship together long enough to get home!");
    }

    currentTripTime += delta;
    if(currentTripTime >= SceneChanger.settings.timeLimitInSeconds)
      TriggerEndgame("Your engine gave out");
  }

  void TriggerEndgame(string message)
  {
    if (exited) return;
    exited = true;
    gameOver = true;
    gameOverMessage = message;

    SceneChanger.GameOver(gameOverMessage);
  }

  
}
