using UnityEngine;

public class SpaceshipTravel : MonoBehaviour
{
  public float timeLimit; // 5 minutes
  private float startTime;
  private float totalDistance;
  private float initialPosition;
  public RectTransform boundingRect;

  // Start is called before the first frame update
  void Start()
  {
    var thisTransform = this.GetComponent<RectTransform>();
    var iconWidth = thisTransform.rect.width;
    initialPosition = boundingRect.offsetMin.x;
    
    var trackStart = boundingRect.offsetMin.x;
    var trackEnd = boundingRect.offsetMax.x - (iconWidth / 2);
    startTime = Time.time;
    totalDistance = trackEnd - trackStart;

    var settings = GameObject.FindObjectOfType<SpaceshipSettings>();
    if (!settings) timeLimit = 100f;
    else timeLimit = settings.timeLimitInSeconds;
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
      Vector3 currentPosition = this.GetComponent<RectTransform>().localPosition;
      
      var completionRatio = (currentTime - startTime) / timeLimit;
      var newX = completionRatio * totalDistance + initialPosition;

      this.GetComponent<RectTransform>().localPosition = new Vector3(newX, currentPosition.y, currentPosition.z);
    }
    else if (startTime > 0 && (currentTime - startTime) >= timeLimit)
    {
      // They win the game -- add win conditions here
    }
  }
}
