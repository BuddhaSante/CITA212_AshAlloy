using UnityEngine;

/// <summary>
/// Controls enemy movement by dynamically targeting and moving toward the player.
/// Provides smooth, continuous tracking of the target's position.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed at which the enemy moves toward the target.")]
    [SerializeField] float moveSpeed = 3f; // Speed of enemy movement

    private Transform target; // The current target the enemy is moving toward

    void Update()
    {
        // Ensure the enemy moves toward the target, if a target is set
        if (target != null)
        {
            MoveTowardTarget();
        }
    }

    /// <summary>
    /// Sets the target for the enemy to move toward.
    /// Typically used to set the player as the target when the enemy spawns.
    /// </summary>
    /// <param name="newTarget">The target Transform to follow.</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget; // Assign the new target
    }

    /// <summary>
    /// Moves the enemy toward its assigned target at the configured speed.
    /// </summary>
    private void MoveTowardTarget()
    {
        // Calculate the direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Move the enemy closer to the target
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optionally, rotate the enemy to face the target (can be omitted for 2D top-down)
        RotateTowardTarget(direction);
    }

    /// <summary>
    /// Rotates the enemy to face its movement direction.
    /// Useful for top-down games where the enemy should "look" at the player.
    /// </summary>
    /// <param name="direction">The normalized direction vector to the target.</param>
    private void RotateTowardTarget(Vector3 direction)
    {
        // Calculate the angle required to face the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to the enemy (Z-axis for 2D rotation)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
