using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public float walkSpeed = 50f;
    public float runSpeed = 70f;
    public float jumpForce = 15f;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    private float moveInput;
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
            moveInput = Input.GetAxisRaw("Horizontal");
            isRunning = Input.GetKey(KeyCode.LeftShift);

            // DIRECTION & FLIP
            if (moveInput != 0)
            {
                transform.rotation = Quaternion.Euler(0, moveInput > 0 ? 0 : 180, 0);
            }

            // ANIMATIONS
            anim.SetBool("IsWalking", moveInput != 0);
            anim.SetBool("IsRunning", moveInput != 0 && isRunning);
            anim.SetBool("IsJumping", !isGrounded);
            anim.SetFloat("yVelocity", rb.linearVelocity.y);

            // JUMP INPUT
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
            }
        } 
    }

    void FixedUpdate()
    {  
        if (rb) 
        { 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.AddForce(new Vector2(currentSpeed * moveInput, rb.linearVelocityY),ForceMode2D.Impulse);
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

