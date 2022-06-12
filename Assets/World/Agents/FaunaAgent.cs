using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace World.Agents
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CapsuleCollider))]
    public abstract class FaunaAgent: Agent
    {
        public NavMeshAgent nav;
        public Transform target;
        public CapsuleCollider listeningRange;
        public CapsuleCollider lookingRange;
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

        protected FaunaAgent()
        {
            moveAble = true;
            Debug.Log("FaunaAgent function fired");
        }
        
        private void Start()
        {
            nav = GetComponent<NavMeshAgent>();
        }
        
        private void Update()
        {
            nav.destination = target.position;
        }
    }
}