using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator anim;
    private Slider Healthbar;
    public PlayerStats Entity; // 📦 Reference to ScriptableObject

    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = Entity.maxHealth;
        do
        {
            GameObject found = GameObject.Find(gameObject.name + " Health");
            Healthbar = found == null ? null : found.GetComponent<Slider>();

        }
        while (Healthbar == null);

        if (Healthbar != null)
        {
            Healthbar.value = currentHealth = Entity.maxHealth;
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
                Healthbar.value = (float)currentHealth / Entity.maxHealth;
            }

            if (currentHealth <= 0)
            {
                StartDeathSequence();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, Entity.maxHealth);

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
        Destroy(Healthbar.gameObject);
    }
}
