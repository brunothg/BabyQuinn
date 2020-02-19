using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    public int maxHitPoints = 1;
    public int speed = 2;
    public bool rightFaced = true;
    public float groundedOffset = 1;
    public float wallOffset = 0.1f;

    [SerializeField]
    int hitPoints;
    public Animator animator;
    Rigidbody2D p_rigidbody2D;
    Vector3 lastPositionWallTest;

    void Start()
    {
        hitPoints = maxHitPoints;
        p_rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    
        lastPositionWallTest = this.transform.position;
    }

    void FixedUpdate()
    {   
        if (getWallTest()) {
            speed *= -1;
        }

        var actualSpeed = speed;
        p_rigidbody2D.velocity = (Vector2.right * actualSpeed) + (Vector2.up * p_rigidbody2D.velocity.y);

        // Calculate Sprite direction & mirroring
        if (rightFaced && getNormalizedDirection(actualSpeed) < 0 || !rightFaced && getNormalizedDirection(actualSpeed) > 0) {
            // Switch the way the player is labelled as facing.
            rightFaced = !rightFaced;
            var actualScale = transform.localScale;
            actualScale.x *= -1;
            transform.localScale = actualScale;
        }

        // Instruct Animator
        if (animator != null)  {
            animator.SetFloat("ActualAbsoluteSpeed", Mathf.Abs(actualSpeed));
        }
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

    
    private bool getWallTest() {
        var actualPosition = transform.position;
        var distance = Vector3.Distance(lastPositionWallTest, actualPosition);
        
        lastPositionWallTest = actualPosition;    
        return distance <= (wallOffset * Time.fixedDeltaTime) && getGroundTest();
    }

    private bool getGroundTest() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, groundedOffset);
        Debug.DrawRay(transform.position, Vector2.down * groundedOffset, Color.red, .2f, false);
        
        foreach (var hit in hits) {
            if (hit.rigidbody != p_rigidbody2D) {
                return true;
            }
        }
        return false;
    }

    private float getNormalizedDirection(float value){
        if (value == 0) {
            return 0;
        }
        if (value > 0) {
            return 1;
        }
        return -1;
    }
}
