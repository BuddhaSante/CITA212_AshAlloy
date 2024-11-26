using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles camera shake effects to emphasize gameplay events, such as explosions or damage.
/// </summary>
public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] float defaultShakeDuration = 1f;   // Default duration of the shake
    [SerializeField] float defaultShakeMagnitude = 0.5f; // Default intensity of the shake

    private Vector3 initialPosition; // Stores the camera's original position for restoration

    void Start()
    {
        // Save the camera's starting position
        initialPosition = transform.position;
    }

    /// <summary>
    /// Starts a shake effect with default settings.
    /// </summary>
    public void Play()
    {
        StartCoroutine(Shake(defaultShakeDuration, defaultShakeMagnitude));
    }

    /// <summary>
    /// Starts a shake effect with custom duration and magnitude.
    /// </summary>
    /// <param name="duration">Duration of the shake.</param>
    /// <param name="magnitude">Intensity of the shake.</param>
    public void Play(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    /// <summary>
    /// Coroutine to perform the shake effect.
    /// </summary>
    /// <param name="duration">Duration of the shake.</param>
    /// <param name="magnitude">Intensity of the shake.</param>
    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsedTime = 0f; // Tracks the elapsed time

        while (elapsedTime < duration)
        {
            // Generate a random offset within a circular range
            Vector3 randomOffset = Random.insideUnitCircle * magnitude;
            transform.position = initialPosition + randomOffset;

            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }

        // Reset the camera to its original position
        transform.position = initialPosition;
    }
}