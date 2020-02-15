using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudRenderer : MonoBehaviour
{

    public Text scoreText;
    public Text timeText;
    public PlayerController playerController;

    ApplicationController applicationController;

    void Awake() {
        applicationController = ApplicationController.getSceneInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     scoreText.text = "Punkte: " + playerController.points;
     timeText.text = "Zeit: " + Mathf.Max(0, Mathf.RoundToInt(applicationController.getActualLevel().referenceTime - playerController.elapsedTime)); 
    }
}
