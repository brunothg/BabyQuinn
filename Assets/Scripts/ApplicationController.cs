using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class ApplicationController : MonoBehaviour
{

    public PlayerInput controls;
    
    public string cancelScene = "Main_Menu";
    public bool cancelNullSceneQuit = true;

    public LevelObject[] levels = {};

    public long score {
        get {
            long result = 0;

            if (levels != null) {
                foreach (var level in levels) {
                    result += (level != null) ? level.score : 0;
                }
            }

            return result;
        }
    }

    public int playTime {
        get {
            return (PlayerPrefs.HasKey("Application.PlayTime")) ? PlayerPrefs.GetInt("Application.PlayTime") : 0;
        }
        private set {
            PlayerPrefs.SetInt("Application.PlayTime", value);
        }
    }

    public string info {
        get {
            return ""
            + "Score:    " + score + "\n"
            + "Spielzeit: " + TimeSpan.FromSeconds(playTime).ToString(@"hh\:mm\:ss");
        }
    }

    public LevelObject getActualLevel() {
        if (levels == null) {
            return null;
        }

        foreach (var level in levels) {
            if (level.sceneId == SceneManager.GetActiveScene().name) {
                return level;
            }
        }

        return null;
    }

    public void finishedLevel(LevelObject level, PlayerController playerController) {
        var score = playerController.score;
        var elapsedTime = Mathf.RoundToInt(playerController.elapsedTime);
        Debug.Log("Score: " + level.sceneId + ":" + level.levelName + ":" + score);
        Debug.Log("ElapsedTime: " + level.sceneId + ":" + level.levelName + ":" + elapsedTime);
        level.score = score;
        level.time = elapsedTime;
        playTime += elapsedTime;
        
        loadMainMenu();
    }

    void Awake() {
        
    }

    void Update(){

        // PlayerInput abfragen
        if (controls != null) {
            if (controls.isCancel()) {
                if (cancelScene != null && cancelScene.Trim().Length >= 0){
                    SceneManager.LoadScene(cancelScene.Trim());
                } else if (cancelNullSceneQuit) {
                    quit();
                }
            }
        }
       
    }

    public void loadLevel(LevelObject level){
       SceneManager.LoadScene(level.sceneId);
    }

    public void loadMainMenu() {
        SceneManager.LoadScene("Main_Menu");
    }

    public void loadInfoMenu() {
        SceneManager.LoadScene("Info_Menu");
    }

    public void quit(){
        Debug.Log("Beende Anwendung");
        Application.Quit();
    }

    public static ApplicationController getSceneInstance() {
        var controllers = FindObjectsOfType<ApplicationController>();
        if (controllers != null && controllers.Length > 1) {
            Debug.LogWarning("Found more than one ApplicationController in scene");
        }

        var instance = (controllers == null || controllers.Length <= 0) ? null : controllers[0];
        return instance;
    }
}
