using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
  public class BatteryTests
  {
    [Test]
    public void TestBrokenBatteryWontCharge()
    {
      var go = new GameObject("MyGameObject");
      go.AddComponent<Battery>();

      var batt = go.GetComponent<Battery>();
      batt.lifetimeInSeconds = 0;
      batt.currentChargeInSeconds = 10;
      batt.maxChargeInSeconds = 20;

      var result = batt.Consume(1, 5);
      Assert.False(result);
      Assert.AreEqual(batt.currentChargeInSeconds, 10);
    }

    [Test]
    public void TestBatteryDiesWhenItExceedsLifetime()
    {
      var go = new GameObject("MyGameObject");
      go.AddComponent<Battery>();

      var batt = go.GetComponent<Battery>();
      batt.lifetimeInSeconds = 10;
      batt.currentChargeInSeconds = 20;
      batt.maxChargeInSeconds = 30;

      var result = batt.Consume(9, 5);
      Assert.True(result);
      result = batt.Consume(2, 5);
      Assert.False(result);
    }

    [Test]
    public void TestBatteryReturnsAppropriateEnergyStates()
    {
      var go = new GameObject("MyGameObject");
      go.AddComponent<Battery>();

      var batt = go.GetComponent<Battery>();
      batt.currentChargeInSeconds = 75;
      batt.maxChargeInSeconds = 100;
      batt.lifetimeInSeconds = 10000;

      Assert.AreEqual(Battery.EnergyState.GOOD, batt.energyLevel);
      batt.Consume(1, 25);
      Assert.AreEqual(Battery.EnergyState.MEDIUM, batt.energyLevel);
      batt.Consume(1, 25);
      Assert.AreEqual(Battery.EnergyState.BAD, batt.energyLevel);
    }
  }
}
