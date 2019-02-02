using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public PlayerState ps;

    private void OnTriggerEnter( Collider collider )
    {
        // Debug.Log(collision.gameObject.name);

        if (collider.gameObject.name.Equals("HeadCollider"))
        {
            //Debug.Log("boiiiiiii");

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
