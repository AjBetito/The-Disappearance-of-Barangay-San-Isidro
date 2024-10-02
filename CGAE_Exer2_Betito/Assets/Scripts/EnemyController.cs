using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the enemy moves
    public Transform player; // Reference to the player
    public Sprite idleSprite; // Sprite for idle state
    public Sprite attackingSprite; // Sprite for attacking state
    public Sprite killedSprite; // Sprite for killed state

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Optionally, find the player in the scene if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Make sure your player has the "Player" tag
        }
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Move towards the player's position
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Optionally, flip the sprite based on direction
            spriteRenderer.flipX = direction.x < 0;

            // Check for collision
            if (Vector2.Distance(transform.position, player.position) < 0.1f)
            {
                // Call the method to handle player death
                KillPlayer();
            }
        }
    }

    private void KillPlayer()
    {
        // Change enemy sprite to indicate attack
        spriteRenderer.sprite = attackingSprite;

        // Optionally, call a method in the PlayerController to handle player death
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.Die(); // You need to implement the Die method in your PlayerController
        }

        // Destroy the enemy after a delay, or you could implement a pooling system instead
        Destroy(gameObject, 1f); // Delay before destroying the enemy
    }
}
