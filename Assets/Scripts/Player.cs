using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] float moveSpeed = 5f; // Speed of player movement
    Vector2 rawInput; // Stores player input for movement
    Vector2 lastDirection = Vector2.down; // Default direction (facing down)

    [Header("Screen Boundaries")]
    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;
    Vector2 minBounds; // Minimum screen boundaries
    Vector2 maxBounds; // Maximum screen boundaries

    [Header("References")]
    Shooter shooter; // Reference to the Shooter component
    Camera mainCamera; // Main camera in the scene
    [SerializeField] Transform playerSprite; // Player's sprite transform
    [SerializeField] Animator playerAnimator; // Animator for controlling animations

    void Awake()
    {
        shooter = GetComponent<Shooter>();
        mainCamera = Camera.main;
    }

    void Start()
    {
        InitBounds(); // Initialize screen boundaries
    }

    void Update()
    {
        Move(); // Handle movement input
        UpdateMouseDirection(); // Set animations based on mouse direction
        UpdateIdleDirection(); // Ensure idle animations align with the mouse direction
    }

    void InitBounds()
    {
        // Find camera bounds to restrict player movement
        PolygonCollider2D cameraBounds = GameObject.Find("CameraBounds").GetComponent<PolygonCollider2D>();
        if (cameraBounds != null)
        {
            Bounds bounds = cameraBounds.bounds;
            minBounds = bounds.min;
            maxBounds = bounds.max;
        }
        else
        {
            Debug.LogError("CameraBounds object not found or missing PolygonCollider2D!");
        }
    }

    void Move()
    {
        // Calculate new position based on input
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2(
            Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight),
            Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop)
        );
        transform.position = newPos;

        // Update last movement direction if moving
        if (rawInput.magnitude > 0)
        {
            lastDirection = rawInput.normalized;
        }

        // Update Animator parameters
        Vector2 direction = rawInput.magnitude > 0 ? rawInput : lastDirection;
        playerAnimator.SetFloat("Horizontal", direction.x);
        playerAnimator.SetFloat("Vertical", direction.y);
        playerAnimator.SetBool("isMoving", rawInput.magnitude > 0);
    }

    void UpdateMouseDirection()
    {
        // Determine the direction from the player to the mouse position
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0; // Ensure z-coordinate is 0 for 2D calculations

        Vector2 directionToMouse = (mouseWorldPosition - transform.position).normalized;

        // Update Animator parameters based on the mouse direction
        if (Mathf.Abs(directionToMouse.x) > Mathf.Abs(directionToMouse.y))
        {
            // Horizontal aiming
            playerAnimator.SetFloat("Horizontal", directionToMouse.x > 0 ? 1f : -1f);
            playerAnimator.SetFloat("Vertical", 0f);
        }
        else
        {
            // Vertical aiming
            playerAnimator.SetFloat("Horizontal", 0f);
            playerAnimator.SetFloat("Vertical", directionToMouse.y > 0 ? 1f : -1f);
        }
        playerAnimator.SetBool("isAiming", true); // Set aiming state to true
    }

    void UpdateIdleDirection()
    {
        // Update the last direction when idle (not moving)
        if (rawInput.magnitude == 0)
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mouseWorldPosition - transform.position).normalized;

            // Determine if the mouse is more horizontal or vertical relative to the player
            if (Mathf.Abs(directionToMouse.x) > Mathf.Abs(directionToMouse.y))
            {
                lastDirection = directionToMouse.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                lastDirection = directionToMouse.y > 0 ? Vector2.up : Vector2.down;
            }
        }
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>(); // Capture movement input
    }

    void OnFire(InputValue value)
    {
        if (shooter != null)
        {
            shooter.isFiring = value.isPressed; // Enable/disable firing
        }
    }

    public float GetVerticalSpeed()
    {
        return rawInput.y * moveSpeed; // Adjust based on how rawInput and movement logic are handled
    }

}