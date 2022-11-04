using UnityEngine;
using World.Agents;

namespace World.Environment.Spawn
{
    public class GrassSpawner : Spawner
    {
        public GrassSpawner()
        {
            spawnAttempts = 3000;
            minHeight = 2.5f;
            maxHeight = 10f;
        }

        public override void SpawnOptions(GameObject newPrefab, RaycastHit hit)
        {
            var plant = Instantiate(newPrefab, hit.point, new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f), transform);
            RegisterFloraAgent(plant.GetComponent<FloraAgent>());
        }
    }
}
