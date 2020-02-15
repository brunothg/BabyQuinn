using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformVisibility : MonoBehaviour
{
    public bool invert = false;
    public RuntimePlatform[] platforms = new RuntimePlatform[]{};

    void Awake(){
        var platform = Application.platform;
        var isSelectedPlatform = Array.Exists(platforms, ele => ele == platform);
        var visibility = (invert) ? !isSelectedPlatform : isSelectedPlatform;

        Debug.Log("Set visibility of " + gameObject.name + " to " + visibility);
        gameObject.SetActive(visibility);
    }
}
