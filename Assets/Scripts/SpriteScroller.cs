using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the scrolling of a sprite's texture to create a dynamic background effect.
/// Can also support parallax scrolling by factoring in an external velocity (e.g., camera movement).
/// </summary>
public class SpriteScroller : MonoBehaviour
{
    [Header("Scrolling Settings")]
    [SerializeField] Vector2 moveSpeed = new Vector2(0.1f, 0f); // Scrolling speed in the X and Y directions
    [Tooltip("Optional velocity to simulate parallax effects.")]
    [SerializeField] Vector2 externalVelocity; // Can be used for parallax scrolling based on camera/player movement

    private Vector2 offset; // Stores the texture offset
    private Material material; // Reference to the material used by the sprite

    void Awake()
    {
        // Retrieve the material of the sprite for texture manipulation
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        Scroll(); // Continuously update texture offset
    }

    /// <summary>
    /// Handles the scrolling of the texture by updating its offset.
    /// Can include external velocity for parallax effects.
    /// </summary>
    void Scroll()
    {
        // Calculate the texture offset based on scrolling speed and external velocity
        offset = (moveSpeed + externalVelocity) * Time.deltaTime;

        // Update the texture's main offset property to simulate scrolling
        material.mainTextureOffset += offset;
    }
}
