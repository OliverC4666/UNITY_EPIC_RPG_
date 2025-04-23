using Unity.VisualScripting;

using UnityEngine;

public class HouseEnter : MonoBehaviour
{
    public Transform teleportPoint; // Assign where the player will teleport inside the house
    private GameObject player;
    public bool at_door;
    public bool button_pressed;

    private void Update()
    {
        button_pressed = Input.GetKey(KeyCode.F);
        Debug.Log(button_pressed);
        if (button_pressed && at_door) player.transform.position = teleportPoint.position;

    }
    void OnTriggerEnter2D(Collider2D Player)
    {

        if (Player.CompareTag("Player"))// Press 'E' to enter
        {
            at_door = true;
            Debug.Log("Player near door");
            player = Player.gameObject;
        }


    }
    void OnTriggerExit2D(Collider2D Player)
    {

        if (Player.CompareTag("Player"))// Press 'E' to enter
        {
            at_door = false;
            Debug.Log("Player not at door");
        }

    }
}
