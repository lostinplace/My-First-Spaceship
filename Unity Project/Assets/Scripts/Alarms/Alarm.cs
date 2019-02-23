using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
  public DeviceBase device;
  
  private static float flashTime = 0.5f;
  
  private MeshRenderer alarmRenderer;

  // Start is called before the first frame update
  void Start()
  {
    alarmRenderer = GetComponent<MeshRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    var strobeMaterial = alarmRenderer.materials[1];
    if (!device.isActive)
    {
      var iteration = Mathf.Floor(Time.fixedTime / flashTime);
      float cycle = iteration % 2;
      
      if (cycle == 0)
      {
        strobeMaterial.EnableKeyword("_EMISSION");
      }
      else
      {
        strobeMaterial.DisableKeyword("_EMISSION");
      }
      
    } else
    {
      strobeMaterial.DisableKeyword("_EMISSION");
    }
  }
}
