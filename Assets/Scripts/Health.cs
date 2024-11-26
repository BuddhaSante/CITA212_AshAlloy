using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the health system for players and enemies, including damage handling,
/// death effects, and optional camera shake.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] bool isPlayer;           // Whether this is the player object
    [SerializeField] int maxHealth = 100;     // Maximum health for this object
    [SerializeField] int scoreValue = 50;     // Score awarded when this object is destroyed
    [SerializeField] ParticleSystem hitEffect; // Visual effect triggered on hit

    private int currentHealth;                // Tracks the current health value

    [Header("Camera Shake")]
    [SerializeField] bool applyCameraShake;   // Whether to apply camera shake on hit
    private CameraShake cameraShake;          // Reference to the CameraShake script

    private AudioPlayer audioPlayer;          // Reference to the AudioPlayer script
    private ScoreKeeper scoreKeeper;          // Reference to the ScoreKeeper script
    private LevelManager levelManager;        // Reference to the LevelManager script

    void Awake()
    {
        // Initialize references to other scripts
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        // Initialize health to the maximum value at the start
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Handles collision detection for incoming damage.
    /// </summary>
    /// <param name="other">The Collider2D that triggered the event.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            TakeDamage(damageDealer.GetDamage()); // Apply damage
            PlayHitEffect();
            audioPlayer.PlayDamageClip();
            ShakeCamera();
            damageDealer.Hit(); // Notify the damage dealer that it has hit
        }
    }

    /// <summary>
    /// Reduces health by the given damage amount.
    /// Triggers death logic if health falls to or below zero.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles the destruction of the object.
    /// Updates the score if this is not the player and triggers game over for the player.
    /// </summary>
    void Die()
    {
        if (!isPlayer)
        {
            scoreKeeper.ModifyScore(scoreValue); // Add score for enemy death
        }
        else
        {
            levelManager.LoadGameOver(); // Trigger game over for the player
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Plays a particle effect at the object's location.
    /// </summary>
    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    /// <summary>
    /// Triggers a camera shake effect if enabled.
    /// </summary>
    void ShakeCamera()
    {
        if (cameraShake != null && applyCameraShake)
        {
            cameraShake.Play();
        }
    }

    /// <summary>
    /// Gets the current health value.
    /// </summary>
    /// <returns>The current health.</returns>
    public int GetHealth()
    {
        return currentHealth;
    }
}