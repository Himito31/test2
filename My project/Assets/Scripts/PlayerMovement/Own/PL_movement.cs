using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PL_movement : MonoBehaviour
{



     public float speed = 50f;
    public float sprintSpeed = 7f;
    public float jumpForce = 5f;
    public float slideForce = 10f;
    public float slideDuration = 1f;
    public float slideCooldown = 2f;
    
    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isSliding = false;
    private float slideTimer = 0f;
    private float slideCooldownTimer = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = CalculateMoveDirection();
        
        if (!isSliding)
        {
            if (Input.GetKey(KeyCode.LeftShift) && verticalInput > 0)
            {
                rb.velocity = moveDirection * sprintSpeed;
            }
            else
            {
                rb.velocity = moveDirection * speed;
            }
        }
        
        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        
        // Sliding
        if (Input.GetKeyDown(KeyCode.LeftControl) && slideCooldownTimer <= 0f && !isSliding && rb.velocity.magnitude > 5f)
        {
            isSliding = true;
            slideTimer = slideDuration;
            rb.AddForce(moveDirection * slideForce, ForceMode.Impulse);
        }
        
        // Sliding cooldown
        if (slideCooldownTimer > 0f)
        {
            slideCooldownTimer -= Time.deltaTime;
        }
        
        // Slide duration
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                isSliding = false;
                slideCooldownTimer = slideCooldown;
            }
        }
    }
    
    Vector3 CalculateMoveDirection()
    {
        // Calculate the move direction based on the camera's forward and right vectors
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = cameraForward * verticalInput + cameraRight * horizontalInput;
        moveDirection.Normalize();
        
        return moveDirection;
    }
    
    void OnCollisionStay(Collision collision)
    {
        // Check if the player is grounded
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
            }
        }
    }
}