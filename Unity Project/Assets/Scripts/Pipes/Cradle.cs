using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Stationary part of the ship that other pipes plug into.*/
public class Cradle : Orientable
{
    public bool isOccupied = false;
    private void OnTriggerEnter(Collider other) {
        Trigger(other);
    }

    public void Trigger( Collider other )
    {
        Pipe[] pipes = other.gameObject.GetComponents<Pipe>();
        Pipe pipe = null;
        if (pipes.Length > 0 && isOccupied == false )
        {
            pipe = pipes[0];
            if (pipe.IsBeingHeld != true)
            {
                isOccupied = true;
                pipe.currentCradle = this;
                pipe.Lock();
                pipe.transform.position = transform.TransformPoint(origin);
                pipe.transform.Translate(pipe.transform.TransformVector(pipe.origin));
                pipe.transform.rotation = transform.rotation * direction * pipe.direction;
            }
            else
                pipe.potentailCradle = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Pipe[] pipes = other.gameObject.GetComponents<Pipe>();
        if (pipes.Length > 0 )
            pipes[0].potentailCradle = null;
    }
  public Pipe connectedPipe;

  public bool connectPipe(Pipe aPipe)
  {
    if (this.connectedPipe != null)
      return false;

    this.connectedPipe = aPipe;
    return true;
  }

  public bool ApplyHeat (float heat){
    return this.connectedPipe && this.connectedPipe.ApplyHeat(heat);
  }

  public bool isConnected()
  {
    return this.connectedPipe && connectedPipe.integrityState != Pipe.PipeIntegrityState.BAD;
  }

  public Pipe disconnectPipe(Pipe aPipe)
  {
    if (this.connectedPipe == null)
      return null;
    Pipe tmpPipe = this.connectedPipe;
    this.connectedPipe = null;
    return tmpPipe;
  }
}
