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


    public EnergyItemBase battery;
    public CradleNetwork cradleNetwork;

    public bool pipesNotRequired = true;

    public bool batteryNotRequired = true;

    public bool isActive = true;

    public void ResetActivation()
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        cradleNetwork = new CradleNetwork();
    }

    public bool DoCycle(float delta)
    {
        if (!this.isActive) return false;
        var powered = !batteryNotRequired && this.battery && this.battery.Consume(delta, powerConsumptionPerSecond * delta);
        var piped = !pipesNotRequired && cradleNetwork.isConnected() && cradleNetwork.ApplyHeat(heatOutputPerSecond * delta);
        return powered && piped;
    } 

    public void AttachBattery(EnergyItemBase aBattery)
    {
        this.battery = aBattery;
        if (this.CanCycle())
        {
            this.isActive = true;
            this.DeviceActivated(this, null);
        }
    }

    public void RemoveBattery()
    {
        this.battery = null;
        this.isActive = false;
        this.DeviceDeactivated(this, null);
    }

    public bool CanCycle()
    {
        var powered = this.batteryNotRequired || (battery && !battery.isDead && battery.currentChargeInSeconds > 0);
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
                this.DeviceActivated(this, null);
            }
            timeActiveInSeconds += delta;
        }
        else
        {
            if (this.isActive)
            {
                this.DeviceDeactivated(this, null);
            }
        }

        if (!hasItem && this.isActive)
        {
            this.productionTime += delta;
            if (this.productionTime >= productionTimeRequired)
            {
                ItemProduced(this, null);
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
