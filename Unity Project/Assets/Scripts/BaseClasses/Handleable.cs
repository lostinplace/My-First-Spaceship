using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handleable
{
    public static void InitializeHandleableItem( HandleableItem toHandle )
    {
        Valve.VR.InteractionSystem.Throwable throwable = 
                toHandle.GetGameObject().GetComponent<Valve.VR.InteractionSystem.Throwable>();
        if (throwable != null)
        {
            throwable.onPickUp.AddListener(toHandle.OnPickup);
            throwable.onDetachFromHand.AddListener(toHandle.OnDrop);
        }
    }
    public interface HandleableItem
    {
        void OnPickup();
        void OnDrop();
        GameObject GetGameObject();
    }
}
