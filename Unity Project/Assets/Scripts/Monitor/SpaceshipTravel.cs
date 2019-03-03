using UnityEngine;

public class SpaceshipTravel : MonoBehaviour
{
  private float totalDistance;
  private float initialPosition;
  private Vector3 initialLocation;
  private RectTransform thisTransform;

  public RectTransform boundingRect;

  void Start()
  {
    thisTransform = this.GetComponent<RectTransform>();
    var iconWidth = thisTransform.rect.width;
    initialPosition = boundingRect.offsetMin.x;
    
    var trackStart = boundingRect.offsetMin.x;
    var trackEnd = boundingRect.offsetMax.x - (iconWidth / 2);
    
    totalDistance = trackEnd - trackStart;

    initialLocation = this.transform.localPosition;
  }

  void Update()
  {
    var completionRatio = SceneChanger.playerState.distanceTraveledInSeconds / SceneChanger.settings.destinationDistanceInSeconds;    
    var newX = completionRatio * totalDistance + initialPosition;

    thisTransform.localPosition = new Vector3(newX, initialLocation.y, initialLocation.z);
  }
}
