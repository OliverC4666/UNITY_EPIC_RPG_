using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator anim;
    public Slider Healthbar;
    public int maxHealth = 100;
    public int health = 100;

    void Start()
    {
        anim = GetComponent<Animator>();
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
            StartCoroutine(Die()); // Use a coroutine instead of async
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

    IEnumerator Die()
    {
        if (anim != null)
        {
            anim.SetBool("died", true);

            // Wait for the animation to finish
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            yield return new WaitForSeconds(1.5f); // Fallback if no animator
        }

        Destroy(gameObject);
    }
}
