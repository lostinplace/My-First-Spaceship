using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeviceBase : MonoBehaviour
{
  public event EventHandler ItemProduced;
  public event EventHandler DeviceDeactivated;
  public event EventHandler DeviceActivated;

  public float powerConsumptionPerSecond = 0.01f;
  public float heatOutputPerSecond = 0.01f;

  public float timeActiveInSeconds = 0;
  

  public float productionTime = 0;
  public float productionTimeRequired=5.0f;
  public bool hasItem = false;

  public Plug plug;

  public Battery currentBattery
  {
    get
    {
      return this.plug.currentBattery;
    }

    set
    {
      this.plug.currentBattery = value;
    }
  }

  public CradleNetwork cradleNetwork;

  public bool pipesNotRequired = true;

  public bool BatteriesRequired = false;

  private bool batteryNotRequired;

  public bool isActive {
    get
    {
      return this.CanCycle();
    }
  }

  public void ResetActivation()
  {
    throw new NotImplementedException();
  }

  // Start is called before the first frame update
  void Start()
  {
    plug.myDevice = this;
    if(!cradleNetwork) cradleNetwork = new CradleNetwork();
    batteryNotRequired = !BatteriesRequired || powerConsumptionPerSecond < 0;
  }

  public bool DoCycle(float delta)
  {
    if (!this.isActive) return false;

    bool powered = false;

    if (BatteriesRequired && this.powerConsumptionPerSecond > 0)
    {
      powered = this.currentBattery && this.currentBattery.Consume(delta, powerConsumptionPerSecond * delta);
    }

    if (this.powerConsumptionPerSecond < 0)
    {
      powered = true;
      currentBattery.Consume(delta, powerConsumptionPerSecond * delta);
    }

    if (!BatteriesRequired) powered = true;

    var piped = pipesNotRequired || (cradleNetwork.isConnected() && cradleNetwork.ApplyHeat(heatOutputPerSecond * delta));

    return powered && piped;
  }

  public void DeactivateDevice()
  {
    if (this.DeviceActivated == null) return;
    this.DeviceDeactivated(this, null);
  }

  public void ProduceItem()
  {
    if (ItemProduced == null) return;
    ItemProduced(this, null);
  }

  public void AttachBattery(Battery aBattery)
  {
    this.currentBattery = aBattery;
  }

  public void RemoveBattery()
  {
    this.currentBattery = null;
    if(this.BatteriesRequired)
      DeactivateDevice();
  }

  public bool CanCycle()
  {
    var powered = !BatteriesRequired || (currentBattery && !currentBattery.isDead && currentBattery.currentChargeInSeconds > 0);
    var piped = pipesNotRequired || cradleNetwork.isConnected();
    return powered && piped;
  }

  // Update is called once per frame
  void Update()
  {
    var delta = Time.deltaTime;
    if (this.CanCycle() && this.DoCycle(delta))
    {
      if (!this.isActive)
      {
        DeactivateDevice();
      }
      timeActiveInSeconds += delta;
    }
    else
    {
      if (this.isActive)
      {
        DeactivateDevice();
      }
    }

    if (!hasItem && this.isActive)
    {
      this.productionTime += delta;
      if (this.productionTime >= productionTimeRequired)
      {
        ProduceItem();
        this.hasItem = true;
        this.productionTime = 0;
      }
    }
    else
    {
      this.productionTime = 0;
    }
  }
}
