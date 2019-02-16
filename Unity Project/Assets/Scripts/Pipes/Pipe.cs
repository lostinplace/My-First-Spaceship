using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public partial class Pipe : Lockable, Handleable.HandleableItem
{
  protected bool isBeingHeld = false;
  public Cradle currentCradle, potentialCradle;

  public bool IsBeingHeld { get => isBeingHeld; }

  public void OnPickup()
  {
    Unlock();
    isBeingHeld = true;
    if (currentCradle) currentCradle.DetachPipe();

    this.currentCradle = null;
  }

  public void OnDrop()
  {
    isBeingHeld = false;
    if (potentialCradle != null)
    {
      potentialCradle.ProcessCollision(this);
    }
  }

  public GameObject GetGameObject() {
    return gameObject;
  }

  void Start() {
    this.currentIntegrity = 1500;
    Handleable.InitializeHandleableItem(this);
    materialDict = new Dictionary<PipeIntegrityState, Material>()
    {
      { PipeIntegrityState.BAD, Resources.Load<Material>("pipe_bad") },
      { PipeIntegrityState.MEDIUM, Resources.Load<Material>("pipe_medium") },
      { PipeIntegrityState.GOOD, Resources.Load<Material>("pipe_good")},
    };
  }

  public static Dictionary<PipeIntegrityState, Material> materialDict;

  void Update()
  {
    
    var renderer = GetComponent<MeshRenderer>();
    try
    {
      renderer.material = materialDict[this.integrityState];
    } catch(Exception ex)
    {
      Debug.Log("weird");
      Debug.LogException(ex);
    }
    
    var heatEmission = CalculateHeatEmission();
    renderer.material.SetColor("_EmissionColor", new Color(heatEmission, 0, 0));
    
    Debug.Log($"pipe heat: {this.currentHeat}");
    Debug.Log($"pipe heatemission: {heatEmission}");
    Debug.Log($"pipe integrity: {this.currentIntegrity}");
  }

  float CalculateHeatEmission()
  {
    
    float result = (this.currentHeat+1) / 100;
    float limited = Math.Min(result, 1);
    return limited * 255.0f;
    
  }

  
}
