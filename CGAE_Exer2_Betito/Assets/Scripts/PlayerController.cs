using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

    //gun mechanics
    private bool isHoldingGun = false;
    public Sprite idleSprite;
    public Sprite holdingGunSprite;
    public Sprite shootingSprite;



    private void Awake()
    {
        if(FindObjectsOfType<PlayerController>().Length > 1)
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

        if (Input.GetKeyDown(E))
        {
            isHoldingGun = !isHoldingGun;
            UpdateSprite();
        }

        if (isHoldingGun)
        {
            movement.x = 0;
            if (Input.GetKeyDown(Z))
            {
                Shoot();
            }
        } else {
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
        }
    }

    private void UpdateSprite()
    {
        if(isHoldingGun)
        {
            spriteRenderer.sprite = holdingGunSprite;
        } else
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    private void Shoot()
    {
        spriteRenderer.sprite = shootingSprite;
        //add bullet code here in the future

        Invoke("ResetToHoldingGunSprite", 0.1f);
    }

    private void ResetToHoldingGunSprite()
    {
        spriteRenderer.sprite = holdingGunSprite;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Check if on the ground
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Not on the ground anymore
        }
    }
}
