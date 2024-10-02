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
    public Sprite dyingSprite;

    public int damage = 1;

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
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Change to appropriate key for toggling gun
        {
            isHoldingGun = !isHoldingGun;
            UpdateSprite();
        }

        if (isHoldingGun)
        {
            movement.x = 0; // Stop movement when holding gun

            // Handle horizontal input to flip sprite direction
            if (Input.GetAxis("Horizontal") > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                spriteRenderer.flipX = true;
            }

            if (Input.GetKeyDown(KeyCode.Z)) // Change to your shooting key
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

        // Define the direction based on the player's facing direction
        Vector2 shootDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        // Draw the ray for debugging
        Debug.DrawRay(transform.position, shootDirection * 5f, Color.red, 5f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, shootDirection, 5f); //fix this in the future

        foreach (RaycastHit2D hit in hits) 
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null) 
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("Hit enemy for damage!");
                }
            }
        }

        Invoke("ResetToHoldingGunSprite", 0.1f);
    }



    private void ResetToHoldingGunSprite()
    {
        spriteRenderer.sprite = holdingGunSprite;
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        spriteRenderer.sprite = dyingSprite;
        Destroy(this.gameObject);
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
