using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipTravel : MonoBehaviour
{
    private float initialPosition;
    private float endPosition;
    public float timeLimit = 300f; // 5 minutes
    public float travelDistance = 144;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = this.GetComponent<RectTransform>().position.x;
        endPosition = initialPosition;
        startTime = Time.time;
    }

    //void BeginGame()
    //{
        //startTime = Time.deltaTime;
    //}

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        Debug.Log(initialPosition);
        if ((currentTime - startTime) < timeLimit)
        {
            Vector3 currentPosition = this.GetComponent<RectTransform>().position;
            this.GetComponent<RectTransform>().position = new Vector3(initialPosition + (((currentTime - startTime) / timeLimit) * travelDistance), currentPosition.y, currentPosition.z);
        } else if (startTime > 0 && (currentTime - startTime) >= timeLimit)
        {
            // They win the game -- add win conditions here
        }
    }
}
