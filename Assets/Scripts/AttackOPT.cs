using UnityEngine;

[System.Serializable]
public struct AttackOptionP
{
    public string name;
    public KeyCode button;
    public string TriggerKey;
    public float AttackRange;
    public int Damage;
}
[System.Serializable]
public class AttackOptionE
{
    public string name;
    public string TriggerKey;
    public float AttackRange;
    public int Damage;
    public bool Attack;
    public float attackCooldown;
}
