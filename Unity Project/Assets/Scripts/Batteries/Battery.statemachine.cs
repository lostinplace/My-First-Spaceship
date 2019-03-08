using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public partial class Battery : Lockable, Handleable.HandleableItem
{
  public enum EnergyState
  {
    GOOD,
    MEDIUM,
    BAD,
    DEAD
  }

  public UnityEvent batteryChargeAudio;

  public float currentChargeInSeconds;
  public float maxChargeInSeconds;
  public float lifetimeInSeconds;
  public bool isDead {
    get => this.lifetimeInSeconds <= 0;
  }

  public float chargeRatio
  {
    get
    {
      return currentChargeInSeconds / maxChargeInSeconds;
    }
  }

  public EnergyState energyLevel
  {
    get
    {
      var tmp = chargeRatio;
      if (isDead) return EnergyState.DEAD;
      switch (tmp)
      {
        case var _ when tmp > 0.65:
          return EnergyState.GOOD;
        case var _ when tmp > 0.35:
          return EnergyState.MEDIUM;
        default:
          return EnergyState.BAD;
      }
    }
  }

  private bool charging
  {
    get => currentPlug && currentPlug.myDevice && currentPlug.myDevice.powerConsumptionPerSecond < 0 && currentPlug.myDevice.isActive;
  }

  private bool StartedCharging { get; set; }

  protected bool AdjustCharge(float adjustment)
  {
    float attemptedCharge = this.currentChargeInSeconds + adjustment;
    float boundedByMin = Math.Max(attemptedCharge, 0);

    this.currentChargeInSeconds = Math.Min(this.maxChargeInSeconds, boundedByMin);
    return this.currentChargeInSeconds > 0;
  }

  public bool Consume(float time, float charge)
  {
    if (this.isDead) return false;
    if (charge > 0) lifetimeInSeconds -= time;
    return AdjustCharge(charge * -1);

  }
}
