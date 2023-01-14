using System;
using System.Collections.Generic;
using Game.Utility;
using UnityEngine;
using Utility;
using World.Agents.Modifier;
using World.Environment;
using World.Structure;

namespace World.Agents
{
    public abstract class Agent : WorldObject
    {
        /// <summary>
        /// If the Agent can move
        /// </summary>
        [Tooltip("Movable")]
        public bool moveAble;
        /// <summary>
        /// If the Agent is alive
        /// </summary>
        [Tooltip("Is alive")]
        public bool alive = true;

        [Tooltip("Domain of agent")]
        public string domain = "Unknown";
        [Tooltip("Biological kingdom of the agent")]
        public string kingdom = "Unknown";
        [Tooltip("Biological phylum of the agent")]
        public string phylum = "Unknown";
        [Tooltip("Biological class of the agent")]
        public string agentClass = "Unknown";
        [Tooltip("Biological order of the agent")]
        public string order = "Unknown";
        [Tooltip("Biological family of the agent")]
        public string family = "Unknown";
        [Tooltip("Biological genus of the agent")]
        public string genus = "Unknown";
        [Tooltip("Species of the agent")]
        public string species = "Unknown";

        /// <summary>
        /// Oxygen modifier value of the agent
        /// </summary>
        [Tooltip("Oxygen of agent")]
        public float o2Modifier;
        /// <summary>
        /// Carbon dioxide modifier value of the agent
        /// </summary>
        [Tooltip("Carbon dioxide modifier of agent")]
        public float co2Modifier;
        /// <summary>
        /// Body size value of the agent
        /// </summary>
        [Tooltip("Body size of agent in meters")]
        public float size;
        /// <summary>
        /// Age of the agent
        /// </summary>
        [Tooltip("Age of agent in years")]
        public float age;
        /// <summary>
        /// Weight of the agent in kg
        /// </summary>
        [Tooltip("Weight of agent in kg")]
        public float weight;

        /// <summary>
        /// Do stuff on the day and sleeps in the night if its true
        /// If its false the agent do stuff in the night and sleeps on day.
        /// </summary>
        [Tooltip("Day active agent")]
        public bool diurnal = true;

        /// <summary>
        /// Life of the agent. If its 0 the agent is dead.
        /// </summary>
        [Tooltip("Healthy of agent")]
        public int health = 0;
        /// <summary>
        /// Thirst value of the agent
        /// </summary>
        [Tooltip("Thirsty of agent")]
        public int thirst = 0;
        /// <summary>
        /// List of injuries modifiers of the agent
        /// </summary>
        [Tooltip("List of injuries of agent")]
        public List<Injury> injuries = new();
        /// <summary>
        /// List of diseases modifiers of the agent
        /// </summary>
        [Tooltip("List of diseases of agent")]
        public List<Disease> diseases = new();

        /// <summary>
        /// Behaviour of the agent
        /// </summary>
        [Tooltip("Behaviour of the agent")]
        public IAgentBehaviour behaviour;
        
        /// <summary>
        /// Event gets fired when the agents loses health
        /// </summary>
        /// <param name="s">Sender of the event</param>
        /// <param name="e">Parameter of the event</param>
        public abstract void OnDamage(object s,  GenEventArgs<int> e);

        /// <summary>
        /// Event gets fired when the agents consumes
        /// </summary>
        /// <param name="s">Sender of the event</param>
        /// <param name="e">Parameter of the event</param>
        public abstract void OnConsumption(object s, EventArgs e);

        /// <summary>
        ///  Function to handle the agent and check the states
        /// </summary>
        public abstract void OnHandle(WorldController world);
        
        /// <summary>
        /// Event gets fired when the agents has no health
        /// </summary>
        /// <param name="s">Sender of the event</param>
        /// <param name="e">Parameter of the event</param>
        public void OnDeath(object s, EventArgs e)
        {
            try
            {
                world.RemoveAgent(this);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        /// Event gets fired when the agents is died
        /// </summary>
        /// <param name="s">Sender of the event</param>
        /// <param name="e">Parameter of the event</param>
        public void OnAfterDeath(object s, EventArgs e)
        {
            gameObject.SetActive(false);
        }
    }

    class AgentNotFoundException : MissingComponentException
    {
        // ReSharper disable once InconsistentNaming
        public new string Message = "The selected object has no agent component";
    }
}
