using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;

    private Rigidbody rb;
    private Transform playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>().transform;
        rb.freezeRotation = true; // Freeze rotation to prevent unwanted physics interactions
    }

    void Update()
    {
        // Basic movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveHorizontal + transform.forward * moveVertical;
        rb.velocity = moveDirection.normalized * speed;

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.Rotate(Vector3.left * mouseY);
    }
}