using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public double Constructed;
    public int WoodRequiredForConstruction;
    public Material Material;
    
    public bool Construct(int woodAmount)
    {
        //Building is already constructed
        if (Constructed == 1)
            return false;

        double percentage = (double)woodAmount / (double)WoodRequiredForConstruction;
        Constructed += percentage;
        Constructed = Constructed > 1 ? 1 : Constructed;
        return true;
    }

    protected void Init()
    {
        GetComponent<MeshRenderer>().material = Material;
    }
}
