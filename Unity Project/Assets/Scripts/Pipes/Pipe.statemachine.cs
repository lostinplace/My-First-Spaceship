﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public partial class Pipe : Lockable, Handleable.HandleableItem
{
  public enum PipeIntegrityState
  {
    GOOD = 1000,
    MEDIUM = 600,
    BAD = 300,
    RUPTURED = 0
  }

  public float currentIntegrity;

  public float currentHeat = 0;
  public static Int32 randomSeed = 7357;

  /// <summary>
  /// this is the exponent that is applied to the current heat level to determine damage.  if it gets bigger than 1 things will break very quickly
  /// </summary>
  public static double damageScalingBase = 0.6;

  static System.Random randGen = new System.Random(randomSeed);

  public PipeIntegrityState integrityState
  {
    get
    {
      switch (currentIntegrity)
      {
        case var _ when currentIntegrity > (float)PipeIntegrityState.GOOD:
          return PipeIntegrityState.GOOD;
        case var _ when currentIntegrity > (float)PipeIntegrityState.MEDIUM:
          return PipeIntegrityState.MEDIUM;
        case var _ when currentIntegrity > (float)PipeIntegrityState.BAD:
          return PipeIntegrityState.BAD;
        default:
          return PipeIntegrityState.RUPTURED;
      }
    }
  }

  public bool isBroken
  {
    get
    {
      return this.integrityState == PipeIntegrityState.RUPTURED;
    }
  }
  public static double GaussianRandom(double mean, double stdDev)
  {
    double u1 = 1.0 - randGen.NextDouble(); 
    double u2 = 1.0 - randGen.NextDouble();
    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); 
    double randNormal = mean + stdDev * randStdNormal; 
    return randNormal;
  }

  public static double GetDamageForHeat(float heatValue)
  {
    double stdDev = Math.Pow(heatValue, damageScalingBase);
    double damageValue = GaussianRandom(0, stdDev);
    return Math.Abs(damageValue);
  }

  public bool ApplyHeat(float heatValue)
  {
    this.currentHeat += heatValue;
    if (heatValue > 0)
    {
      this.ApplyDamage(Time.deltaTime);
    }
    return this.currentIntegrity > 0;
  }

  public void ProcessHeat(float timeDelta)
  {
    var heatToBeApplied = CoolingRate * timeDelta;
    this.currentHeat -= Math.Min(heatToBeApplied, this.currentHeat);
  }

  public void ApplyDamage(float timeDelta)
  {
    double damage = GetDamageForHeat(this.currentHeat) * timeDelta;
    this.currentIntegrity -= (float)damage;
    
  }
}

