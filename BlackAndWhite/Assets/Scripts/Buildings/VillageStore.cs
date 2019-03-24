using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageStore : Building
{
    public int Food = 1000;
    public int Wood = 1000;

    public bool CollectWood(int amount)
    {
        if(Wood >= amount)
        {
            Wood -= amount;
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
