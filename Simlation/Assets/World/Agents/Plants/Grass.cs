using System;
using System.Collections;
using UnityEngine;
using World.Agents;
using World.Structure;
using Random = UnityEngine.Random;

public class Grass : FloraAgent
{
    public Grass(Ground ground) : base(ground)
    {
        
    }

    public override string LN()
    {
        return "Grass";
    }
}
