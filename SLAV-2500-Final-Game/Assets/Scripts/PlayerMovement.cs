using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Animator animator;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 5f;

    private float horizontalMove;
    private bool jump = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);
        animator.SetFloat("Horizontal Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Is Jumping", true);
        }
        
    }

    public void onLanding()
    {
        jump = false;
        animator.SetBool("Is Jumping", false);
    }
}
