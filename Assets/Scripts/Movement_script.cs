using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public float jumpForce = 15f;
    public float attSpeed;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    private float moveInput;

    private int JumpCount = 0;
    private bool isRunning;

    private bool isGrounded;

    void Start()
    {
        if (!anim) anim = GetComponent<Animator>();
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb) 
        {
            
            // INPUT
            moveInput = Mathf.Clamp(Input.GetAxisRaw("Horizontal"), -1f, 1f);
            isRunning = Input.GetKey(KeyCode.LeftShift);

            if (moveInput != 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, moveInput > 0 ? 0 : 180, 0));
            }
            // DIRECTION & FLIP
            if (Input.GetKeyDown(KeyCode.Space) && JumpCount < 2)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                JumpCount++;

            }
            // ANIMATIONS
            anim.SetBool("IsWalking", moveInput != 0);
            anim.SetBool("IsRunning", moveInput != 0 && isRunning);
            anim.SetBool("IsJumping", !isGrounded);
            anim.SetFloat("attSpeed", attSpeed);
            //Debug.Log($"Velocity: {rb.linearVelocity}, IsGrounded: {isGrounded}, JumpCount: {JumpCount}");
            // JUMP INPUT

        } 
    }

    void FixedUpdate()
    {  
        if (rb) 
        {

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
            if (isGrounded) JumpCount = 0;
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            rb.linearVelocity = new Vector2(currentSpeed * moveInput, rb.linearVelocityY);

        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }


}

