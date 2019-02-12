using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    protected List<Icon> icons;
    protected List<StatusBar> statusBars;
    protected Icon spaceShipIcon, battaryIcon, backgroundImage;
    protected SpaceshipTravel spaceShipProgressBar;
    void Start()
    {
        icons = new List< Icon >();
        statusBars = new List< StatusBar >();
        const string CANVAS_NAME_C = "Canvas";
        const string SPACE_SHIP_ICON_NAME_C = "Spaceship";
        const string BATTARY_ICON_NAME_C = "Battary";
        const string BACKGROUND_ICON_NAME_C = "Background";
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
                        statusBars.Add(statusBar);
                    else
                    {
                        //Assuming there is one ship progress bar on the canvas!
                        spaceShipProgressBar = current.GetComponent<SpaceshipTravel>();
                    }
                }
            }
        }
    }
}
