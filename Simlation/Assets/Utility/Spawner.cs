using System;
using UnityEngine;
using World.Agents;
using Random = UnityEngine.Random;

namespace World.Environment
{
    /// <summary>
    /// Interface for plantSpawner to 
    /// </summary>
    public abstract class Spawner: MonoBehaviour
    {
        public WorldController world;
        public int spawnAttempts = 500;
        public float minHeight = 3.5f;
        public float maxHeight = 7.5f;
        public GameObject[] objs;

        public abstract void SpawnOptions(GameObject newPrefab, RaycastHit hit);

        public void Spawn(WorldController world)
        {
            this.world = world;
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
            for (var i = 0; i < spawnAttempts; i++)
            {
                //1 and -1 because of the map border
                var x = Random.Range(1f, world.size.x-1f);
                var z = Random.Range(1f, world.size.y-1f);

                var ray = new Ray(new Vector3(x, 50, z), new Vector3(x, 50-200, z));

                Physics.Raycast(ray, out var hit, 200, LayerMask.GetMask("World", "Water"));

                if (hit.point == Vector3.zero || hit.transform.gameObject.layer == 4)
                {
                    continue;
                }
                
                if (hit.point.y < minHeight || hit.point.y > maxHeight) continue;
                
                var obj = Random.Range(0, objs.Length);
                SpawnOptions(objs[obj], hit);
            }
        }

        protected void RegisterFloraAgent(FloraAgent agent)
        {
            if (agent == null)
            {
                throw new AgentNotFoundException();
            }
            world.RegisterFloraAgent(agent);
        }

        public class NoSpawnerException : Exception
        {
            // ReSharper disable once InconsistentNaming
            public new string Message = "The selected object is not a spawner.";
        }
    }
}
