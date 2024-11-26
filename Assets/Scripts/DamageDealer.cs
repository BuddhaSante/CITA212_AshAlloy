using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles damage logic for projectiles and other damaging objects in the game.
/// </summary>
public class DamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] int damage = 10; // The amount of damage dealt by this object
    [Tooltip("Enable this to trigger additional effects when damage is dealt.")]
    public bool triggerEffects = true;

    /// <summary>
    /// Gets the amount of damage this object deals.
    /// </summary>
    /// <returns>The damage value.</returns>
    public int GetDamage()
    {
        return damage;
    }

    /// <summary>
    /// Called when this object successfully hits a target.
    /// Can trigger additional effects or destroy the object.
    /// </summary>
    public void Hit()
    {
        if (triggerEffects)
        {
            // Optionally notify other systems (e.g., particles, sound)
            TriggerHitEffects();
        }

        // Destroy the game object after dealing damage
        Destroy(gameObject);
    }

    /// <summary>
    /// Triggers visual or audio effects when damage is dealt.
    /// This method can be expanded for more dynamic feedback.
    /// </summary>
    private void TriggerHitEffects()
    {
        // Example: Play a sound or particle effect here
        Debug.Log("Hit triggered! Add effects like particles or sound here.");
    }
}
