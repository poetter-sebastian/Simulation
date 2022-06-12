using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector2Int size;
    
    public const int SquareSize = 1;

    // Start is called before the first frame update
    private void Start()
    {
        var heatmap = new Mesh();

        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        int vertexIndex = 0;
 
        for (uint x = 0; x < size.x; x++)
        {
            for (uint y = 0; y < size.y; y++)
            {
               var currentFace = new[] {
                    new Vector3(0 + x, 0.5f, 0 + y),
                    new Vector3(SquareSize + x, 0.5f, 0 + y),
                    new Vector3(SquareSize + x, 0.5f, SquareSize + y),
                    new Vector3(0 + x, 0.5f, 1 + y)
                };
  
                vertices.Add(currentFace[0]);
                vertices.Add(currentFace[1]);
                vertices.Add(currentFace[2]);
                vertices.Add(currentFace[3]);
  
                triangles.Add(0 + vertexIndex);
                triangles.Add(1 + vertexIndex);
                triangles.Add(2 + vertexIndex);
                triangles.Add(0 + vertexIndex);
                triangles.Add(2 + vertexIndex);
                triangles.Add(3 + vertexIndex);
 
                vertexIndex += 4;
            }
        }
        
        heatmap.vertices = vertices.ToArray();
        heatmap.triangles = triangles.ToArray();

        heatmap.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = heatmap;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
