using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CradleNetwork : MonoBehaviour
{

  List<Cradle> cradles = new List<Cradle>();
  public bool isConnected()
  {
    return cradles.TrueForAll(x => x.isConnected());
  }

  public bool ApplyHeat(float heat)
  {
    return cradles.TrueForAll(x => x.ApplyHeat(heat));
  }
  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
