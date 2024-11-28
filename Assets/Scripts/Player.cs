using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // For handling input with Unity's Input System

public class Player : MonoBehaviour
{
    // Adjustable movement speed of the player
    [SerializeField] float moveSpeed = 5f;

    // Raw input vector from player input (e.g., keyboard or controller)
    Vector2 rawInput;

    // Padding values to restrict player movement within the screen bounds
    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;

    // Variables to store the calculated screen boundaries
    Vector2 minBounds;
    Vector2 maxBounds;

    // Reference to the Shooter script (if attached)
    Shooter shooter;

    // Reference to the Camera to calculate mouse position in world space
    Camera mainCamera;

    // Reference to the child GameObject (playerBlueSprite) for visual rotation
    [SerializeField] Transform playerSprite; // Drag "playerBlueSprite" here in the Inspector

    void Awake()
    {
        // Get the Shooter component (if available)
        shooter = GetComponent<Shooter>();

        // Assign the main camera
        mainCamera = Camera.main;
    }

    void Start()
    {
        // Initialize the screen boundaries for player movement
        InitBounds();
    }

    void Update()
    {
        // Handle player movement
        Move();

        // Rotate the visual sprite to face the mouse
        RotateTowardsMouse();
    }

    // Initialize the movement boundaries based on the CameraBounds object in the scene
    void InitBounds()
    {
        // Find the GameObject named "CameraBounds" and get its PolygonCollider2D
        PolygonCollider2D cameraBounds = GameObject.Find("CameraBounds").GetComponent<PolygonCollider2D>();

        // Ensure the bounds are calculated only if the collider exists
        if (cameraBounds != null)
        {
            Bounds bounds = cameraBounds.bounds; // Get the world-space bounds of the collider
            minBounds = bounds.min; // Bottom-left corner of the bounds
            maxBounds = bounds.max; // Top-right corner of the bounds
        }
        else
        {
            Debug.LogError("CameraBounds object not found or missing PolygonCollider2D!");
        }
    }

    // Handle player movement based on input
    void Move()
    {
        // Calculate the movement delta based on input, speed, and time
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;

        // Determine the new position, clamped within screen boundaries
        Vector2 newPos = new Vector2();
        newPos.x = Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPos.y = Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);

        // Apply the new position to the player
        transform.position = newPos;
    }

    // Rotate the visual sprite (child object) to face the mouse
    void RotateTowardsMouse()
    {
        // Get the mouse position in screen coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert the mouse position to world space
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0; // Ensure Z is consistent in 2D space

        // Calculate the direction vector from the player to the mouse
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        // Calculate the angle in degrees using Mathf.Atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the visual sprite only
        playerSprite.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Handle movement input from Unity's Input System
    void OnMove(InputValue value)
    {
        // Get the raw input vector (e.g., from WASD or joystick)
        rawInput = value.Get<Vector2>();
    }

    // Handle firing input from Unity's Input System
    void OnFire(InputValue value)
    {
        // Trigger the Shooter's firing logic if the Shooter script exists
        if (shooter != null)
        {
            shooter.isFiring = value.isPressed;
        }
    }
}