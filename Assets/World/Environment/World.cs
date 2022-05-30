using System.Collections.Generic;
using UnityEngine;
using World.Agents;

namespace World.Environment
{
    public class World : MonoBehaviour
    {
        public List<Agent> AgentList;
        public Vector2Int size;

        private void Start()
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