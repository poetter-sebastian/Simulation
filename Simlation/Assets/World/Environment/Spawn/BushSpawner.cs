using UnityEngine;
using World.Agents;

namespace World.Environment.Spawn
{
    public class BushSpawner : Spawner
    {
        public BushSpawner()
        {
            spawnAttempts = 1500;
            minHeight = 3f;
            maxHeight = 9f;
        }
        
        public override void SpawnOptions(GameObject newPrefab, RaycastHit hit)
        {
            var plant = Instantiate(newPrefab, hit.point, new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f), transform);
            RegisterFloraAgent(plant.GetComponent<FloraAgent>());
            var scale = Random.Range(0.8f, 1.2f);
            plant.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
