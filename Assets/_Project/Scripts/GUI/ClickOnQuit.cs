﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnQuit : MonoBehaviour {
    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();

    }
}