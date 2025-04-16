using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator anim;
    public Slider Healthbar;
    public PlayerStats playerStats;  // ScriptableObject reference

    private bool damaged = false;
    private float timeSinceDamaged = 0f;
    private float healDelay = 5f;
    private float healRate = 1f; // Amount to heal per second
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (Healthbar == null)
            Debug.LogWarning("Healthbar reference is missing!", this);

        if (playerStats == null)
            Debug.LogError("PlayerStats ScriptableObject not assigned!", this);
    }

    void Update()
    {
        // Handle healthbar UI update
        if (Healthbar != null && playerStats != null)
        {
            Healthbar.value = (float)playerStats.currentHealth / playerStats.maxHealth;
        }

        // Death check
        if (playerStats != null && playerStats.currentHealth <= 0)
        {
            if (rb != null) Destroy(rb);
            if (anim != null) anim.SetBool("died", true);
            return; // stop all healing logic
        }

        // Heal after delay if not recently damaged
        if (!damaged)
        {
            timeSinceDamaged += Time.deltaTime;

            if (timeSinceDamaged >= healDelay && playerStats.currentHealth < playerStats.maxHealth)
            {
                playerStats.currentHealth += Mathf.CeilToInt(healRate * Time.deltaTime);
                playerStats.currentHealth = Mathf.Clamp(playerStats.currentHealth, 0, playerStats.maxHealth);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (playerStats.currentHealth <= 0) return; // already dead
        StartCoroutine(Damaged(damage));
    }

    IEnumerator Damaged(int damage)
    {
        damaged = true;
        timeSinceDamaged = 0f;

        playerStats.currentHealth -= damage;
        playerStats.currentHealth = Mathf.Clamp(playerStats.currentHealth, 0, playerStats.maxHealth);

        if (anim != null)
            anim.SetTrigger("IsAttacked");
        else
            Debug.LogWarning("Animator is missing!", this);

        yield return new WaitForSeconds(0.2f); // Optional: invincibility window
        damaged = false;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
