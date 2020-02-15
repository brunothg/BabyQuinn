using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class LevelSelectionRenderer : MonoBehaviour
{

  
  public GameObject levelSelectionPrefab;
  public float spacing = 20;
  public string levelNameObjectName = "Level_Name";
  public string levelInfoObjectName = "Level_Info";

  ApplicationController applicationController;
  RectTransform rectTransform;
  Vector3 levelEntryStartPosition;

  void Awake() {
    rectTransform = GetComponent<RectTransform>();
    levelEntryStartPosition = levelSelectionPrefab.GetComponent<RectTransform>().localPosition;
    applicationController = ApplicationController.getSceneInstance();
  }

  void OnEnable() {
      Debug.Log("Generate Level Selection UI");
      clearLevelSelections();

      var levels = applicationController.levels;
      float gesHeight = 0;

      foreach (var level in levels) {

        var newLevelSelection = Instantiate(levelSelectionPrefab);
        newLevelSelection.name = "LevelSelection_" + level.sceneId + "_" + level.levelName;
        newLevelSelection.transform.SetParent(this.transform);

        var newLevelSelectionRectTransform = newLevelSelection.GetComponent<RectTransform>();
        newLevelSelectionRectTransform.localPosition = new Vector3(levelEntryStartPosition.x,  -gesHeight - spacing, levelEntryStartPosition.z);

        var newLevelSelectionButton = newLevelSelection.GetComponent<Button>();
        newLevelSelectionButton.onClick.AddListener(()=> {
          if (level.requireScore <= 0 || level.requireScore <= applicationController.score) {
            Debug.Log("Levelselection clicked " + level.sceneId + ":" + level.levelName);
            applicationController.loadLevel(level);
          } else {
            Debug.Log("Levelselection clicked, but player score < reqired level score");
          }
        });

        var levelNameObject = newLevelSelection.transform.Find(levelNameObjectName);
        if (levelNameObject != null && levelNameObject.GetComponent<Text>() != null) {
          levelNameObject.GetComponent<Text>().text = level.levelName;
        }

        var levelInfoObject = newLevelSelection.transform.Find(levelInfoObjectName);
        if (levelInfoObject != null && levelInfoObject.GetComponent<Text>() != null) {
          levelInfoObject.GetComponent<Text>().text = level.info;
        }


        gesHeight += newLevelSelectionRectTransform.sizeDelta.y + spacing;
      }
      gesHeight += spacing;
      
      rectTransform.sizeDelta = new Vector2(0, gesHeight);
  }

  private void clearLevelSelections() {
    for (var i = this.transform.childCount - 1; i >= 0; i--)
    {
        var child = this.transform.GetChild(i);
        child.transform.SetParent(null);
    } 
  }
}
