using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PowerUpFlower : MonoBehaviour
{

    public float powerIncrease = 10f;

    public GameObject promptUI; // UI Text object for "Press E"
    public Transform playerHead;

    private bool playerInRange = false;

    public TextMeshProUGUI tmp;
    void Start()
    {

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerStats stats = Resources.Load<PlayerStats>("PlayerStats");
            if (stats != null)
            {
                new WaitForSeconds(3);
                stats.IncreasePower(powerIncrease);
                Debug.Log("Power increased by " + powerIncrease);

                StartCoroutine(ShowFloatingText("+" + powerIncrease.ToString() + " Power"));

                

                
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    IEnumerator ShowFloatingText(string message)
    {


            if (tmp != null)
            {
                tmp.text = message;
            }

            // Position it above the player's head in screen space
            Vector3 screenPos = Camera.main.WorldToScreenPoint(playerHead.position + Vector3.up * 1.5f);


            
            if (promptUI != null)
                promptUI.SetActive(false);
            yield return new WaitForSeconds(1.5f);

            Destroy(tmp); 


            Destroy(gameObject);// Clean up after 1.5s
        
    }
}
