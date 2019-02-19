using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
  public DeviceBase device;
  
  private static float flashTime = 0.5f;
  
  private MeshRenderer alarmRenderer;

  private Color onColor = new Color(1, 0, 0);
  private Color offColor = new Color(0, 0, 0);


  // Start is called before the first frame update
  void Start()
  {
    alarmRenderer = GetComponent<MeshRenderer>();
  }

  // Update is called once per frame
  void Update()
  {

    if (!device.isActive)
    {
      var iteration = Mathf.Floor(Time.fixedTime / flashTime);
      float cycle = iteration % 2;
      alarmRenderer.materials[1].SetColor("_EmissionColor", cycle == 0 ? onColor : offColor);
    } else
    {
      alarmRenderer.materials[1].SetColor("_EmissionColor", offColor);
    }
  }
}
