using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 6;
    public float minSpeed = 2;
    public float acceleration = 9;
    public float jumpForce = 400;
    public bool airControl = false;
    public bool rightFaced = true;
    public float groundedOffset = 1;
    public float ceilingOffset = 0.5f;
    public Animator animator;
    public Collider2D[] headColliders;
    public PlayerInput controls;
    

    [SerializeField]
    float actualSpeed = 0f;

    [SerializeField]
    float actualAccelerationX = 0f;
    
    [SerializeField]
    bool jump = false;
    
    [SerializeField]
    bool duck = false;
    
    [SerializeField]
    bool grounded = true;
    
    Rigidbody2D p_rigidbody2D;
    
    ApplicationController applicationController;
    
    public int points {
        get;
        private set;
    }
    public int score {
        get {
            return points + Mathf.RoundToInt(Mathf.Max(0, applicationController.getActualLevel().referenceTime - elapsedTime));
        }
    }

    public float startTime {
        get;
        private set;
    }

    public float elapsedTime {
        get {
            return Time.time - startTime;
        }
    }

    public void addPoints(int addedPoints) {
        points += Mathf.Abs(addedPoints);
    }

    void Awake() {
        applicationController = ApplicationController.getSceneInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        p_rigidbody2D = GetComponent<Rigidbody2D>();
        controls = (controls!=null) ? controls : gameObject.AddComponent<PlayerInput>();
        startTime = Time.time;
        points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        actualAccelerationX = controls.getHorizontalMovement() * acceleration;
        jump = controls.isJump();
        duck = controls.isDuck();
    }

    // FixedUpdate is called at fixed rate (physics)
    void FixedUpdate()
    {
        grounded = getGroundTest();
        if (getCeilTest()) {
            duck = true;
        }
        
        // Calculate actual speed & acceleration
        actualSpeed = p_rigidbody2D.velocity.x;
        if (Mathf.Abs(actualAccelerationX) > 0.1 && (grounded || airControl)) {
            actualSpeed += actualAccelerationX * Time.fixedDeltaTime;
        }
        actualSpeed = Mathf.Min(Mathf.Max(-maxSpeed, actualSpeed), +maxSpeed);
        actualSpeed = (Mathf.Abs(actualSpeed) < minSpeed) ? minSpeed * getNormalizedDirection(actualAccelerationX) : actualSpeed;
        p_rigidbody2D.velocity = (Vector2.right * actualSpeed) + (Vector2.up * p_rigidbody2D.velocity.y);

        // Calculate Sprite direction & mirroring
        if (rightFaced && getNormalizedDirection(actualSpeed) < 0 || !rightFaced && getNormalizedDirection(actualSpeed) > 0) {
            // Switch the way the player is labelled as facing.
            rightFaced = !rightFaced;
            var actualScale = transform.localScale;
            actualScale.x *= -1;
            transform.localScale = actualScale;
        }

        // Calculate Jump force
        if (jump && grounded && p_rigidbody2D.velocity.y >= 0) {
            grounded = false;
            p_rigidbody2D.AddForce(Vector2.up * jumpForce);
        }

        // Enabel/Disable Duck Colliders
        Array.ForEach(headColliders, (headCollider) => headCollider.enabled = !(grounded && duck));

        // Instruct Animator
        if (animator != null)  {
            animator.SetBool("Grounded", grounded);
            animator.SetBool("Duck", duck);
            animator.SetFloat("ActualAbsoluteSpeed", Mathf.Abs(actualSpeed));
        }
    }

    private bool getCeilTest() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, ceilingOffset);
        
        foreach (var hit in hits) {
            if (hit.rigidbody != p_rigidbody2D) {
                return true;
            }
        }
        return false;
    }

    private bool getGroundTest() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, groundedOffset);
        
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
