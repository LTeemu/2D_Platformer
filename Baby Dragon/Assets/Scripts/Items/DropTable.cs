using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Drop
{
    public Pickup item;
    public int dropChance;
}

[CreateAssetMenu]
public class DropTable : ScriptableObject
{
    public Drop[] drops;

    public Pickup dropItem()
    {
        int r = Random.Range(0, 100);
        for(int i = 0; i < drops.Length; i++)
        {
            if(drops[i].dropChance >= r)
            {
                return drops[i].item;
            }           
        }
        return null;
    }
}
