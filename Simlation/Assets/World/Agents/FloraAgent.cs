using System;
using World.Structure;

namespace World.Agents
{
    public abstract class FloraAgent: Agent
    {
        public readonly Ground ground;
        
        public float waterConsumption = 1;
        public Ground.GroundTypes preferredGround = Ground.GroundTypes.Silt;
        public float heatResistance = 1;
        public float dryResistance = 1;
        public bool xerophyt = false;

        protected FloraAgent(Ground ground)
        {
            this.ground = ground;
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