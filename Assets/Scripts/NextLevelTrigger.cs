using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene transitions

/// <summary>
/// Manages level transitions when the player enters a trigger zone.
/// Can be used for moving to the next level, boss levels, or other game scenes.
/// </summary>
public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // The name of the next scene to load.

    /// <summary>
    /// Called when another collider enters the trigger zone.
    /// </summary>
    /// <param name="other">The Collider2D that entered the trigger zone.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player (using the "Player" tag)
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    /// <summary>
    /// Loads the specified scene using Unity's SceneManager.
    /// This must be public to be used in UnityEvents.
    /// </summary>
    public void LoadNextScene()
    {
        Debug.Log($"Loading next scene: {nextSceneName}");
        // Ensure the next scene name is set; avoid errors if the field is empty
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); // Loads the scene specified in the Inspector
        }
        else
        {
            Debug.LogError("Next scene name is not specified in the NextLevelTrigger!");
        }
    }
}
