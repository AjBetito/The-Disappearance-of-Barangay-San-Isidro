using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    private bool isPlayerAlive = true;

    // Sprites for different enemy states
    public Sprite idleSprite;
    public Sprite attackingSprite;
    public Sprite killedPlayerSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on the enemy!");
        }

        // Try to find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player object is tagged 'Player'.");
        }
        else
        {
            Debug.Log("Enemy script started and player found.");
        }

        // Set the initial idle sprite
        if (idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    private void Update()
    {
        if (isPlayerAlive && player != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move the enemy towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Flip the sprite based on direction
        spriteRenderer.flipX = direction.x < 0;

        // Check for collision with the player
        if (Vector2.Distance(transform.position, player.position) < 0.1f)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController != null && isPlayerAlive)
        {
            playerController.Die(); // Call player's Die method to handle death logic
            ChangeToKilledPlayerState(); // Change the enemy sprite to indicate that it killed the player
        }
    }

    private void ChangeToKilledPlayerState()
    {
        // Change the enemy's sprite to the killedPlayerSprite after killing the player
        if (killedPlayerSprite != null)
        {
            spriteRenderer.sprite = killedPlayerSprite;
        }
        else
        {
            Debug.LogError("Killed player sprite is not assigned!");
        }

        // Optionally, stop the enemy from moving after the player is killed
        isPlayerAlive = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPlayerAlive)
        {
            KillPlayer();
        }
    }
}
