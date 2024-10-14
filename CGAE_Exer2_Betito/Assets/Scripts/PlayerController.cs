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
    private Animator animator; // Reference to the Animator

    private bool isHoldingGun = false;
    private bool isTogglingGun = false; // To track if the player is currently toggling the gun
    private bool isShooting = false; // To track if the player is currently shooting

    public Sprite idleSprite;
    public Sprite holdingGunSprite;
    public Sprite shootingSprite;
    public Sprite togglingGun;
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
        animator = GetComponent<Animator>(); // Initialize the Animator
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Change to appropriate key for toggling gun
        {
            if (!isTogglingGun) // Prevent toggling while already in the animation
            {
                StartCoroutine(ToggleGun());
            }
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
                StartCoroutine(Shoot());
            }
        }
        else
        {
            movement.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }

            // Handle horizontal input to flip sprite direction
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
        // Update animator parameters based on the current state
        animator.SetBool("isHoldingGun", isHoldingGun);
        animator.SetFloat("Speed", Mathf.Abs(movement.x));

        // Update the sprite renderer as needed
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

    private IEnumerator ToggleGun()
    {
        isTogglingGun = true; // Prevent other actions during toggling
        animator.SetBool("isHoldingGun", false); // Trigger the toggling animation
        spriteRenderer.sprite = togglingGun; // Optionally change to toggling gun sprite

        // Wait for the toggling animation to finish
        yield return new WaitForSeconds(0.5f); // Adjust duration as needed based on the animation length

        // Toggle the gun state
        isHoldingGun = !isHoldingGun;
        UpdateSprite(); // Update to the correct sprite (holding or idle)

        isTogglingGun = false; // Allow actions again after toggling
    }

    private IEnumerator Shoot()
    {
        isShooting = true; // Prevent other actions during shooting
        animator.SetBool("isShooting", true); // Trigger shooting animation
        spriteRenderer.sprite = shootingSprite;

        // Wait for the shooting animation to finish
        yield return new WaitForSeconds(0.5f); // Adjust shooting duration as needed

        // Define the direction based on the player's facing direction
        Vector2 shootDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        // Draw the ray for debugging
        Debug.DrawRay(transform.position, shootDirection * 5f, Color.red, 5f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, shootDirection, 5f);

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

        // Reset to holding gun sprite after shooting
        spriteRenderer.sprite = holdingGunSprite;
        animator.SetBool("isShooting", false); // Reset shooting state

        isShooting = false; // Allow actions again after shooting
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        spriteRenderer.sprite = dyingSprite;
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueisPlaying)
        {
            return;
        }

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
