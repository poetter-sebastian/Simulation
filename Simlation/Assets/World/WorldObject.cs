using Game.Utility;
using UnityEngine;
using Utility;
using World.Environment;
using World.Structure;

namespace World.Agents
{
    public abstract class WorldObject: MonoBehaviour, ILog, IMouseListener
    {
        public Ground ground;
        public WorldController world;
        
        public abstract string LN();
        
        public abstract void MouseClick();
        public abstract void MouseOver();
        public abstract void MouseExit();
    }
}