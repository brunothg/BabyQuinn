using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PointTrigger : MonoBehaviour
{
    public int pointCount = 5;
    public bool removeOnTrigger = true;

    void OnTriggerEnter2D(Collider2D collider) {
        var playerController = collider.gameObject.GetComponent<PlayerController>();
        if (playerController != null) {
            playerController.addPoints(pointCount);

            if (removeOnTrigger) {
                Destroy(this.gameObject);
            }
        }
    }   
}
