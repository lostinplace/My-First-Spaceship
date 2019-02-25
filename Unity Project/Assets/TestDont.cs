using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestDont : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneChanger.GameOver("Test text!!!");
    }
}
