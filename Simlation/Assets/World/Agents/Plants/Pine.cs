using System;
using World.Agents;
using World.Structure;

public class Pine : TreeAgent
{
    public Pine(Ground ground) : base(ground)
    {
        domain = "Eukarya";
        kingdom = "Animalia";
        phylum = "Chordata";
        agentClass = "";
        order = "";
        family = "";
        genus = "";
        species = "";
        health = 200;
        waterConsumption = 10;
    }
    
    private void Start()
    {
        
    }

    public override string LN()
    {
        return "Pine";
    }
}
