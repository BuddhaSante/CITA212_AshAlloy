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
    Vector2 minBounds;
    Vector2 maxBounds;

    [Header("References")]
    Shooter shooter;
    Camera mainCamera;
    [SerializeField] Transform playerSprite;
    [SerializeField] Animator playerAnimator;

    void Awake()
    {
        shooter = GetComponent<Shooter>();
        mainCamera = Camera.main;
    }

    void Start()
    {
        InitBounds();
    }

    void Update()
    {
        Move();
        RotateTowardsMouse();
        UpdateIdleDirection(); // Ensure idle matches mouse direction when stationary
    }

    void InitBounds()
    {
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
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2(
            Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight),
            Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop)
        );
        transform.position = newPos;

        // Update last movement direction
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

    void RotateTowardsMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        playerSprite.rotation = Quaternion.Euler(0, 0, angle);
    }

    void UpdateIdleDirection()
    {
        if (rawInput.magnitude == 0)
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mouseWorldPosition - transform.position).normalized;

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
        rawInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (shooter != null)
        {
            shooter.isFiring = value.isPressed;
        }
    }
}
