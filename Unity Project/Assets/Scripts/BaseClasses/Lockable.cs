using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockable :MonoBehaviour
{
    protected bool isLocked = false;
    public bool IsLocked { get => isLocked; }

    public void Lock() {
        isLocked = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public void Unlock() {
        isLocked = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
