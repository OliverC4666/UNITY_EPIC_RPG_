using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Animator anim;
    public Slider Healthbar;
    public PlayerStats enemyStats; // 📦 Reference to ScriptableObject

    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (Healthbar != null)
        {
            Healthbar.value = enemyStats.currentHealth = enemyStats.maxHealth;
        }
        else
        {
            Debug.LogWarning("Healthbar reference is missing!", this);
        }
    }

    void Update()
    {
        if (!isDead)
        {
            if (Healthbar != null)
            {
                Healthbar.value = (float)enemyStats.currentHealth / enemyStats.maxHealth;
            }

            if (enemyStats.currentHealth <= 0)
            {
                StartDeathSequence();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        enemyStats.currentHealth -= damage;
        enemyStats.currentHealth = Mathf.Clamp(enemyStats.currentHealth, 0, enemyStats.maxHealth);

        if (anim != null)
        {
            anim.SetTrigger("IsAttacked");
        }
        else
        {
            Debug.LogWarning("Animator reference is missing!", this);
        }
    }

    async void StartDeathSequence()
    {
        if (isDead) return;
        isDead = true;

        if (anim != null)
        {
            anim.SetBool("died", true);
        }

        DisableAllScripts();
        await Task.Delay(900);
        Destroy(gameObject);
    }

    void DisableAllScripts()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }
    }
}
