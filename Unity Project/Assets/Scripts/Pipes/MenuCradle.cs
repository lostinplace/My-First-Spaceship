﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class MenuCradle : MonoBehaviour
{
    protected Cradle menuCradle;
    public static readonly Color TARGET_COLOR_RO = new Color(0f, 0f, 0f, 1f);
    public UnityEngine.UI.Image fadeOutImage;
    public float fadeSpeed = .001f;
    public float fadeTolerence = .003f;
    public UnityEvent FadeTitleMusic;
    void Start() {
        menuCradle = GetComponent<Cradle>();
    }

  void Update()
  {


    if (menuCradle.connectedPipe == null)
      {
        FadeTitleMusic.Invoke();
        SceneChanger.LoadGame();
      }
  }
}
