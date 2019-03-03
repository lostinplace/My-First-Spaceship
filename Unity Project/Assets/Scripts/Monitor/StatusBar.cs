using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum StatusBarState {
    ENGINE, AIR, FOOD
}

public class StatusBar : MonoBehaviour
{
  private RectTransform thisTransform;
  public Func<float> scalingFunc
  {
    get;
    set;
  }

  public StatusBarState AssociatedState;
  
    // Start is called before the first frame update
  void Start()
  {
    thisTransform = this.GetComponent<RectTransform>();
  }

  // Update is called once per frame
  void Update()
  {
    var xScale = scalingFunc();
    var clamped = Mathf.Clamp(xScale, 0, 1);
    thisTransform.localScale = new Vector3(clamped, 1, 1);

  }
}
