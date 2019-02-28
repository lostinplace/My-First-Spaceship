﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public partial class Pipe : Lockable, Handleable.HandleableItem
{
  [SerializeField] public ParticleSystem smokePrefab;
  //Highly suggest this be in local space/parented to this object!//
  [SerializeField] public GameObject smokeSpawnPoint;
  protected ParticleSystem smoke;
  protected bool isBeingHeld = false;
  public Cradle currentCradle, potentialCradle;
  private static PlayerState myPlayerState;

  public bool IsBeingHeld { get => isBeingHeld; }

  public static float MaxHeat = 300;

  public static float HeatLossPerSecond = 3f;

  public UnityEvent pipeBurstAudio;
  private bool hasPlayedBurstAudio = false;

  public void OnPickup()
  {
    Unlock();
    isBeingHeld = true;
    if (currentCradle)
      currentCradle.DetachPipe();
    else
      print("oops");


    this.currentCradle = null;
    if( myPlayerState )
      myPlayerState.PipesHeld++;
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
    if( myPlayerState )
      myPlayerState.PipesHeld--;
  }

  public GameObject GetGameObject() {
    return gameObject;
  }

  void Start()
  {
    Handleable.InitializeHandleableItem(this);
    materialDict = new Dictionary<PipeIntegrityState, Material>()
    {
      { PipeIntegrityState.BAD, Resources.Load<Material>("pipe_bad") },
      { PipeIntegrityState.MEDIUM, Resources.Load<Material>("pipe_medium") },
      { PipeIntegrityState.GOOD, Resources.Load<Material>("pipe_good") },
      { PipeIntegrityState.RUPTURED, Resources.Load<Material>("pipe_ruptured") },
    };
    myRenderer = GetComponent<MeshRenderer>();
    rupturedMesh = Resources.Load<Mesh>("pipe_rupture");
    myPlayerState = GameObject.FindObjectOfType<PlayerState>();
    myRenderer.material.EnableKeyword("_EMISSION");


    var settings = GameObject.FindObjectOfType<SpaceshipSettings>();
    if( settings )
        InitializeWithSettings(settings);

    smoke = Instantiate(smokePrefab, transform);
    smoke.gameObject.SetActive(false);
    smoke.transform.position = smokeSpawnPoint.transform.position;
    smoke.transform.rotation = smokeSpawnPoint.transform.rotation;
  }

  private void InitializeWithSettings(SpaceshipSettings settings) {
    currentIntegrity = currentIntegrity == 0 ? settings.defaultPipeIntegrity : currentIntegrity;

  }

  public static Dictionary<PipeIntegrityState, Material> materialDict;
  private Renderer myRenderer;

  private static Mesh rupturedMesh;

  float updateThreshold = 0.2f;
  float cumulativeDelta = 0;

  void Update()
  {
    if (UnityEngine.Random.value > updateThreshold)
    {
      cumulativeDelta += Time.deltaTime;
      return;
    } 

    ProcessHeat(cumulativeDelta);
    SetAppearance();
    cumulativeDelta = 0;
  }

  void SetAppearance()
  {
    if(this.integrityState == PipeIntegrityState.RUPTURED)
    {
      var filter = this.GetComponent<MeshFilter>();
      filter.mesh = rupturedMesh;

      if (!hasPlayedBurstAudio)
      {
        pipeBurstAudio.Invoke();
        hasPlayedBurstAudio = true;
      }
      if (currentCradle)
        smoke.gameObject.SetActive(true);
      else
        smoke.gameObject.SetActive(false);
    }
    myRenderer.material = materialDict[integrityState];
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
