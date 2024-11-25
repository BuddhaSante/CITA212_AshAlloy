using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    EnemySpawner enemySpawner; // Reference to the enemy spawner script
    WaveConfigSO waveConfig;    // Stores the configuration of the current wave
    List<Transform> waypoints;  // Holds the waypoints for enemy pathing
    int waypointIndex = 0;      // Current index of the waypoint to follow
    
    void Awake()
    {
        // Finds the EnemySpawner component in the scene
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        // Initializes the wave configuration and sets up waypoints
        waveConfig = enemySpawner.GetCurrentWave();
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].position; // Starts at the first waypoint
    }

    void Update()
    {
        // Moves the enemy along the designated path
        FollowPath();
    }

    // Method to move the enemy towards the next waypoint
    void FollowPath()
    {
        // If there are more waypoints to follow
        if (waypointIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointIndex].position; // Target position
            float delta = waveConfig.GetMoveSpeed() * Time.deltaTime;   // Adjusts speed by frame time
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta); // Moves enemy

            // When the enemy reaches the target position
            if (transform.position == targetPosition)
            {
                waypointIndex++; // Advances to the next waypoint
            }
        }
        else
        {
            Destroy(gameObject); // Destroys the enemy when the path is completed
        }
    }
}
