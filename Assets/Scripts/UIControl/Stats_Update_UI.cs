using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Stats_Update_UI : MonoBehaviour
{
    public GameObject player;
    public PlayerStats stats;
    public List<TMP_InputField> inputFields;
    public TMP_InputField Health;

    private PlayerHealth PHealth;

    void Awake()
    {
        player.TryGetComponent<PlayerHealth>(out PHealth);
        if (inputFields != null) 
        { 
            for (int i=0 ;i < inputFields.Count;i++)
                {
                foreach (var stat in stats.stats)
                    if (stat.name == inputFields[i].name)
                        inputFields[i].text = $"{stat.value}";
                }
        }
    }

 
    public void UpdateStat() 
    {
        if (inputFields != null)
        {
            for (int i = 0; i < inputFields.Count; i++)
            {
                foreach (var stat in stats.stats)
                    if (stat.name == inputFields[i].name)
                        inputFields[i].text = $"{stat.value}";
            }
        }
        float currentHealth = PHealth.GetCurrentHealth();
        if (Health != null) { Health.text = $"{currentHealth} / {stats.maxHealth}"; }
    }

}
