using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacePiolet : MonoBehaviour
{
    public Object prefab;
    public GameObject replacement;
    void Start() {
        UnityEditor.PrefabUtility.ReplacePrefab(replacement, prefab);
    }
}
