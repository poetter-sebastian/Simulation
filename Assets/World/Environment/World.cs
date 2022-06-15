using System;
using System.Collections.Generic;
using UnityEngine;
using World.Agents;
using World.Structure;

namespace World.Environment
{
    public class World : MonoBehaviour
    {
        public List<Agent> AgentList;
        public Vector2Int size;

        public Graph Graph = new Graph();

        public World()
        {
            Graph.AddNode(Vector3.zero);
            Graph.AddNode(Vector3.up);
        }

        private void OnDrawGizmos()
        {
            
        }

        // Update is called once per frame
        private void Update()
        {
            HandleAgents();
        }

        private void HandleAgents()
        {
            foreach (var agent in AgentList)
            {
                agent.Handle();
            }
        }
    }
}