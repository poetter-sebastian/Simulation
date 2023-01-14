using System;

namespace World.Agents
{
    public abstract class AgentModifier
    {
        protected AgentModifier(string n)
        {
            name = n;
        }
        
        public readonly string name;

        public abstract void OnInit(Agent sender, EventArgs e);

        public abstract void OnCall(Agent sender, EventArgs e);

        public abstract void OnHeal(Agent sender, EventArgs e);
    }
}