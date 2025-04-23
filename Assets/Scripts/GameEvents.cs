using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;


public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;
    void Awake()
    {
        instance = this;
    }
    public event Action<int,bool> OnAnyToggle;

    public void onAnyToggle(int ID,bool active) 
    {
        OnAnyToggle?.Invoke(ID,active);
    }
    [Space(5)]
    [Header("Game Over GUI")]
    public GameObject gameOverPanel;
    public event Action OnPlayerDestroyed;
    public void GameOver()
    {   
        if (OnPlayerDestroyed == null ) { return; }
        gameOverPanel.SetActive(true);
        OnPlayerDestroyed?.Invoke();
    }
    
}   
