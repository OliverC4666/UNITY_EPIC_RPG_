using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public PlayerStats playerStats;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;


    private float baseWalkSpeed = 1f;
    private float baseRunSpeed = 3f;
    private float baseJumpForce = 15f;
    private float baseAttSpeed = 1f;
    private int JumpCount = 0;
    private float currentSpeed = 1f;
    private bool isRunning;
    private bool isGrounded;
    private float moveInput;

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
                rb.AddForce(Vector2.up * baseJumpForce, ForceMode2D.Impulse);
                JumpCount++;

            }
            // ANIMATIONS
            anim.SetBool("IsWalking", moveInput != 0);
            anim.SetBool("IsRunning", moveInput != 0 && isRunning);
            anim.SetBool("IsJumping", !isGrounded);
            anim.SetFloat("attSpeed", baseAttSpeed + playerStats.Get("attack speed")/100f);
            anim.SetFloat("MoveSpeed", currentSpeed + playerStats.Get("speed")/100f);
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
            currentSpeed = isRunning ? baseRunSpeed : baseWalkSpeed;
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

