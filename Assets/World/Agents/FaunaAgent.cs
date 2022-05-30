using System;
using UnityEngine;
using UnityEngine.AI;

namespace World.Agents
{
    public class FaunaAgent: Agent
    {
        public NavMeshAgent nav;
        public Transform target;

        private FaunaAgent()
        {
            moveAble = true;
        }
        
        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            nav.destination = target.position;
        }
    }
}