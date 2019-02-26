using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Monitor : MonoBehaviour
{
  public PlayerState player;
  protected List<Icon> icons;
  protected List<StatusBar> statusBars;
  protected Icon spaceShipIcon, battaryIcon, backgroundImage;
  protected SpaceshipTravel spaceShipProgressBar;

  private DeviceBase myDevice;

  //Internal useful references, so //
  protected StatusBar hungerBar, engineBar, airBar;
  
  void Start()
  {
    myDevice = this.GetComponent<DeviceBase>();
    icons = new List< Icon >();
    statusBars = new List< StatusBar >();
    const string CANVAS_NAME_C = "Canvas";
    const string SPACE_SHIP_ICON_NAME_C = "Spaceship";
    const string BATTARY_ICON_NAME_C = "Battary";
    const string BACKGROUND_ICON_NAME_C = "Background";
    const string ENGINE_STATE_NAME_C = "Engine";
    const string AIR_STATE_NAME_C = "Air";
    const string FOOD_STATE_NAME_C = "Food";
    GameObject canvas = null;
    for (int i = 0; i < transform.childCount; ++i )
    {
      //So GetChild doesent run twice for some reason.//
      GameObject currentChild = transform.GetChild( i ).gameObject;
      if ( currentChild.name.CompareTo( CANVAS_NAME_C ) == 0 ) {
        canvas = currentChild;
        break;
      }
    }
    if( canvas != null )
    {
      Regex iconMatcher = new Regex( "([!-z])+(Icon)" );
      for( int i = 0; i < canvas.transform.childCount; ++i )
      {
        GameObject current = canvas.transform.GetChild( i ).gameObject;
        if (iconMatcher.IsMatch(current.name) == true)
          icons.Add(new Icon(current));
        else if (current.name.CompareTo(SPACE_SHIP_ICON_NAME_C) == 0)
          spaceShipIcon = new Icon(current);
        else if (current.name.CompareTo(BATTARY_ICON_NAME_C) == 0)
          battaryIcon = new Icon(current);
        else if (current.name.CompareTo(BACKGROUND_ICON_NAME_C) == 0)
          backgroundImage = new Icon(current);
        else
        {
          //I imagine GetComponent uses reflection and I dont want it to run twice.//
          StatusBar statusBar = current.GetComponent<StatusBar>();
          if (statusBar != null)
          {
            statusBars.Add(statusBar);
            if (statusBar.stateName.CompareTo(ENGINE_STATE_NAME_C) == 0)
              engineBar = statusBar;
            else if (statusBar.stateName.CompareTo(AIR_STATE_NAME_C) == 0)
              airBar = statusBar;
            else if (statusBar.stateName.CompareTo(FOOD_STATE_NAME_C) == 0)
              hungerBar = statusBar;
          }
          else
          {
            //Assuming there is one ship progress bar on the canvas!
            spaceShipProgressBar = current.GetComponent<SpaceshipTravel>();
          }
        }
      }
    }
  }

  void Update() {
    UpdateBars();
    
    if (myDevice)
    {
      this.GetComponentInChildren<MeshRenderer>().enabled = myDevice.isActive;
    }
  }

  void UpdateBars()
  {
    DisplayHungerStatus();
    DisplayAirStatus();
    DisplayEngineStatus();
  }
  /*The reason this needs to be called every frame is because
   Time.deltaTime changes, @TODO: (Chris G.) add a rate for change for 
   every stat in PlayerState ? Have an event that handles this in PlayerState ?*/
  protected void DisplayHungerStatus() {
    hungerBar.StartChange(StatusBarState.DECREASING, -1f / player.unburntHungerMax);
  }
  //TODO: Untested
  protected void DisplayAirStatus()
  {
    if (player.IsAirOn)
      airBar.StartChange(StatusBarState.INCREASING, player.AirRefillRate);
    else
      airBar.StartChange(StatusBarState.DECREASING, (-1f / player.MaxAirlessTime));
  }
  //TODO: Untested
  protected void DisplayEngineStatus()
  {
    if (player.Engine)
    {
      if (player.Engine.isActive == true)
        engineBar.StartChange(StatusBarState.STOPPED, 0f);
      else
        engineBar.StartChange(StatusBarState.DECREASING, (-1f / player.MaxEngineOffTime));
    }
  }
}
