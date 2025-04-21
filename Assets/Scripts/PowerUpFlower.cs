using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(PowerUpFlower))]
public class PowerUpFlowerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PowerUpFlower flower = (PowerUpFlower)target;
        if (GUILayout.Button("Reload Stats from PlayerStats"))
        {
            flower.PlayerStatsToIncrease.Clear();
            foreach (var stat in flower.stats.stats)
            {
                flower.PlayerStatsToIncrease.Add(stat.Clone());
            }
            EditorUtility.SetDirty(flower);
        }
    }
}
#endif
public class PowerUpFlower : MonoBehaviour
{



    public GameObject promptUI; // UI Text object for "Press E"
    public TextMeshProUGUI noticeMess;

    public PlayerStats stats;

    public List<Stats> PlayerStatsToIncrease;

    private bool playerInRange = false;


    void OnValidate()
    {
        if (PlayerStatsToIncrease == null || stats == null) return;

        // Only clone once if list is empty
        if (PlayerStatsToIncrease.Count == 0)
        {
            foreach (var stat in stats.stats)
            {
                PlayerStatsToIncrease.Add(stat.Clone());
            }

            // Mark dirty to make Unity know to save the change
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
        #endif
        }
    }
    void Start()
    {

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ApplyPowerUps());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    IEnumerator ShowFloatingText(string message,float offset)
    {


            if (noticeMess != null)
            {
                noticeMess.text = message;
            }

            // Position it above the player's head in screen space
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * offset);

            
            if (promptUI != null)
                promptUI.SetActive(false);
            yield return new WaitForSeconds(1.5f);

        noticeMess.text = "";
    }
    private bool used = false;

    IEnumerator ApplyPowerUps()
    {
        if (used) yield break; // prevent double use
        used = true;

        int i = 0;
        foreach (var stat in PlayerStatsToIncrease)
        {
            stats.IncreaseStat(stat.name, stat.value);
            Debug.Log($"{stat.name} increased by {stat.value}");
            StartCoroutine(ShowFloatingText($"+ {stat.value}% {stat.name}", 1.5f + 0.15f * i));
            i++;
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }


}
