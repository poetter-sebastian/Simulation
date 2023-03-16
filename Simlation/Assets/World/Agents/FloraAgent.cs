using System;
using System.Collections.Generic;
using Utility;
using World.Agents.Modifier;
using World.Environment;
using World.Structure;

namespace World.Agents
{
    public abstract class FloraAgent: Agent
    {
        public float waterConsumption = 1;
        public Ground.GroundTypes preferredGround = Ground.GroundTypes.Silt;
        public float heatResistance = 1;
        public float dryResistance = 1;
        public bool xerophyt = false;

        protected FloraAgent(Ground ground)
        {
            this.ground = ground;
            world = ground.RefWorld();
        }

        public override void OnHandle(WorldController world)
        {
            OnConsumption(this, EventArgs.Empty);
        }
        
        public override void OnConsumption(object sender, EventArgs e)
        {
            if (ground == null)
            {
                WorldController.Instance.RegisterFloraAgent(this);
                ILog.LE(LN, "Wrong registered plant!");
            }
            if (!ground.GetWater(-1 * waterConsumption))
            {
                OnDamage(this, new GenEventArgs<int>(-5));
            };
        }
        
        public override void OnDamage(object s, GenEventArgs<int> e)
        {
            health += e.Value;
            if (health <= 0)
            {
                //TODO try to define world on spawn!
                world = ground.RefWorld();
                OnDeath(this, EventArgs.Empty);
            }
        }
        
        public override void MouseClick()
        {

        }

        public override void MouseOver()
        {

        }

        public override void MouseExit()
        {

        }

        public void AddDisease(Disease d)
        {
            (diseases ?? new List<Disease>()).Add(d);
        }
    }
}