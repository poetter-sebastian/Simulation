using System;
using UnityEngine;
using World.Agents;
using Random = UnityEngine.Random;

namespace World.Environment.Spawn
{
    public class TreeSpawner : Spawner
    {
        public TreeSpawner()
        {
            spawnAttempts = 550;
            minHeight = 3.5f;
            maxHeight = 7.5f;
        }

        public override void SpawnOptions(GameObject newPrefab, RaycastHit hit)
        {
            var plant = Instantiate(newPrefab, hit.point, new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f), transform);
            RegisterFloraAgent(plant.GetComponent<FloraAgent>());

            var scale = Random.Range(0.4f, 1);
            plant.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
