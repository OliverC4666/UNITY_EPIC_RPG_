using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class Stats_Update_UI : MonoBehaviour
{
    public GameObject player;
    public PlayerStats stats;
    public List<TMP_InputField> inputFields;
    public TMP_InputField Health;

    private PlayerHealth PHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   if (inputFields != null) 
        { 
            for (int i=0 ;i < inputFields.Count;i++)
                {
                foreach (var stat in stats.stats)
                    if (stat.name == inputFields[i].name)
                        inputFields[i].text = $"{stat.value}";
                }
        }
    }

    // Update is called once per frame
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
        player.TryGetComponent<PlayerHealth>(out PHealth);
        float currentHealth = PHealth.GetCurrentHealth();
        if (Health != null) { Health.text = $"{currentHealth} / {stats.maxHealth}"; }
    }

}
