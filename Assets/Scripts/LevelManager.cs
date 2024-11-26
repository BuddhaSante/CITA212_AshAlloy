using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene transitions, including loading game levels, menus, and game-over screens.
/// Also manages delays and cleanup between scenes.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] float sceneLoadDelay = 2f; // Delay before loading the next scene

    private ScoreKeeper scoreKeeper; // Reference to the ScoreKeeper script

    void Awake()
    {
        // Initialize reference to the ScoreKeeper script
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
    }

    /// <summary>
    /// Loads the main gameplay scene and resets the score.
    /// </summary>
    public void LoadGame()
    {
        scoreKeeper.ResetScore(); // Reset score for a new game
        StartCoroutine(WaitAndLoad("Game")); // Load the game scene
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Transition to the main menu
    }

    /// <summary>
    /// Loads the credits scene.
    /// </summary>
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits"); // Transition to the credits screen
    }

    /// <summary>
    /// Triggers the game-over sequence and transitions to the GameOver scene after a delay.
    /// </summary>
    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad("GameOver", sceneLoadDelay)); // Load the game-over scene
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); // Close the application
    }

    /// <summary>
    /// Coroutine to wait for a specified delay before transitioning to a new scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    /// <param name="delay">The delay before loading the scene (default is 0).</param>
    IEnumerator WaitAndLoad(string sceneName, float delay = 0f)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.LoadScene(sceneName); // Load the specified scene
    }
}