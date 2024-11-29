using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles projectile firing for both player and enemy units.
/// Supports AI-controlled firing and adjustable fire rates.
/// </summary>
public class Shooter : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] GameObject projectilePrefab;       // Prefab for the projectile
    [SerializeField] float projectileSpeed = 10f;       // Speed of the projectile
    [SerializeField] float projectileLifetime = 5f;     // Lifetime of the projectile before destruction
    [SerializeField] float baseFiringRate = 0.2f;       // Base firing rate (seconds between shots)

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
    /// Coroutine to continuously fire projectiles at regular intervals.
    /// </summary>
    IEnumerator FireContinuously()
    {
        while (true)
        {
            // Instantiate the projectile at the current position
            GameObject instance = Instantiate(
                projectilePrefab, 
                transform.position, 
                Quaternion.identity
            );

            // Apply velocity to the projectile
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed; // Fire in the "up" direction
            }

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