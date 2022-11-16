using System;
using World.Agents;
using World.Structure;

public class Oak : TreeAgent
{
    public Oak(Ground ground) : base(ground)
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

    public override string LN()
    {
        return "Oak";
    }
}
