using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour
{
    public List<AttackOptionE> AttackConf = new(); // Configurable attacks
    public GameObject attackHitbox;    // The attack hitbox GameObject
    public float DemoRange;
    public LayerMask PlayerLayer;     // Defines what counts as an enemy/player
    private Animator anim;
    private bool hitboxActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        anim = GetComponent<Animator>();

        if (attackHitbox == null)
        {
            Debug.LogError("⚠️ AttackHitbox is not assigned to  script!");
        }
    }

    void Update()
    {
        foreach (var config in AttackConf)
        {
            if (config.Attack) // Check for Attack enabled
            {   
                anim.SetTrigger(config.TriggerKey); // Trigger attack animation
                Attack(config.AttackRange, config.Damage); // Use attack settings
                config.Attack = false;
            }
        }
    }
    public void EnableHitbox()
    {
        hitboxActive = true;

        int attackIndex = 0;
        var currentAttack = AttackConf[attackIndex];

        Attack(currentAttack.AttackRange, currentAttack.Damage);
    }

    public void DisableHitbox()
    {
        hitboxActive = false;
    }
    public void Attack(float range, int attackDamage)
    {
        if (!hitboxActive) return;

        if (attackHitbox == null)
        {
            Debug.LogError("⚠️ AttackHitbox is missing. Assign it in the Inspector!");
            return;
        }

        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(attackHitbox.transform.position, range, PlayerLayer);

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

    // Update is called once per frame

    private void OnDrawGizmosSelected()
    {
        if (attackHitbox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackHitbox.transform.position, DemoRange);
        }
    }
}
