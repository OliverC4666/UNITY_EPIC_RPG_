using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    public Transform player;
    public Animator anim;
    public SpriteRenderer sr;
    public EnemyAttack EnemyAttack;
    public GameObject attackHitbox;
    public Transform patrolBorder;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 5f;
    public float attackRange = 1f;

    public Transform leftPoint, rightPoint;

    private bool isChasing = false;
    private bool isAttacking = false;
    private bool movingRight = true;
    private float timeSinceLastSeen = 0f;
    private readonly float loseSightDuration = 2f;

    public void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (patrolBorder != null)
        {
            patrolBorder.SetParent(null); // Detach from enemy, keeping its world position
        }
        // Ensure enemy starts facing the correct direction
        Flip(movingRight);
    }

    private void Update()
    {
        if (player != null) 
        { 
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                StartCoroutine(Attack("Slash Attack"));
            }
            else if (distanceToPlayer <= detectionRange && !isAttacking)
            {
                isChasing = true;
                timeSinceLastSeen = 0f;
            }
            else
            {
                timeSinceLastSeen += Time.deltaTime;
                if (timeSinceLastSeen > loseSightDuration)
                    isChasing = false;
            }

            if (!isAttacking)
            {
                anim.SetBool("IsRunning", isChasing);

                if (isChasing)
                    ChasePlayer();
                else
                    Patrol();
            }
        }
    }

    void Patrol()
    {
        float moveDirection = movingRight ? 1 : -1;
        transform.position += new Vector3(moveDirection * patrolSpeed * Time.deltaTime, 0, 0);
        anim.SetBool("IsWalking", !isChasing);

        if (movingRight && transform.position.x >= rightPoint.position.x)
        {
            movingRight = false;
            Flip(movingRight);
        }
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
        {
            movingRight = true;
            Flip(movingRight);
        }
    }

    void ChasePlayer()
    {
        float moveDirection = player.position.x > transform.position.x ? 1 : -1;
        transform.position += new Vector3(moveDirection * chaseSpeed * Time.deltaTime, 0, 0);

        // Flip direction if necessary
        Flip(moveDirection > 0);
    }

    IEnumerator Attack(string MoveName)
    {
        isAttacking = true; // Prevents movement or other actions
        anim.SetTrigger("Attack"); // Play attack animation

        foreach (var config in EnemyAttack.AttackConf)
        {
            if (config.name == MoveName)
            {
                yield return new WaitForSeconds(config.attackCooldown / 2); // Prepare for attack

                // Enable hitbox for a short time
                config.Attack = true;
                attackHitbox.SetActive(config.Attack);
                yield return new WaitForSeconds(0.1f);
                                                        // Hitbox active for 0.1s
                attackHitbox.SetActive(config.Attack); // Disable after attack

                yield return new WaitForSeconds(config.attackCooldown / 2); // Finish attack cooldown
                config.Attack = false;
            }

        }

        isAttacking = false; // Allow attacking again
    }

    // ✅ Correctly flips the enemy sprite
    void Flip(bool faceRight)
    {
        transform.rotation = faceRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }
    public bool GetIsChasing()
    {
        return isChasing;
    }
    public bool GetIsAttacking()
    {
        return isAttacking;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackHitbox.transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
