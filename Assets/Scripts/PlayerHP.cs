using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator anim;
    public Slider Healthbar;
    public int maxHealth = 100;
    public int health = 100;

    private Rigidbody2D rb;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (Healthbar != null)
        {
            Healthbar.value = (float)health / maxHealth; // Initialize correctly
        }
        else
        {
            Debug.LogWarning("Healthbar reference is missing!", this);
        }
    }
    private void Update()
    {
        if (gameObject == null) Debug.Log("object terminated");
        if (Healthbar != null)
        {
            Healthbar.value = (float)health / maxHealth;
        }
        else
        {
            Debug.LogWarning("Healthbar is null!", this);
        }

        if (health <= 0)
        {
            Destroy(rb);
            anim.SetBool("died", true); 
        }
    }
    public void TakeDamage(int damage)
    {
        if (health <= 0) return; // Already dead

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Prevent negative values
        if (anim != null)
        {
            anim.SetTrigger("IsAttacked");
        }
        else
        {
            Debug.LogWarning("Animator reference is missing!", this);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
