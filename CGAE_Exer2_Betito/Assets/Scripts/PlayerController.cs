using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

   
    private bool isHoldingGun = false;
    public Sprite idleSprite;
    public Sprite holdingGunSprite; 
    public Sprite shootingSprite; 
    public Sprite runningSprite; 

    private void Awake()
    {
        if (FindObjectsOfType<PlayerController>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isHoldingGun = !isHoldingGun;
            UpdateSprite();
        }

        if (isHoldingGun)
        {
            movement.x = 0;

            if (Input.GetAxis("Horizontal") > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                spriteRenderer.flipX = true;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            movement.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }

            if (movement.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (movement.x < 0)
            {
                spriteRenderer.flipX = true;
            }

            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        if (isHoldingGun)
        {
            spriteRenderer.sprite = holdingGunSprite;
        }
        else
        {
            if (movement.x != 0)
            {
                spriteRenderer.sprite = runningSprite;
            }
            else
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
    }

    private void Shoot()
    {
        spriteRenderer.sprite = shootingSprite;
        // Add bullet stuff here soon

        Invoke("ResetToHoldingGunSprite", 0.1f);
    }

    private void ResetToHoldingGunSprite()
    {
        spriteRenderer.sprite = holdingGunSprite;
    }

    public void Die()
    {
        // Implement what happens when the player dies, e.g., resetting the game, showing a game over screen, etc.
        Debug.Log("Player has died!");

        // Optionally, reset the player position or reload the scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the current scene
    }

    void FixedUpdate()
    {

        if (!isHoldingGun)
        {
            rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; 
        }
    }
}
