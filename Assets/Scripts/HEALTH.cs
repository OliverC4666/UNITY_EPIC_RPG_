using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Animator anim;
    public Slider Healthbar;
    public int maxHealth = 100;
    public int health = 100;
    private bool isDead = false; // Prevent multiple death triggers

    void Start()
    {
        anim = GetComponent<Animator>();

        if (Healthbar != null)
        {
            Healthbar.value = (float)health / maxHealth;
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
                Healthbar.value = (float)health / maxHealth;
            }

            if (health <= 0)
            {
                StartDeathSequence();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Ignore if already dead

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

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
        if (isDead) return; // Prevent multiple executions
        isDead = true;

        if (anim != null)
        {
            anim.SetBool("died", true);
        }

        DisableAllScripts();

        await Task.Delay(900); // Wait before destruction
        Destroy(gameObject);
    }

    void DisableAllScripts()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // Keep this script active for the death animation
            {
                script.enabled = false;
            }
        }
    }
}
