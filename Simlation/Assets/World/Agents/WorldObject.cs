using UnityEngine;
using Utility;

namespace World.Agents
{
    public abstract class WorldObject: MonoBehaviour, ILog
    {
        private Environment.WorldController position;
        public abstract string LN();
    }
}