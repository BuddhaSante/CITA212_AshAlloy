using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages damage logic for projectiles or other damage-dealing objects.
/// This script allows flexibility for triggering additional effects when an object deals damage.
/// </summary>
public class DamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] int damage = 10; // The amount of damage dealt by this object
    [Tooltip("Enable this to trigger visual/audio effects when damage is dealt.")]
    public bool triggerEffects = true; // Toggle to activate hit effects

    /// <summary>
    /// Gets the damage value dealt by this object.
    /// </summary>
    /// <returns>Integer value of the damage.</returns>
    public int GetDamage()
    {
        return damage;
    }

    /// <summary>
    /// Called when this object successfully deals damage.
    /// Handles destruction and optional visual/audio effects.
    /// </summary>
    public void Hit()
    {
        if (triggerEffects)
        {
            TriggerHitEffects(); // Optionally trigger effects like particles or sounds
        }

        // Destroy the game object after dealing damage
        Destroy(gameObject);
    }

    /// <summary>
    /// Triggers visual or audio effects when damage is dealt.
    /// Expand this function for customized feedback.
    /// </summary>
    private void TriggerHitEffects()
    {
        // Placeholder: Add particle effects, sound effects, or screen shake here
        Debug.Log("Damage dealt! Add visual or sound effects here.");
    }
}
