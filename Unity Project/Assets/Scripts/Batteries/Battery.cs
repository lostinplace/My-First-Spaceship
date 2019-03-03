using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Battery : Lockable, Handleable.HandleableItem
{
  public Plug currentPlug = null;
  public Plug potentialPlug = null;
  
  public bool isBeingHeld = false;

  private PlayerState playerState => SceneChanger.playerState;
  
  public void OnPickup()
  {
    Unlock();
    isBeingHeld = true;
    if (currentPlug) currentPlug.DetachBattery();
    if(playerState) playerState.BatteriesHeld++;   
  }

  Color getEmissiveColor()
  {
    return new Color(2.0f * (1 - chargeRatio), 2.0f * chargeRatio, 0);
  }

  public static Dictionary<EnergyState, Material> BatteryMaterials;

  public void OnDrop()
  {
    isBeingHeld = false;
    if (potentialPlug) potentialPlug.ProcessCollision(this);
    if(playerState) playerState.BatteriesHeld--;
    SceneChanger.CleanupList.Add(this.gameObject);
  }
  
  public GameObject GetGameObject() {
    return gameObject;
  }

  private MeshRenderer myRenderer;

  void Start()
  {
    Handleable.InitializeHandleableItem(this);
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

  private void Update()
  {
    //var myMaterial = BatteryMaterials[energyLevel];
    //myRenderer.material = myMaterial;
    
    if (this.isDead)
    {
      myRenderer.material.DisableKeyword("_EMISSION");
      return;
    }

    var color = getEmissiveColor();

    if (charging && chargeRatio != 1) {
      var iteration = Mathf.Floor(Time.fixedTime / chargingFlashTime);
      var nowColor = iteration % 2 == 0 ? new Color() : color;
      myRenderer.material.SetColor("_EmissionColor", nowColor);
      return;
    }

    myRenderer.material.SetColor("_EmissionColor", color);
   
  }
}
