using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Movement")]
    public float jumpForce = 5f;
    public float speed = 5f;
    [SerializeField] private bool isGrounded = true;

    [Header("Player Components")]

    [SerializeField] 
    private Animator playerAnimator;

    [SerializeField] 
    private SpriteRenderer spriteRenderer;
    private float horizontalInput;
    private bool isFacingRight = true;

    private Rigidbody2D rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        FlipSprite();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            playerAnimator.SetBool("isJumping", !isGrounded);
        }
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(horizontalInput * speed, rigidBody.velocity.y);
        playerAnimator.SetFloat("xVelocity", Mathf.Abs(rigidBody.velocity.x));
        playerAnimator.SetFloat("yVelocity", rigidBody.velocity.y);
    }

    private void FlipSprite()
    {
        if (horizontalInput != 0)
        {
            bool shouldFaceRight = horizontalInput > 0f;
            if (shouldFaceRight != isFacingRight)
            {
                isFacingRight = shouldFaceRight;
                spriteRenderer.flipX = !isFacingRight;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = true;
            playerAnimator.SetBool("isJumping", !isGrounded);
        }
    }

}
