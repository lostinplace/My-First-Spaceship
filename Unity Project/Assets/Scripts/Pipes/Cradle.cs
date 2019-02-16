﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Stationary part of the ship that other pipes plug into.*/
public partial class Cradle : MonoBehaviour
{

  private void OnTriggerEnter(Collider other) {
    Pipe tmpPipe = other.gameObject.GetComponent<Pipe>();
    ProcessCollision(tmpPipe);
  }

  public bool startWithPipe = true;

  void Start()
  {

    if (startWithPipe)
    {
      var prefab = Resources.Load("RuntimePipe");
      var obj = (GameObject)Instantiate(prefab);
      var myPipe = obj.GetComponent<Pipe>();
      AttachPipe(myPipe);
    }
  }

  public void ProcessCollision( Pipe tmpPipe )
  {

    if (!tmpPipe) return;
    if (!tmpPipe.IsBeingHeld && !connectedPipe)
    {
      AttachPipe(tmpPipe);
    }
    else
    {
      tmpPipe.potentialCradle = this;
    }
  }

  public void AttachPipe(Pipe aPipe)
  {
    if(!connectPipe(aPipe)) return;

    aPipe.currentCradle = this;
    aPipe.Lock();

    var pipeAreaTransform = this.gameObject.GetComponentInChildren<Transform>().Find("PipeArea");
    var capsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();

    aPipe.transform.SetPositionAndRotation(pipeAreaTransform.position, pipeAreaTransform.rotation);
  }

  public void DetachPipe()
  {
    connectedPipe = null;
  }

  private void OnTriggerExit(Collider other)
  {
    Pipe tmpPipe = other.gameObject.GetComponent<Pipe>();
    if (!tmpPipe) return;
    tmpPipe.potentialCradle = null;
  }

}
