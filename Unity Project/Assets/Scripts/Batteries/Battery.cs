using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Battery : Lockable, Handleable.HandleableItem
{
  public Plug currentPlug = null;
  public Plug potentialPlug = null;
  
  public bool isBeingHeld { get; set; }

  private PlayerState playerState => SceneChanger.playerState;
  
  public void OnPickup()
  {
    SceneChanger.CleanupList.Add(this.gameObject);
    Unlock();
    isBeingHeld = true;
    if (currentPlug) currentPlug.DetachBattery();
    if(playerState) playerState.BatteriesHeld++;
    currentPlug = null;
  }

  Color getEmissiveColor()
  {
    return new Color(2.0f * (1 - chargeRatio), 2.0f * chargeRatio, 0);
  }

  public static Dictionary<EnergyState, Material> BatteryMaterials { get; set; }

  public void OnDrop()
  {
    isBeingHeld = false;
    if (potentialPlug) potentialPlug.ProcessCollision(this);
    if(playerState) playerState.BatteriesHeld--;
  }
  
  public GameObject GetGameObject() {
    return gameObject;
  }

  private MeshRenderer myRenderer;

  void Start()
  {
    Handleable.InitializeHandleableItem(this);
    StartedCharging = false;
    isBeingHeld = false;
    BatteryMaterials = new Dictionary<EnergyState, Material>()
    {
      {EnergyState.GOOD, Resources.Load<Material>("battery_good")},
      {EnergyState.MEDIUM, Resources.Load<Material>("battery_medium")},
      {EnergyState.BAD, Resources.Load<Material>("battery_bad")},
      {EnergyState.DEAD, Resources.Load<Material>("battery_dead")}
    };
    myRenderer = this.GetComponent<MeshRenderer>();
    var settings = GameObject.FindObjectOfType<SpaceshipSettings>();
    if(settings)
      InitializeWithSettings(settings);
  }

  private void InitializeWithSettings(SpaceshipSettings settings)
  {
    lifetimeInSeconds = lifetimeInSeconds == 0 ? settings.defaultBatteryLifetimeInSeconds : lifetimeInSeconds;
    currentChargeInSeconds = currentChargeInSeconds == 0 ? settings.defaultBatteryChargeInSeconds : currentChargeInSeconds;
    this.maxChargeInSeconds = maxChargeInSeconds == 0 ? settings.defaultBatteryChargeInSeconds : maxChargeInSeconds;
  }

  public static float chargingFlashTime = 0.25f;

  private bool playedChargingSoundForThisPlug { get; set; }

  public void setStartedCharging()
  {
    playedChargingSoundForThisPlug = true;
  }

  private void Update()
  {
    if (isDead)
    {
      myRenderer.material = BatteryMaterials[EnergyState.DEAD];
      myRenderer.material.DisableKeyword("_EMISSION");
      return;
    }

    if(currentPlug && currentPlug.myDevice && !currentPlug.myDevice.currentBattery)
    {
      currentPlug.myDevice.AttachBattery(this);
    }

    var color = getEmissiveColor();

    if (charging) {
      if (!playedChargingSoundForThisPlug)
      {
        batteryChargeAudio.Invoke();
        playedChargingSoundForThisPlug = true;
      }

      if(chargeRatio != 1)
      {
        var iteration = Mathf.Floor(Time.fixedTime / chargingFlashTime);
        var nowColor = iteration % 2 == 0 ? new Color(0,0,0) : color;
        myRenderer.material.SetColor("_EmissionColor", nowColor);
      }
    } else
    {
      playedChargingSoundForThisPlug = false;
    }

    myRenderer.material.SetColor("_EmissionColor", color);
  }
}
