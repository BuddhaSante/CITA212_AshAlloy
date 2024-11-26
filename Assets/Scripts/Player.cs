using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // Movement speed of the player
    Vector2 rawInput; // Stores raw input values

    // Padding variables to restrict player movement within screen bounds
    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;

    Vector2 minBounds; // Minimum boundaries based on screen size
    Vector2 maxBounds; // Maximum boundaries based on screen size

    Shooter shooter;
    void Awake()
    {
        shooter = GetComponent<Shooter>();
    }

    void Start()
    {
        InitBounds(); // Initialize screen boundaries for player movement
    }

    void Update()
    {
        Move(); // Call movement logic each frame
    }

    // Calculates the screen boundaries using the main camera's viewport
    void InitBounds()
    {
        // Reference the Polygon Collider 2D from CameraBounds
        PolygonCollider2D cameraBounds = GameObject.Find("CameraBounds").GetComponent<PolygonCollider2D>();
        if (cameraBounds != null)
        {
            // Get the world-space bounds of the collider
            Bounds bounds = cameraBounds.bounds;

            // Use the bounds to set the min and max positions
            minBounds = bounds.min;
            maxBounds = bounds.max;
        }
        else
        {
            Debug.LogError("CameraBounds object not found or missing PolygonCollider2D!");
        }
    }


    // Movement logic for the player based on input and screen bounds
    void Move()
    {
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime; // Movement delta
        Vector2 newPos = new Vector2();

        // Clamping the new position within defined boundaries
        newPos.x = Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPos.y = Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);
        
        transform.position = newPos; // Updates player position
    }

    // Called when the player provides input through the Input System
    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>(); // Gets movement input from the player
    }

    void OnFire(InputValue value)
    {
        if(shooter != null)
        {
            shooter.isFiring = value.isPressed;
        }
    }
}
