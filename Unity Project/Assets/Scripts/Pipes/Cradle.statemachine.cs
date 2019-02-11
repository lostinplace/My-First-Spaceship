using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Stationary part of the ship that other pipes plug into.*/
public partial class Cradle : Orientable
{
    public Pipe connectedPipe;

    public bool connectPipe(Pipe aPipe)
    {
        if (this.connectedPipe)
            return false;

        this.connectedPipe = aPipe;
        return true;
    }

    public bool ApplyHeat(float heat)
    {
        return this.connectedPipe && this.connectedPipe.ApplyHeat(heat);
    }

    public bool isConnected()
    {
        return this.connectedPipe && connectedPipe.integrityState != Pipe.PipeIntegrityState.BAD;
    }

    public Pipe disconnectPipe(Pipe aPipe)
    {
        if (!this.connectedPipe)
            return null;
        Pipe tmpPipe = this.connectedPipe;
        this.connectedPipe = null;
        return tmpPipe;
    }
}
