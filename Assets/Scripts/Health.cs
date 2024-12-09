using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Required for using UnityEvent

/// <summary>
/// Handles the health system for players and enemies, including damage handling,
/// death effects, and optional camera shake.
/// Now includes functionality for bosses with defeat events.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] bool isPlayer;           // Whether this entity is the player
    [SerializeField] int maxHealth = 100;     // Maximum health for this entity
    [SerializeField] int scoreValue = 50;     // Points awarded when this entity is defeated
    [SerializeField] ParticleSystem hitEffect; // Optional visual effect when damaged

    private int currentHealth;                // Tracks current health during gameplay

    [Header("Camera Shake")]
    [SerializeField] bool applyCameraShake;   // Whether to apply camera shake when hit
    private CameraShake cameraShake;          // Reference to the CameraShake script

    private AudioPlayer audioPlayer;          // Reference to the AudioPlayer script for sound effects
    private ScoreKeeper scoreKeeper;          // Reference to the ScoreKeeper script for updating score
    private LevelManager levelManager;        // Reference to the LevelManager script for game over transitions

    [Header("Boss Settings")]
    [SerializeField] bool isBoss = false;     // Flag to indicate if this is a boss
    [SerializeField] UnityEvent onBossDefeated; // Event triggered when the boss is defeated

    void Awake()
    {
        // Initialize references to other components and scripts
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        // Set initial health to maximum value
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Detects collisions with objects capable of dealing damage and processes the damage.
    /// </summary>
    /// <param name="other">The collider that triggered this event.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null) // Ensure the collider has a DamageDealer component
        {
            TakeDamage(damageDealer.GetDamage()); // Apply the damage
            PlayHitEffect(); // Show visual feedback for the hit
            audioPlayer.PlayDamageClip(); // Play damage sound effect
            ShakeCamera(); // Apply camera shake (if enabled)
            damageDealer.Hit(); // Notify the DamageDealer that it hit successfully
        }
    }

    /// <summary>
    /// Reduces health by a specified amount and checks if the entity should die.
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
    void TakeDamage(int damage)
    {
        currentHealth -= damage; // Subtract damage from current health
        if (currentHealth <= 0)  // Check if health is depleted
        {
            Die();
        }
    }

    /// <summary>
    /// Handles logic when the entity's health reaches zero.
    /// Triggers special logic for bosses and ends the game if the player dies.
    /// </summary>
    void Die()
    {
        if (!isPlayer)
        {
            if (isBoss && onBossDefeated != null)
            {
                onBossDefeated.Invoke(); // Trigger the boss-defeated event
            }
            scoreKeeper.ModifyScore(scoreValue); // Award score for defeating this entity
        }
        else
        {
            levelManager.LoadGameOver(); // End the game if the player dies
        }

        Destroy(gameObject); // Remove the entity from the scene
    }

    /// <summary>
    /// Plays a particle effect at the entity's position.
    /// </summary>
    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            // Create a temporary particle system and destroy it after it finishes
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    /// <summary>
    /// Triggers a camera shake effect if enabled and available.
    /// </summary>
    void ShakeCamera()
    {
        if (cameraShake != null && applyCameraShake)
        {
            cameraShake.Play();
        }
    }

    /// <summary>
    /// Retrieves the current health of the entity.
    /// Useful for UI or debugging purposes.
    /// </summary>
    /// <returns>The current health value.</returns>
    public int GetHealth()
    {
        return currentHealth;
    }
}