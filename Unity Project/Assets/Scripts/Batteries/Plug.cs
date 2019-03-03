﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
  public Battery currentBattery;
  public DeviceBase myDevice;
  public bool StartsWithBattery = true;
  private static PlayerState playerState => SceneChanger.playerState;
  private MeshRenderer placeholderRenderer;

  private void Start()
  {
    if (StartsWithBattery)
    {
      var prefab = Resources.Load("RuntimeBattery");
      var obj = (GameObject)Instantiate(prefab);
      var myBattery = obj.GetComponent<Battery>();
      AttachBattery(myBattery);
    }
    
    placeholderRenderer = transform.Find("Placeholder").GetComponent<MeshRenderer>();
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
