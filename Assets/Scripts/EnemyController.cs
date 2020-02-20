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
    public float maxLeft;
    public float maxRight;
    public float groundedOffset = 1;
    public float wallOffset = 0.5f;
    

    [SerializeField]
    int hitPoints;
    Animator animator;
    Rigidbody2D p_rigidbody2D;
    Vector3 startPosition;
    Vector3 lastPositionWallTest;
    GameObject healthBar;
    RectTransform healthBarForegroundTransform;
   

    void Start()
    {
        hitPoints = maxHitPoints;
        p_rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        try {
            healthBar = transform.Find("HealthBar").gameObject;
            healthBar.SetActive(maxHitPoints > 1);
            healthBarForegroundTransform = healthBar.transform.Find("Foreground").gameObject.GetComponent<RectTransform>();
        } catch {}

        startPosition = this.transform.position;
        lastPositionWallTest = this.transform.position;
    }

    void FixedUpdate()
    {   
        if (getWallTest()) {
            Debug.Log("Wall detected");
            speed *= -1;
        }

        var actualSpeed = speed;
        if (maxLeft <= 0 && maxRight <=0) {
            actualSpeed = 0;
        } else {
            var direction =  transform.position - startPosition;
            var sqrDistance = direction.sqrMagnitude;
            
    	    if ((direction.x < 0 && sqrDistance >= maxLeft * maxLeft) || (direction.x > 0 && sqrDistance >= maxRight * maxRight)) {
                Debug.Log("Max distance - change direction");
                speed *= -1;
                actualSpeed = speed;
            }
        }
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
                Debug.Log("Killed Enemy - Points: " + maxHitPoints);
                playerController.addPoints(maxHitPoints);
            }

            Destroy(this.gameObject);
            return;
        }

        // Set health bar
        if (healthBarForegroundTransform != null) {

            float difference = 1 - getPercentageHealth();
            healthBarForegroundTransform.anchorMax = new Vector2(1-(difference / 2), 1);
            healthBarForegroundTransform.anchorMin = new Vector2((difference / 2), 0);
            BroadcastMessage("", getPercentageHealth());
        }
    }

    private float getPercentageHealth() {
        return (float)hitPoints / (float)maxHitPoints;
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
