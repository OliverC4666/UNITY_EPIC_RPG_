using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Animator anim;
    public PlayerStats playerStats;  // ScriptableObject reference

    private Rigidbody2D rb;
    private Slider Healthbar;
    private bool damaged = false;
    private float currentHealth;
    private float timeSinceDamaged = 0f;
    private float healDelay = 10f;
    private float healRate = 1f; // Amount to heal per second
    

    void Start()
    {
        currentHealth = playerStats.maxHealth;

        GameEvents.instance.OnPlayerDestroyed += Die;

        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        do
        {
            GameObject found = GameObject.Find(gameObject.name + " Health");
            Healthbar = found == null ? null : found.GetComponent<Slider>();

        }
        while (Healthbar == null);


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
            Healthbar.value = (float)currentHealth / playerStats.maxHealth;
        }

        // Death check
        if (playerStats != null && currentHealth <= 0)
        {
            if (rb != null) Destroy(rb);
            if (anim != null) anim.SetBool("died", true);
            GameEvents.instance.GameOver();
            return; // stop all healing logic
        }

        // Heal after delay if not recently damaged
        if (!damaged)
        {
            timeSinceDamaged += Time.deltaTime;

            if (timeSinceDamaged >= healDelay && currentHealth < playerStats.maxHealth)
            {
                currentHealth += Mathf.CeilToInt(healRate * Time.deltaTime);
                currentHealth = Mathf.Clamp(currentHealth, 0, playerStats.maxHealth);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <=0) return; // already dead
        StartCoroutine(Damaged(damage));
    }

    IEnumerator Damaged(int damage)
    {
        damaged = true;
        timeSinceDamaged = 0f;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, playerStats.maxHealth);

        if (anim != null)
            anim.SetTrigger("IsAttacked");
        else
            Debug.LogWarning("Animator is missing!", this);

        yield return new WaitForSeconds(0.2f); // Optional: invincibility window
        damaged = false;
    }

    public void Die()
    {
        GameEvents.instance.OnPlayerDestroyed -= Die;
        Destroy(Healthbar.gameObject);
        Destroy(gameObject);
        
    }
    public float GetCurrentHealth() { return currentHealth; }
}
