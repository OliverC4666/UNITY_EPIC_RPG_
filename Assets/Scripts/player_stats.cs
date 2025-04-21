using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{

    public List<Stats> stats = new() ;
    public float maxHealth = 100;

    public void IncreaseStat(string name,float amount)
    {
        Stats stat = Search(name);

        if (stats != null) 
            Search(name).value += amount;
        else 
        Debug.Log($"Stat: {name} not found in {this}, Check the name!");
    }

    public void ResetAllStats()
    {
        foreach (var stat in stats) { stat.value = 0; }
    }
    private Stats Search(string name) 
    {
        foreach(var stat in stats) 
        {
            if (stat.name == name)
                return stat;
        }
        
        return null;
    }
    public float Get(string name)
    {
        if (!string.IsNullOrEmpty(name))
            return Search(name)!=null? Search(name).value:0;
        return 0;
    }
    

}