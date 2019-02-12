using UnityEngine;

public class SpaceshipTravel : MonoBehaviour
{
    private float initialPosition;
    private float endPosition;
    public float timeLimit = 300f; // 5 minutes
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = this.GetComponent<RectTransform>().position.x;
        endPosition = initialPosition;
        endPosition += Mathf.Abs(initialPosition) * 2;
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
        if ((currentTime - startTime) < timeLimit)
        {
            Vector3 currentPosition = this.GetComponent<RectTransform>().position;
            this.GetComponent<RectTransform>().position = new Vector3(initialPosition + (((currentTime - startTime) / timeLimit) * (endPosition - initialPosition)), currentPosition.y, currentPosition.z);
        }
        else if (startTime > 0 && (currentTime - startTime) >= timeLimit)
        {
            // They win the game -- add win conditions here
        }
    }
}
