using System;
using World.Agents;
using World.Structure;
using Object = UnityEngine.Object;

public class Pine : FloraAgent
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
    }
    
    private void Start()
    {
        
    }

    public override void OnDeath(object s, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public override void OnConsumption(object s, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public override void OnHandle()
    {
        OnConsumption(this, EventArgs.Empty);
    }
}
