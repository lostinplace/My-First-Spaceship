using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Plug : MonoBehaviour
{
  public Battery currentBattery;
  public DeviceBase myDevice;
  public bool StartsWithBattery = true;
  private static PlayerState playerState => SceneChanger.playerState;

  private MeshRenderer placeholderRenderer;
  public GameObject BatteryPlaceholder;

  public UnityEvent batteryPlugInAudio;
  private bool HasPlayedPlugInAudio = false;

  private void Start()
  {
    if (StartsWithBattery)
    {
      var prefab = Resources.Load("RuntimeBattery");
      var obj = (GameObject)Instantiate(prefab);
      var myBattery = obj.GetComponent<Battery>();
      
      if (myDevice && myDevice.overrideStartingBatteryChargeInSeconds != 0)
        myBattery.currentChargeInSeconds = myDevice.overrideStartingBatteryChargeInSeconds;
      
      if (myDevice && myDevice.overrideStartingBatteryMaxChargeInSeconds != 0)
        myBattery.maxChargeInSeconds = myDevice.overrideStartingBatteryMaxChargeInSeconds;
      
      if (myDevice && myDevice.overrideStartingBatteryLifeTimeInSeconds != 0)
        myBattery.lifetimeInSeconds= myDevice.overrideStartingBatteryLifeTimeInSeconds;

      myBattery.setStartedCharging();

      HasPlayedPlugInAudio = true;
      
      AttachBattery(myBattery);
      
      HasPlayedPlugInAudio = false;
    }
    
    placeholderRenderer = BatteryPlaceholder.GetComponent<MeshRenderer>();
  }

  private void OnTriggerEnter(Collider other) {
    
    var battery = other.GetComponent<Battery>();
    ProcessCollision(battery);
  }


  public bool AttachBattery(Battery aBattery)
  {
    if (currentBattery) return false;
    aBattery.Lock();
    aBattery.currentPlug = this;
    
    var myTransform = this.gameObject.transform;
    aBattery.transform.SetPositionAndRotation(myTransform.position, myTransform.rotation);
    myDevice.AttachBattery(aBattery);

    if (!HasPlayedPlugInAudio)
    {
      batteryPlugInAudio.Invoke();
    }

    return true;
  }

  internal void DetachBattery()
  {
    myDevice.RemoveBattery();
    currentBattery = null;
  }

  public bool ConnectBattery(Battery aBattery)
  {
    if (currentBattery) return false;
    currentBattery = aBattery;
    if (myDevice) myDevice.AttachBattery(aBattery);

    return true;
  }

  public void DisconnectBattery()
  {
    currentBattery = null;
  }

  public void ProcessCollision(Battery battery)
  {
    
    if (!battery) return;

    if(!battery.isBeingHeld && !currentBattery)
    {
      AttachBattery(battery);
    }
    else
    {
      battery.potentialPlug = this;
    }     
  }
  private void OnTriggerExit(Collider other)
  {
    Battery battery = other.gameObject.GetComponent<Battery>();
    if (!battery) return;
    battery.potentialPlug = null;
  }

  void Update()
  {
    placeholderRenderer.enabled = playerState.BatteriesHeld > 0 && !currentBattery;
  }
}
