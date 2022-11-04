using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World.Environment.Spawn
{
    public class RubbishSpawn : Spawner
    {
        public RubbishSpawn()
        {
            spawnAttempts = 550;
            minHeight = 3.5f;
            maxHeight = 7.5f;
        }
        
        public override void SpawnOptions(GameObject newPrefab, RaycastHit hit)
        {
            var el = Instantiate(newPrefab, hit.point, new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f), transform);
            
            var scale = Random.Range(0.5f, 1);
            el.transform.localScale = new Vector3(scale, scale, scale);
            var rub = el.GetComponent<Rubbish>();
            if (rub != null)
            {
                rub.player = world.player;
            }
            else
            {
                throw new MissingComponentException("Rubbish component not found");
            }
        }
    }
}
