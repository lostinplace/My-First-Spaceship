using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialScene : MonoBehaviour
{
    public bool noVR = false;
    // Start is called before the first frame update
    void Start()
    {
      SceneChanger.isNoVR = noVR;
      SceneChanger.GoToMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
