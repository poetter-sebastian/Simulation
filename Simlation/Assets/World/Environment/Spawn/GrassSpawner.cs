using UnityEngine;

namespace World.Environment.Spawn
{
    public class GrassSpawner : MonoBehaviour, ISpawner
    {
        public GameObject[] plants;
        public int spawnAttempts = 3000;
        public float minHeight = 2.5f;
        public float maxHeight = 10f;
        
        public void Spawn(Vector2Int size)
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
            for (var i = 0; i < spawnAttempts; i++)
            {
                var x = Random.Range(1f, size.x-1f);
                var z = Random.Range(1f, size.y-1f);

                var ray = new Ray(new Vector3(x, 50, z), new Vector3(x, 50-200, z));

                Physics.Raycast(ray, out var hit, 200, LayerMask.GetMask("World", "Water"));

                if (hit.point == Vector3.zero || hit.transform.gameObject.layer == 4)
                {
                    continue;
                }

                if (hit.point.y < minHeight || hit.point.y > maxHeight) continue;

                var obj = Random.Range(0, plants.Length);
                Instantiate(plants[obj], hit.point, new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f), transform);
            }
        }
    }
}
