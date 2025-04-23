using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public tabsContainObj tabsContainObj;
    public ToggleScript ToggleScript;
    public Button Right;
    public Button Left;
    private int tabID;
    private int MaxCount;
    void Start()
    {
        MaxCount = ToggleScript.toggles.Count;
        tabID = tabsContainObj != null ? tabsContainObj.Current_ID : 0;
        Left.gameObject.SetActive(tabID == 0);
    }

    // Update is called once per frame
    void Update()
    {
        tabID = tabsContainObj.Current_ID;
        if (tabID == 0)
        { 
            Left.gameObject.SetActive(false);
        }
        else if (tabID >0 && !Left.gameObject.activeSelf) Left.gameObject.SetActive(true);
        if (tabID == MaxCount - 1) Right.gameObject.SetActive(false);
        else if (tabID < MaxCount - 1 && !Right.gameObject.activeSelf) Right.gameObject.SetActive(true);

    }
    public void SwitchID(int i) 
    {
        ToggleScript.toggles[tabID].isOn = !ToggleScript.toggles[tabID].isOn;
        ToggleScript.toggles[tabID+i].isOn = !ToggleScript.toggles[tabID + i].isOn;
        ToggleScript.Activate(tabID + i);
    }
}
