using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCollider : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        var playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        } else {
            Destroy(collision.gameObject);
        }
    }
}
