using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad; // Name of the scene to load
    [SerializeField] private Vector3 spawnPosition = new Vector3(-10f, 0f, 0f); // Position on the left side

    // Detect collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player (assuming the player has the tag "Player")
        if (collision.gameObject.CompareTag("Player"))
        {
            // Set the player spawn position for the next scene
            GameData.playerSpawnPosition = spawnPosition;

            // Load the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
