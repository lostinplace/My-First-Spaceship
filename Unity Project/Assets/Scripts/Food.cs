using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour
{
    public PlayerState playerState => SceneChanger.playerState;

    public UnityEvent foodEatAudio;

    private void OnTriggerEnter( Collider collider )
    {
        if (collider.gameObject.name.Equals("HeadCollider"))
        {
            if (playerState)
            {
                Valve.VR.InteractionSystem.Interactable interaction = collider.gameObject.GetComponent<Valve.VR.InteractionSystem.Interactable>();
                if (interaction != null)
                {
                    if (interaction.attachedToHand != null)
                        interaction.attachedToHand.DetachObject(gameObject, false);
                }
                transform.parent = null;
                playerState.ReceiveFood();
                Destroy(gameObject);
                foodEatAudio.Invoke();
            }

        }
    }
}
