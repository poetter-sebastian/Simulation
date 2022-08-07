using System;
using UnityEngine;
using Rand = System.Random;
using Random = UnityEngine.Random;

namespace World.Environment.Spawn
{
    public class TreeSpawner : MonoBehaviour, ISpawner
    {
        public GameObject[] trees;
        
        public void Spawn()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
            var rand = new Rand();
            for (var i = 0; i < 550; i++)
            {
                var x = Random.Range(1, 99);
                var z = Random.Range(1, 99);
                
                var start = new Vector3(x, 50, z);
                var end = new Vector3(x, 50-200, z);

                var ray = new Ray(start, end);

                Physics.Raycast(ray, out var hit, 200, LayerMask.GetMask("World", "Water"));

                if (hit.point == Vector3.zero || hit.transform.gameObject.layer == 4)
                {
                    continue;
                }
                
                switch (hit.point.y)
                {
                    case < 3.5f:
                    case > 7.6f:
                        continue;
                    default:
                    {
                        var obj = rand.Next(0, trees.Length);
                        Instantiate(trees[obj], hit.point, Quaternion.identity, transform);
                        break;
                    }
                }
            }
        }
    }
}
