using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using World.Agents.Animals;
using World.Structure;

namespace World.Agents
{
    public abstract class TreeAgent: FloraAgent
    {
        protected TreeAgent(Ground ground) : base(ground) { }

        public void CutTree()
        {
            OnDamage(this, new GenEventArgs<int>(-health*10));
        }
    }
}