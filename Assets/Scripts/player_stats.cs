using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float power = 0;
    public float speed = 5f;

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void IncreasePower(float amount)
    {
        power += amount;
    }

    public void ResetStats()
    {
        currentHealth = maxHealth;
        power = 0;
    }
}