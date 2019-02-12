using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Helper class//
public class Icon
{
    protected GameObject parent;
    protected Image image;
    protected RectTransform rect;
    public GameObject Parent { get => parent; set => parent = value; }
    public Image Image { get => image; set => image = value; }
    public RectTransform Rect { get => rect; set => rect = value; }

    public Icon( GameObject icon ) {
        SetIconFromGameObject(icon);
    }

    public void SetIconFromGameObject( GameObject icon )
    {
        image = GetIfComponentExists<Image>(icon);
        rect = GetIfComponentExists<RectTransform>(icon);
        parent = icon.transform.parent.gameObject;
    }

    protected static COMPONENT_T GetIfComponentExists<COMPONENT_T>( GameObject from ) {
        COMPONENT_T[] components = from.GetComponents<COMPONENT_T>();
        if (components.Length > 0)
            return components[0];
        return default( COMPONENT_T );
    }
    //TODO: Add change image method.//
}
