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
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
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
