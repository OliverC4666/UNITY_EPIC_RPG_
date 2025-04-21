using UnityEngine;

[System.Serializable]
public struct AttackOptionP
{
    public string name;
    public KeyCode button;
    public string TriggerKey;
    public float AttackRange;
    public int baseDamage;
}
[System.Serializable]
public struct AttackOptionE
{
    public string name;
    public string TriggerKey;
    public float AttackRange;
    public int baseDamage;
    public bool Attack;
    public float attackCooldown;
}

[System.Serializable]
public class Stats
{
    public string name;
    public float value;
    public void Stat(string name, float value)
    {
        this.name = name;
        this.value = value;
    }
    public float GetStat()
    {
        return value;
    }
    public Stats Clone()
    {
        return new Stats { name = this.name, value = this.value };
    }
}

