using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    public PanelStateContr contr;
    static public List<Toggle> toggles = new();
    private ToggleGroup toggleGroup;


    private void Awake()
    {

        toggleGroup = GetComponent<ToggleGroup>();


        foreach (Transform child in transform)
        {
            GameObject go = child.gameObject;

            Toggle toggle = go.GetComponent<Toggle>();

            if (toggle != null)
            {
                toggle.group = toggleGroup;
                toggles.Add(toggle);
            }

        }
    }
    private void Start()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i; // capture loop variable
            toggles[i].onValueChanged.AddListener((isOn) => {
                if (isOn) Activate(index);
            });
        }
    }
    public void Activate(int ID)
    {
        if (ID < 0 || ID >= toggles.Count)
        {
            Debug.LogWarning("Invalid toggle ID: " + ID);
            return;
        }

        contr.MenuToggle(toggleGroup.AnyTogglesOn());
        GameEvents.instance.onAnyToggle(ID, toggles[ID].isOn);
        
    }
}
