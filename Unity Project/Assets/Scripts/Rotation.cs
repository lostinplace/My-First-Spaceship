using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for rotating one GameObject around another.
/// </summary>
public class Rotation : MonoBehaviour {
    [SerializeField]
    private float rotationSpeed;

    void Update() {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
