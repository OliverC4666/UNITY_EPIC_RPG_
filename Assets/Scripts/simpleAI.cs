using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    
    public LayerMask PlayerLayer;
    public List<AttackOptionE> attackOptions = new();
    public GameObject attackHitbox;
    public Transform patrolBorder;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 5f;
    public float attackRange = 1f;

    public Transform leftPoint, rightPoint;

    protected Animator anim ;
    protected SpriteRenderer sr;
    private bool isChasing = false;
    private bool movingRight = true;
    private float timeSinceLastSeen = 0f;
    private readonly float loseSightDuration = 2f;
    private bool isAttacking = false;
    private bool hitboxActive = false;

    private int currentAttackIndex = 0;
    private float attackCooldownTimers ; // One per attack

    public void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (patrolBorder != null)
            patrolBorder.SetParent(null);

        if (attackHitbox == null)
            Debug.LogError("⚠️ AttackHitbox is not assigned!");

        Flip(movingRight);

        // Init cooldown timers
            attackCooldownTimers = 0f;
    }

    private void Update()
    {
        if (player == null) return;

        // Reduce cooldowns over time
        attackCooldownTimers -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && !isAttacking)
        {

            int i = 0;
                var config = attackOptions[i];
                if (config.Attack && attackCooldownTimers <= 0f)
                {
                    anim.SetTrigger(config.TriggerKey);
                Debug.Log("ENEMY ATTACKED!");
                    attackCooldownTimers = config.attackCooldown; // Start cooldown

                }

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
        Flip(moveDirection > 0);
    }

    public void EnableHitbox()
    {
        Debug.Log(">> EnableHitbox called!");

        if (attackHitbox == null)
        {
            Debug.LogError("⚠️ AttackHitbox is missing.");
            return;
        }

        hitboxActive = true;
        isAttacking = true;

        var currentAttack = attackOptions[currentAttackIndex];
        Attack(currentAttack.AttackRange, currentAttack.Damage);
    }

    public void DisableHitbox()
    {
        hitboxActive = false;
        isAttacking = false;
    }

    private void Attack(float range, int attackDamage)
    {
        if (!hitboxActive || attackHitbox == null) return;

        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(attackHitbox.transform.position, range, PlayerLayer);

        foreach (Collider2D target in targetsHit)
        {
            if (target.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(attackDamage);
                Debug.Log($"✅ Hit {target.gameObject.name} for {attackDamage} damage!");
            }
            else
            {
                Debug.LogWarning($"⚠️ {target.gameObject.name} missing Health component!");
            }
        }
    }

    void Flip(bool faceRight)
    {
        transform.rotation = faceRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }

    public bool GetIsChasing() => isChasing;
    public bool GetIsAttacking() => isAttacking;

    void OnDrawGizmosSelected()
    {
        if (attackHitbox == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackHitbox.transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
