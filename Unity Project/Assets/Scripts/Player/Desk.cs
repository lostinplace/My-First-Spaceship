using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour, Handleable.HandleableItem
{
    // Start is called before the first frame update
  void Start()
  {
    var settings = GameObject.FindObjectOfType<Camera>();
  }

  // Update is called once per frame
  void Update()
  {
        
  }

  public void OnPickup()
  {
    
  }

  public void OnDrop()
  {
    
  }

  public GameObject GetGameObject()
  {
    return this.gameObject;
  }

}
