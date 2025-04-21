// PlayerMovement.cs (ground‑check via OverlapCircle for dynamic 2D platformer)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    public bool isDead = false;
    private bool deathHandled = false;

    private Vector3 spawnPoint;

    private void Awake()
    {
        // cache your start position
        spawnPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // call this to play death anim and stop movement
    public void Die()
    {
        if (deathHandled) return;
        deathHandled = true;
        isDead = true;

        // 1) fire the trigger
        anim.SetBool("isJumping", false);
        anim.SetTrigger("isDead");

        // 2) immediate physics shutdown
        rb.velocity = Vector2.zero;

        // 3) figure out how long our death animation really is
        float deathClipLength = 0f;
        foreach (var clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Player_Dead")  
            {
                deathClipLength = clip.length;
                break;
            }
        }

        // 4) reload *after* the death animation has time to play
        StartCoroutine(ReloadAfterDeath(deathClipLength));
    }

    // private void Awake()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    //     anim = GetComponent<Animator>();
    //     spriteRenderer = GetComponent<SpriteRenderer>();
    // }

    private void Update()
    {
        if (isDead) return;
        // 1. Read input
        horizontalInput = Input.GetAxis("Horizontal");

        // 2. Flip sprite
        FlipSprite();

        // 3. Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 4. Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 5. Update animator
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isJumping", !isGrounded);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        // Move horizontally
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private IEnumerator ReloadAfterDeath(float delay)
    {
        // if for some reason we couldn't find the clip, default to 1s
        yield return new WaitForSeconds(delay > 0 ? delay : 1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // private IEnumerator RespawnAfterDeath(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     transform.position = spawnPoint;
    //     deathHandled = false;
    //     isDead = false;
    //     anim.ResetTrigger("Death");
    //     this.enabled = true;    // re‑enable movement
    // }

    private void FlipSprite()
    {
        if (horizontalInput > 0 && !isFacingRight || horizontalInput < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !isFacingRight;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
#endif
}
