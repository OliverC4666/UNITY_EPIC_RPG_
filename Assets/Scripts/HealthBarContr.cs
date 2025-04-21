using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarContr : MonoBehaviour
{
    public TagHandle Tag;
    public GameObject HealthBarModel;
    public int verticalOffset;
    private GameObject[] Entities;
    private GameObject[] Sliders;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Entities = GameObject.FindGameObjectsWithTag("Entity");
        Sliders = new GameObject[Entities.Length];
        int i = 0;
        foreach (GameObject entity in Entities)
        {
            if (i < Entities.Length)
            {
                if (entity == null)
                {
                    i++; continue;
                }
                Sliders[i] = Instantiate(HealthBarModel, gameObject.transform, true);
                Sliders[i].name = entity.name + " Health";
                Sliders[i].SetActive(true);
                i++;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Entities.Length; i++)
        {
            if (Entities[i] != null && Sliders[i] != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(
                    Entities[i].transform.position + new Vector3(0, verticalOffset, 0) // Vertical offset
                );
                Sliders[i].transform.position = screenPos;
            }
        }
    }



}
