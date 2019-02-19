﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public partial class Pipe : Lockable, Handleable.HandleableItem
{
  protected bool isBeingHeld = false;
  public Cradle currentCradle, potentialCradle;

  public bool IsBeingHeld { get => isBeingHeld; }
  public static float MaxHeat = 300;

  public static float CoolingRate = 0.8f;

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
    if (currentCradle)
    {
      currentCradle.DetachPipe();
    }
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
      { PipeIntegrityState.RUPTURED, Resources.Load<Material>("pipe_ruptured")},
    };
    myRenderer = GetComponent<MeshRenderer>();
    rupturedMesh = Resources.Load<Mesh>("pipe_rupture");
  }

  public static Dictionary<PipeIntegrityState, Material> materialDict;
  private Renderer myRenderer;

  private static Mesh rupturedMesh;

  void Update()
  {
    var delta = Time.deltaTime;
    ProcessHeat(delta);
    SetAppearance();
  }

  void SetAppearance()
  {
    if(this.integrityState == PipeIntegrityState.RUPTURED)
    {
      var filter = this.GetComponent<MeshFilter>();
      filter.mesh = rupturedMesh;
    }
    myRenderer = GetComponent<MeshRenderer>();
    myRenderer.material = materialDict[this.integrityState];
    var heatEmission = CalculateHeatEmission();
    myRenderer.material.SetColor("_EmissionColor", new Color(heatEmission, 0, 0));
  }

  float CalculateHeatEmission()
  {
    float result = (this.currentHeat+1) / MaxHeat;
    float limited = Math.Min(result, 1);

    return limited;
  }


}
