using UnityEngine;

/// <summary>
/// Handles movement and behavior specific to the boss.
/// Allows for dynamic targeting of the player while leaving room for unique boss mechanics.
/// </summary>
public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed at which the boss moves toward its target.")]
    [SerializeField] float moveSpeed = 2f; // Boss's movement speed, generally slower than regular enemies.

    private Transform target; // The target the boss will move toward (e.g., the player).

    void Start()
    {
        // Attempt to automatically find the player as the target, if not set manually.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            SetTarget(player.transform);
        }
    }

    void Update()
    {
        // If a target exists, move toward it.
        if (target != null)
        {
            MoveTowardTarget();
        }
    }

    /// <summary>
    /// Assigns the target for the boss to follow dynamically.
    /// </summary>
    /// <param name="newTarget">The Transform of the target to follow.</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget; // Assign the target (e.g., the player).
    }

    /// <summary>
    /// Moves the boss toward its assigned target.
    /// </summary>
    private void MoveTowardTarget()
    {
        // Calculate the direction vector to the target.
        Vector3 direction = (target.position - transform.position).normalized;

        // Adjust the boss's position using the direction and speed.
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optionally rotate the boss to face the target for visual accuracy.
        RotateTowardTarget(direction);
    }

    /// <summary>
    /// Rotates the boss to face the target or its movement direction.
    /// </summary>
    /// <param name="direction">The normalized direction vector toward the target.</param>
    private void RotateTowardTarget(Vector3 direction)
    {
        // Calculate the angle to rotate the boss to face the target.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the Z-axis (used in 2D games for top-down views).
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}