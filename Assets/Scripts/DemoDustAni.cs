using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator dustAnimator; // Assign the dust Animator in Inspector
    public bool isRunning;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        playerAnimator.SetBool("isRunning", isRunning);
        dustAnimator.SetBool("isRunning", isRunning);
    }
}
