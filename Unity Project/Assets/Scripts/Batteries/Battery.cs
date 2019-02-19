using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Battery : Lockable, Handleable.HandleableItem
{
  public Plug currentPlug = null;
  public Plug potentialPlug = null;
  
  public bool isBeingHeld = false;

  private static PlayerState myPlayerState;
  
  public void OnPickup()
  {
    Unlock();
    isBeingHeld = true;
    if (currentPlug) currentPlug.DetachBattery();
    myPlayerState.BatteriesHeld++;
    
  }

  public static Dictionary<EnergyState, Material> BatteryMaterials;

  public void OnDrop()
  {
    isBeingHeld = false;
    if (potentialPlug) potentialPlug.ProcessCollision(this);
    myPlayerState.BatteriesHeld--;
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
    myPlayerState = GameObject.FindObjectOfType<PlayerState>();
  }

  private void Update()
  {
    var myMaterial = BatteryMaterials[energyLevel];
    myRenderer.material = myMaterial;
  }
}
