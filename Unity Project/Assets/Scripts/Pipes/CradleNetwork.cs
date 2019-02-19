using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CradleNetwork : MonoBehaviour
{
  public List<Cradle> cradles;

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
    if (cradles == null) cradles = new List<Cradle>();
  }

  // Update is called once per frame
  void Update()
  {
  }
}
