using UnityEngine;

public class RoofLayer : MonoBehaviour
{
    public string playerTag = "Player"; // Make sure your player is tagged as "Player"
    public SpriteRenderer roofSR;
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

                roofSR.sortingLayerName = "ForeG"; 

            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

                roofSR.sortingLayerName = "Default"; 

            
        }
    }
}
