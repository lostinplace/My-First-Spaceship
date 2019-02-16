using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public PlayerState ps;

    private void OnTriggerEnter( Collider collider )
    {
        if (collider.gameObject.name.Equals("HeadCollider"))
        {
            if (ps)
            {
                Valve.VR.InteractionSystem.Interactable interaction = collider.gameObject.GetComponent<Valve.VR.InteractionSystem.Interactable>();
                if (interaction != null)
                {
                    if (interaction.attachedToHand != null)
                        interaction.attachedToHand.DetachObject(gameObject, false);
                }
                transform.parent = null;
                ps.ReceiveFood();
                Destroy(gameObject);
            }

        }
    }
}
