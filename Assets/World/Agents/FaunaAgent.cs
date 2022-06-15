using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace World.Agents
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class FaunaAgent: Agent
    {
        public NavMeshAgent nav;
        public Transform target;
        public CapsuleCollider hearRange;
        public CapsuleCollider lookRange;
        public CapsuleCollider smellRange;

        /// <summary>
        /// Hunger of the agent
        /// </summary>
        [Tooltip("Hunger value of the agent")]
        public int hunger;
        /// <summary>
        /// Max normal distribution value of children (0-x possible children)
        /// </summary>
        [Tooltip("How many offsprings are possible")]
        public int offspring = 1;
        /// <summary>
        /// How long the pregnancy takes
        /// </summary>
        [Tooltip("Time of pregnancy")]
        public float gestationDuration;
        /// <summary>
        /// How long the agent need to mature
        /// </summary>
        [Tooltip("Time to mature")]
        public float timeToMature;
        /// <summary>
        /// Methane modifier value of the agent
        /// </summary>
        [Tooltip("Methane modifier of agent")]
        public float ch4Modifier;
        /// <summary>
        /// Body weight value of the agent in m
        /// </summary>
        [Tooltip("Body weight of agent")]
        public float weight;
        /// <summary>
        /// Body temperature value of the agent in °C
        /// </summary>
        [Tooltip("Body temperature of agent")]
        public float temperature = 24.5f;
        public float maxPossibleSpeed;
        public float smellRadius = 1;
        public float listingRadius = 1;
        public float visionRadius = 1;
        public bool cannibalism = false;

        protected FaunaAgent()
        {
            moveAble = true;
        }
        
        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            nav.speed = maxPossibleSpeed;
        }

        private void Update()
        {
            //nav.destination = target.position;
        }
    }
}