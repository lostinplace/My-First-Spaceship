using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CradleNetwork : MonoBehaviour
{
  public Cradle[] CradleList;
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
    this.cradles = CradleList != null ? CradleList.ToList() : new List<Cradle>();
  }

  // Update is called once per frame
  void Update()
  {
  }
}
