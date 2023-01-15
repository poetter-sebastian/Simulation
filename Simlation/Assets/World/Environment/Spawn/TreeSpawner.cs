using UnityEngine;
using World.Agents;
using World.Agents.Modifier.Diseases;
using Random = UnityEngine.Random;

namespace World.Environment.Spawn
{
    public class TreeSpawner : Spawner
    {
        public int deceaseCounter = 0;
        
        public TreeSpawner()
        {
            spawnAttempts = 550;
            minHeight = 3.5f;
            maxHeight = 7.5f;
        }

        public override void SpawnOptions(GameObject newPrefab, RaycastHit hit)
        {
            var plant = Instantiate(newPrefab, hit.point, new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f), transform);
            var tree = plant.GetComponent<TreeAgent>();
            
            tree.co2Modifier = Random.Range(tree.co2Modifier * 0.75f, tree.co2Modifier * 1.25f);
            tree.o2Modifier = Random.Range(tree.o2Modifier * 0.75f, tree.o2Modifier * 1.25f);
            tree.waterConsumption = Random.Range(tree.waterConsumption * 0.75f, tree.waterConsumption * 1.25f);

            if (deceaseCounter < 15)
            {
                tree.AddDisease(new BarkBeetle());
            }
            else if(Random.Range(0f, 1f) > 0.8f)
            {
                tree.AddDisease(new BarkBeetle());
            }
            
            deceaseCounter++;
            
            RegisterFloraAgent(tree);

            var scale = Random.Range(0.4f, 1);
            plant.transform.localScale = new Vector3(scale, scale, scale);
        }

        public GameObject GetRandomTree()
        {
            return objs[Random.Range(0, objs.Length)];
        }
    }
}
