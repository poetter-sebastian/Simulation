using System;
using Utility;
using World.Structure;

namespace World.Agents
{
    public abstract class FloraAgent: Agent
    {
        public Ground ground;
        
        public float waterConsumption = 1;
        public Ground.GroundTypes preferredGround = Ground.GroundTypes.Silt;
        public float heatResistance = 1;
        public float dryResistance = 1;
        public bool xerophyt = false;

        protected FloraAgent(Ground ground)
        {
            this.ground = ground;
        }

        public override void OnHandle()
        {
            OnConsumption(this, EventArgs.Empty);
        }
        
        public override void OnConsumption(object sender, EventArgs e)
        {
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
                OnDeath(this, EventArgs.Empty);
            }
        }
        
        public override void MouseClick()
        {
            throw new NotImplementedException();
        }

        public override void MouseOver()
        {
            throw new NotImplementedException();
        }

        public override void MouseExit()
        {
            throw new NotImplementedException();
        }
    }
}