using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShip : MonoBehaviour
{
    [SerializeField] public Vector3[] destinations;
    [SerializeField] public float rocketSpeed = 1f;
    [SerializeField] public float rocketDestinationTolerence;
    protected Vector3 currentDestination;
    protected int currentDestinationIndex = 0;

    void Start()
    {
        if (destinations.Length == 1)
            transform.LookAt(destinations[0]);
        else {
            transform.position = destinations[1];
            transform.LookAt(destinations[0]);
            currentDestination = destinations[0];
        }
        //rocketDestinationTolerence = rocketSpeed;
    }

    public static bool IsCloseEnoughToDestination( Vector3 position, Vector3 destination, float tolerence ) {
        //        Debug.Log((destination - position).magnitude);
        return (position - destination).magnitude < tolerence;
        /*Debug.Log(((int)destination.x == (int)position.x) + " " + position.x + " " + destination.x);
        return ((int)destination.x == (int)position.x &&
                (int)destination.y == (int)position.y &&
                (int)destination.z == (int)position.z);*/
    }

    void Update()
    {
        if (IsCloseEnoughToDestination(transform.position, currentDestination, rocketDestinationTolerence) != true)
            transform.position += transform.forward * Time.deltaTime * rocketSpeed;//Vector3.Lerp(transform.position, currentDestination, rocketSpeed * Time.deltaTime);
        else
        {
            if (++currentDestinationIndex < destinations.Length)
                currentDestination = destinations[currentDestinationIndex];
            else
            {
                currentDestinationIndex = 0;
                currentDestination = destinations[currentDestinationIndex];
            }
            transform.LookAt(currentDestination);
        }
    }
}
