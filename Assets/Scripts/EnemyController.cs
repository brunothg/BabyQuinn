using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public int maxHitPoints = 1;

    [SerializeField]
    int hitPoints;
    Rigidbody2D p_rigidbody2D;

    void Start()
    {
        hitPoints = maxHitPoints;
        p_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
    }

    void OnProjectileDamage(Projectile projectile)
    {
        hitPoints -= projectile.damage;

        if (hitPoints <= 0) {
            var playerController = projectile.emmitedFrom.GetComponent<PlayerController>();
            if (playerController != null) {
                playerController.addPoints(maxHitPoints);
            }

            Destroy(this.gameObject);
        }
    }
}
