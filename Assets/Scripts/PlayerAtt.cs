using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerAttack : MonoBehaviour
{
    public List<AttackOptionP> AttackConf = new(); // Configurable attacks
    public GameObject attackHitbox;  // The attack hitbox GameObject
    public LayerMask EnemyLayer;     // Defines what counts as an enemy
    public float trackingDuration = 0.5f; // Time window to track combos

    private int counterClick = 0;
    private Animator anim;
    private bool isTracking = false; // Flag for combo tracking

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (attackHitbox == null)
        {
            Debug.LogError("⚠️ AttackHitbox is not assigned to PlayerAttack script!");
        }
    }

    void Update()
    {
        foreach (var config in AttackConf)
        {
            if (Input.GetKeyDown(config.button)) // Check for key press
            {
                counterClick++;
                anim.SetInteger("ClickCounter", counterClick);
                anim.SetTrigger(config.TriggerKey); // Trigger attack animation
                Attack(config.AttackRange, config.Damage); // Use attack settings

                if (!isTracking)
                {
                    StartCoroutine(ResetCounter());
                }
            }
        }
    }

    public void Attack(float range, int attackDamage)
    {
        if (attackHitbox == null)
        {
            Debug.LogError("⚠️ AttackHitbox is missing. Assign it in the Inspector!");
            return;
        }

        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(attackHitbox.transform.position, range, EnemyLayer);

        foreach (Collider2D target in targetsHit)
        {
            if (target.TryGetComponent<EnemyHealth>(out var enemy))
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
        counterClick = 0; // Reset counter after tracking duration
        anim.SetInteger("ClickCounter", counterClick); // Reset animator parameter
        isTracking = false;
    }

    // Debugging: Draws the attack range in Scene View
    private void OnDrawGizmosSelected()
    {
        if (attackHitbox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackHitbox.transform.position, 1f);
        }
    }
}