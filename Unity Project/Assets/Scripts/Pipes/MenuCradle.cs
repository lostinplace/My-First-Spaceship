using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCradle : MonoBehaviour
{
    protected Cradle menuCradle;
    public static readonly Color TARGET_COLOR_RO = new Color(0f, 0f, 0f, 1f);
    public UnityEngine.UI.Image fadeOutImage;
    public float fadeSpeed = .001f;
    public float fadeTolerence = .003f;
    void Start() {
        menuCradle = GetComponent<Cradle>();
        fadeOutImage.color = new Color(0f, 0f, 0f, 0f);
    }

    void Update()
    {
        if (menuCradle.connectedPipe == null)
        {
            if ((TARGET_COLOR_RO.a - fadeOutImage.color.a) > fadeTolerence)
                fadeOutImage.color = new Color(0f, 0f, 0f, fadeOutImage.color.a + (fadeSpeed * Time.deltaTime));
            else
                SceneChanger.LoadGame();
        }
    }
}
