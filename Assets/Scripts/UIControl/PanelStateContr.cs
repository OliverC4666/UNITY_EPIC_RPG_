using System.Collections.Generic;
using UnityEngine;

public class PanelStateContr : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape) )
        {
            gameObject.SetActive(false);
        }
    }
    public void MenuToggle(bool state)
    {
        gameObject.SetActive(state);
    }

}
