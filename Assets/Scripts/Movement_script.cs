using UnityEngine;
using System.Collections;
using static UnityEngine.UI.Image;
using Unity.VisualScripting;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public float Wspeed = 5f;      // Walking speed
    public float Rspeed = 10f;     // Running speed
    public float jumpHeight = 10f; // Jump strength
    private bool isGrounded = true; // Initially grounded
    private float speed; // Current movement speed

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        speed = Wspeed; // Default walking speed
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        transform.Translate(Mathf.Abs(move) * speed * Time.deltaTime * Vector3.right);
        if (move != 0) anim.SetBool("IsWalking", true);
        else anim.SetBool("IsWalking", false);

        // Flip character sprite
        if (move < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);  // Rotate character
        else if (move > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);   // Reset to default

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            anim.SetBool("IsJumping", true);
            StartCoroutine(Jump());
        }

        // Sprinting (Hold Shift to run, release to walk)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Rspeed;
            anim.SetBool("IsRunning", true);
        }
        else
        {
            speed = Wspeed;
            anim.SetBool("IsRunning", false);
        }

    }

    IEnumerator Jump()
    {
        float jumpSpeed = jumpHeight / 0.5f;
        float gravity = jumpSpeed / 0.5f;
        float velocity = jumpSpeed;

        // Ascend
        while (velocity > 0)
        {
            transform.Translate(Time.deltaTime * velocity * Vector3.up);
            velocity -= gravity * Time.deltaTime;
            yield return null;
        }

        // Descend
        while (!IsGrounded())
        {
            transform.Translate(gravity * Time.deltaTime * Vector3.down);
            yield return null;
        }

        isGrounded = true;
        anim.SetBool("IsJumping", !isGrounded);
        anim.SetFloat("JumpSpeed", Mathf.Abs(velocity)*100);
    }

    bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector2.down * 0.1f, Color.red);
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
    }
}
