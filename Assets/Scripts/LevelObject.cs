using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "BabyQuinn/Level", order = 1)]
public class LevelObject : ScriptableObject
{
    
    /// <summary>Der Name einer LevelSzene</summary>
    public string sceneId;

    public string levelName;

    /// <summary>Zeit (in sec) zum Beenden des Levels</summary>
    public float referenceTime = 0;

    public long requireScore = 0;

    public int score {
        get {
            return (PlayerPrefs.HasKey("Level." + sceneId + ".Score")) ? PlayerPrefs.GetInt("Level." + sceneId + ".Score") : 0;
        }

        set {
            PlayerPrefs.SetInt("Level." + sceneId + ".Score", Mathf.Max(value, score));
        }
    }

    public int time {
        get {
            return (PlayerPrefs.HasKey("Level." + sceneId + ".Time")) ? PlayerPrefs.GetInt("Level." + sceneId + ".Time") : 0;
        }

        set {
            PlayerPrefs.SetInt("Level." + sceneId + ".Time", Mathf.Min(value, (time <= 0) ? value : time));
        }
    }

    public string info {
        get {
            
            var result = "" 
            + "Score: " + score + "\n"
            + "Zeit:  " + TimeSpan.FromSeconds(time).ToString(@"mm\:ss");

            // Debug.Log("Get Level Info: " + result);
            return result;
        }
    }

}
