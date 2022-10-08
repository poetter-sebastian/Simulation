using System;
using World.Agents;
using World.Structure;

public class Cypress : FloraAgent
{
    public Cypress(Ground ground) : base(ground)
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
        return "Cypress";
    }
}
