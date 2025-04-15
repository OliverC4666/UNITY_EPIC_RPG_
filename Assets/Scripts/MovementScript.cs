using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public Animator ANI;
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public LayerMask wallLayer;
    public float Wspeed = 5f;  // Walking speed
    public float Rspeed = 10f; // Running speed

    private Vector2 movement;
    private float moveDir = 0;
    private float moveUpDown = 0;
    private float speed;
    private bool isMoving = false;
    private bool wasMoving = false;

    void Start()
    {
        ANI = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        speed = Wspeed; // Default walking speed
    }

    void Update()
    {
        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // Normalize to prevent diagonal speed boost
        

        isMoving = movement.sqrMagnitude > 0;

        // Update movement direction
        if (movement.x != 0) moveDir = movement.x;
        if (movement.y != 0) moveUpDown = movement.y;

        HandleAnimations();
        ANI.SetInteger("Moving",(int) movement.sqrMagnitude);
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift)) speed = Rspeed;
        else speed = Wspeed;
        // Move the character using Rigidbody2D physics
        if (!IsCollidingWithWall(movement))
        {
            RB.linearVelocity = movement * speed;
        }
        else RB.linearVelocity = new Vector2(0,0);


    }

    void HandleAnimations()
    {
        // Detecting movement change
        if (isMoving)
        {
            // Disable all idle animations when moving
            ResetIdleAnimations();

            // Flip sprite when moving left or right
            if (movement.x != 0)
            {
                SR.flipX = movement.x < 0;
                ANI.SetBool("Turn", true);
            }
            else
            {
                ANI.SetBool("Turn", false);
            }

            // Handle up/down movement animations
            if (movement.y > 0)
            {
                ANI.SetBool("W", true);
                ANI.SetBool("S", false);
            }
            else if (movement.y < 0)
            {
                ANI.SetBool("S", true);
                ANI.SetBool("W", false);
            }

            // If moving diagonally, prioritize "Turn" animation
            if (Mathf.Abs(movement.x) > 0 && Mathf.Abs(movement.y) > 0)
            {
                ANI.SetBool("W", false);
                ANI.SetBool("S", false);
            }
        }
        else
        {
            // If the player stopped moving, return to idle state
            SetIdleState();
        }

        wasMoving = isMoving;
    }

    void ResetIdleAnimations()
    {
        ANI.SetBool("W", false);
        ANI.SetBool("S", false);
        ANI.SetBool("Turn", false);
    }

    void SetIdleState()
    {
        if (!wasMoving) return;

        // Keep the last animation in idle loop
        if (moveDir != 0)
        {
            ANI.SetBool("Turn", true);
        }
        else if (moveUpDown > 0)
        {
            ANI.SetBool("W", true);
        }
        else if (moveUpDown < 0)
        {
            ANI.SetBool("S", true);
        }
    }
    bool IsCollidingWithWall(Vector2 direction)
    {
        float rayLength = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, wallLayer);
        return hit.collider != null; // Returns true if there's a wall
    }
}
