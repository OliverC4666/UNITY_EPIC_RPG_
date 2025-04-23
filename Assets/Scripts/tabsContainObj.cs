using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

public class tabsContainObj : MonoBehaviour
{

    public int Current_ID;
    private List<GameObject> childObjects = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in transform) childObjects.Add(child.gameObject);
        foreach (var obj in childObjects)
            {
             obj.SetActive(false);
            }
        GameEvents.instance.OnAnyToggle += setRunningTab;
    }


    public void setRunningTab(int ID,bool active)
    {
        gameObject.SetActive(true);
        if (ID < 0 || ID >= childObjects.Count) return;

        for (int i = 0; i < childObjects.Count; i++)
        {
            childObjects[i].SetActive(i == ID&&active);
        }
        //Debug.Log($"ID: {ID}, childObjects.Count: {childObjects.Count}");
        Current_ID = ID;
    }



}
