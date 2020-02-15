using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
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
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null) {
            Debug.Log("Exit");

            applicationController.finishedLevel(applicationController.getActualLevel(), playerController);
        }

    }
}
