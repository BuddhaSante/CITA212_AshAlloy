using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles projectile firing for both player and enemy units.
/// Supports firing toward the mouse click and adjustable fire rates.
/// </summary>
public class Shooter : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] GameObject projectilePrefab;       // Prefab for the projectile
    [SerializeField] float projectileSpeed = 10f;       // Speed of the projectile
    [SerializeField] float projectileLifetime = 5f;     // Lifetime of the projectile before destruction
    [SerializeField] float baseFiringRate = 0.2f;       // Base firing rate (seconds between shots)
    [SerializeField] Transform firingPoint;             // Point from where the projectile is fired
    [SerializeField] Camera mainCamera;                 // Reference to the main camera for mouse position

    [Header("AI Settings")]
    [SerializeField] bool useAI = false;                // Whether this shooter is AI-controlled
    [SerializeField] float firingRateVariance = 0.1f;   // Variance to make firing intervals dynamic
    [SerializeField] float minimumFiringRate = 0.1f;    // Minimum firing rate to prevent excessively fast shots

    [HideInInspector] public bool isFiring = false;     // Tracks whether the shooter is actively firing

    private Coroutine firingCoroutine;                  // Reference to the firing coroutine
    private Animator playerAnimator;                    // Reference to the Animator for firing animations
    private AudioPlayer audioPlayer;                    // Reference to the AudioPlayer script

    void Awake()
    {
        // Initialize reference to the Animator and AudioPlayer
        playerAnimator = GetComponent<Animator>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        // Automatically enable firing for AI-controlled shooters
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        HandleFiring(); // Continuously check and handle firing state
    }

    /// <summary>
    /// Starts or stops firing based on the current firing state.
    /// </summary>
    void HandleFiring()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }

        // Update Animator firing state
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isFiring", isFiring);
        }
    }

    /// <summary>
    /// Coroutine to continuously fire projectiles at regular intervals toward the mouse position.
    /// </summary>
    IEnumerator FireContinuously()
{
    while (true)
    {
        // Determine target position (player or any target)
        Vector3 targetPosition = transform.up; // Default direction for AI
        if (!useAI)
        {
            // For player-controlled shooting, use the mouse
            Vector3 mouseScreenPosition = Input.mousePosition;
            targetPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        }
        else
        {
            // For AI, target the player
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // Ensure player has "Player" tag
            if (player != null)
            {
                targetPosition = player.transform.position;
            }
        }

        // Calculate direction to target
        Vector2 directionToTarget = (targetPosition - firingPoint.position).normalized;

        // Instantiate the projectile at the firing point
        GameObject instance = Instantiate(
            projectilePrefab,
            firingPoint.position,
            Quaternion.identity
        );

        // Apply velocity to the projectile
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = directionToTarget * projectileSpeed;
        }

        // Rotate the projectile to face the target
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        instance.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Destroy the projectile after its lifetime expires
        Destroy(instance, projectileLifetime);

        // Play shooting sound (if an AudioPlayer is linked)
        if (audioPlayer != null)
        {
            audioPlayer.PlayShootingClip();
        }

        // Calculate the time until the next projectile is fired
        float timeToNextProjectile = Random.Range(
            baseFiringRate - firingRateVariance,
            baseFiringRate + firingRateVariance
        );

        // Clamp the interval to ensure it stays above the minimum firing rate
        timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

        // Wait for the calculated interval before firing again
        yield return new WaitForSeconds(timeToNextProjectile);
    }
}
}