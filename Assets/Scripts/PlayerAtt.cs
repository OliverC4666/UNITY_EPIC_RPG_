using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    public List<AttackOptionP> AttackConf = new(); // Configurable attacks
    public LayerMask EnemyLayer;                   // Defines what counts as an enemy
    public float trackingDuration = 0.5f;          // Time window to track combos
    public float attackRange;
    public GameObject attackHitbox;                // The attack hitbox GameObject
    public PlayerStats playerStats;                // Reference to ScriptableObject

    private int counterClick = 0;
    private bool isTracking = false;
    private bool hitboxActive = false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (playerStats == null)
        {
            Debug.LogError("⚠️ PlayerStats (ScriptableObject) not assigned!");
        }

        if (attackHitbox == null)
        {
            Debug.LogError("⚠️ AttackHitbox is not assigned to PlayerAttack script!");
        }
    }

    void Update()
    {
        foreach (var config in AttackConf)
        {
            if (Input.GetKeyDown(config.button))
            {
                counterClick++;
                anim.SetInteger("ClickCounter", counterClick);
                anim.SetTrigger(config.TriggerKey);

                if (!isTracking)
                {
                    StartCoroutine(ResetCounter());
                }
            }
        }
    }

    public void EnableHitbox()
    {
        hitboxActive = true;

        int attackIndex = Mathf.Clamp(counterClick - 1, 0, AttackConf.Count - 1);
        var currentAttack = AttackConf[attackIndex];

        Attack(currentAttack.AttackRange, currentAttack.baseDamage + playerStats.Get("power") / 100f);
    }

    public void DisableHitbox()
    {
        hitboxActive = false;
    }

    public void Attack(float range, float attackDamage)
    {
        if (!hitboxActive) return;

        if (attackHitbox == null)
        {
            Debug.LogError("⚠️ AttackHitbox is missing. Assign it in the Inspector!");
            return;
        }

        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(attackHitbox.transform.position, range, EnemyLayer);

        foreach (Collider2D target in targetsHit)
        {
            if (target.TryGetComponent<Health>(out var enemy))
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"✅ Hit {target.gameObject.name} for {attackDamage} damage!");
            }
            else
            {
                Debug.LogWarning($"⚠️ {target.gameObject.name} is missing an EnemyHealth component!");
            }
        }
    }

    IEnumerator ResetCounter()
    {
        isTracking = true;
        yield return new WaitForSeconds(trackingDuration);
        counterClick = 0;
        anim.SetInteger("ClickCounter", counterClick);
        isTracking = false;
    }

    public void OnDrawGizmos()
    {
        if (attackHitbox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackHitbox.transform.position, attackRange);
        }
    }
}
