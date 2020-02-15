﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerStatisticsRenderer : MonoBehaviour
{
    ApplicationController applicationController;
    Text text;

    void Awake() {
        text = GetComponent<Text>();
        applicationController = ApplicationController.getSceneInstance();        
    }

    void OnEnable() {
        text.text = applicationController.info;
    }
}
