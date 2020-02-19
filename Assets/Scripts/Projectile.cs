using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public float speed = 1;
    public float maxDistance = 3;
    public bool directionRight = true;
    public GameObject emmitedFrom;

    private Vector3 startPosition;
    private Rigidbody2D p_rigidbody2D;

    void Start(){
        p_rigidbody2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    // FixedUpdate is called at fixed rate (physics)
    void FixedUpdate()
    {
        p_rigidbody2D.velocity = new Vector3(speed * ((directionRight) ? +1 : -1), p_rigidbody2D.velocity.y);

        if (transform.position.x >= startPosition.x + maxDistance) {
            Destroy(this.gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != emmitedFrom) {
            collision.BroadcastMessage("OnProjectileDamage", this, SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
    }
}
