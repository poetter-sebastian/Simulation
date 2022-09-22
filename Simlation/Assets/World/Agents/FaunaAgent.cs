using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using World.Agents.Animals;

namespace World.Agents
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class FaunaAgent: Agent
    {
        public NavMeshAgent nav;
        public Transform target;
        public HearBehaviour hearSense;
        public SeeBehaviour seeSense;
        public SmellBehaviour smellSense;
        public BodyBehaviour bodySense;

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
        public float maxPossibleHealth;
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
            
            //hearSense.OnSense += OnHear;
            //hearSense.OnRadiusSense += OnHearRadius;
            //hearSense.OnRadiusExitSense += OnHearRadiusExit;
            
            //seeSense.OnSense += OnSee;
            //seeSense.OnRadiusSense += OnSeeRadius;
            //seeSense.OnRadiusExitSense += OnSeeRadiusExit;
            
            smellSense.OnSense += OnSmell;
            smellSense.OnRadiusSense += OnSmellRadius;
            //smellSense.OnRadiusExitSense += OnSmellRadiusExit;
            
            bodySense.OnSense += OnFoodFood;
        }

        protected abstract void OnSee(GameObject obj);
        protected abstract void OnSeeRadius(GameObject obj);
        protected abstract void OnSeeRadiusExit(GameObject obj);
        protected abstract void OnHear(GameObject obj);
        protected abstract void OnHearRadius(GameObject obj);
        protected abstract void OnHearRadiusExit(GameObject obj);
        protected abstract void OnSmell(GameObject obj);
        protected abstract void OnSmellRadius(GameObject obj);
        protected abstract void OnSmellRadiusExit(GameObject obj);
        protected abstract void OnFoodFood(GameObject obj);
        
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
        
        public override void OnDamage(object s, GenEventArgs<int> e)
        {
            throw new NotImplementedException();
        }
    }
}