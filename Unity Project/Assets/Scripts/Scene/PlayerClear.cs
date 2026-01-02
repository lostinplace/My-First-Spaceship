using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Valve.VR;

public class PlayerClear : MonoBehaviour
{
    // Start is called before the first frame update
  void Start()
  {
    //ClearExcessPlayers();
  }

  public static void ClearExcessPlayers()
  {
    var playerList = GameObject.FindGameObjectsWithTag("Player");
    while (playerList.Length > 1)
    {
            UnityEngine.Debug.Log("Deleted Excess Player");
      GameObject.DestroyImmediate(playerList[1]);
      playerList = GameObject.FindGameObjectsWithTag("Player");
    }
  }


  // Update is called once per frame
  void Update()
    {
    }
}
