using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using Utility;
using World.Agents;
using World.Structure;

namespace World.Environment
{
    public class World : MonoBehaviour
    {
        [Header("Simulation Settings")]
        public List<Agent> AgentList;
        public Vector2Int size;
        [Min(0.15f)]
        public uint pointScale = 1;
        [Min(1)]
        public uint highScale = 1;

        public AnimationCurve test;
        
        public static World Instance;
        
        public Graph MeshGraph;
        public Graph MovementGraph;

        public GameObject[] spawner;

        public Gradient groundColors;
        public Gradient heightColors;
        public Gradient waterLevel;
        public Gradient aridity;
        
        public Color[] colors;

        [Header("Noise Settings")]
        public int randomizer = 10000;
        [Range(0, 10000000)]
        public int seed = 0;
        [Range(0, 50)]
        public float scale = 8;
        [Range(1, 8)] 
        public int octave = 1;
        [Range(0, 1)] 
        public float persistence = 0.4f;
        [Range(0, 1)] 
        public float lacunarity = 0.345f;
        
        [Header("Utility")]
        public bool showMesh = false;
        public bool showGraph = false;
        
        private float maxHeight = 0;
        private float minHeight = float.MaxValue;
        private int currentGradient = 0;

        private Mesh GenerateGraph(out Vector3[] vertices)
        {
            maxHeight = float.MinValue;
            minHeight = float.MaxValue;

            MeshGraph = new Graph();
            MovementGraph = new Graph();

            var mesh = new Mesh();
            mesh.Clear();
            vertices = new Vector3[size.x * size.y];
            var noiseMap =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, 1, scale, octave, persistence, lacunarity, Vector3.zero);

            Node meshNode = null, graphNode = null;
            int x;
            for (x = 0; x < size.x; x++)
            {
                int z;
                for (z = 0; z < size.y; z++)
                {
                    var y = noiseMap[x, z] * test.Evaluate(noiseMap[x, z]) * highScale;
                    //graph for mesh
                    //origin
                    MeshGraph.AddNode(new Vector3(x * pointScale, y, (float)z * pointScale), out var newNode,
                        Node.NodeType.Mesh);
                    vertices[newNode.ID] = new Vector3(newNode.Pos.x, newNode.Pos.y, newNode.Pos.z);
                    if (meshNode is not null)
                    {
                        MeshGraph.AddEdge(meshNode, newNode);
                    }

                    meshNode = newNode;
                    if (x > 0)
                    {
                        //inner squares
                        MeshGraph.AddEdge(meshNode, new Vector2(meshNode.Pos.x - 1 * pointScale, meshNode.Pos.z));
                        if (z > 0)
                        {
                            MeshGraph.AddEdge(meshNode,
                                new Vector2(meshNode.Pos.x - 1 * pointScale, meshNode.Pos.z - 1 * pointScale));
                        }
                    }

                    if (y < minHeight)
                    {
                        minHeight = y;
                    }
                    else if (y > maxHeight)
                    {
                        maxHeight = y;
                    }

                    //graph for movement
                    if (x + 1 < size.x && z + 1 < size.y)
                    {
                        //to left
                        var pos = new Vector3((x + 0.5f) * pointScale, 0, (z + 0.5f) * pointScale);
                        MovementGraph.AddNode(pos, out var newGraphNode);

                        if (graphNode is not null && z != 0)
                        {
                            MovementGraph.AddEdge(graphNode, newGraphNode);
                        }

                        graphNode = newGraphNode;

                        if (x > 0)
                        {
                            //to left
                            MovementGraph.AddEdge(graphNode, new Vector2(pos.x - 1f * pointScale, pos.z));
                            if (z > 0)
                            {
                                //back left
                                MovementGraph.AddEdge(graphNode, new Vector2(pos.x - 1 * pointScale, pos.z - 1 * pointScale));
                            }

                            if (z + 1 < size.y - 1)
                            {
                                //back right
                                MovementGraph.AddEdge(graphNode, new Vector2(pos.x - 1 * pointScale, pos.z + 1 * pointScale));
                            }
                        }
                    }
                }

                meshNode = null;
                //graphNode = null;
            }
            return mesh;
        }

        private Color[] GenerateColor(Gradient gradient)
        {
            int x;
            //vertex-color
            var color = new Color[MeshGraph.Nodes.Count];
            var i = 0;
            for (x = 0; x < size.x; x++)
            {
                for (var z = 0; z < size.y; z++)
                {
                    var height = MeshGraph.Nodes[new Vector2((float)x * pointScale, (float)z * pointScale)].Pos.y;
                    color[i] = gradient.Evaluate(Mathf.InverseLerp(minHeight, maxHeight, height));
                    i++;
                }
            }
            return color;
        }
        

        private int[] GenerateTriangles()
        {
            var triangles = new int[size.x * size.y * 6];
            var currentPos = Vector3.zero;

            var next = MeshGraph.Nodes[currentPos];
            Node firstElementInRow;
            var i = 0;

            //triangles
            do
            {
                firstElementInRow = next;
                Node startVertex;
                do
                {
                    //0
                    startVertex = next;
                    triangles[i++] = startVertex.ID;
                    //1
                    startVertex.Up(out next);
                    triangles[i++] = next.ID;
                    //2
                    next.Right(out var right);
                    triangles[i++] = right.ID;
                    //0
                    triangles[i++] = startVertex.ID;
                    //2
                    triangles[i++] = right.ID;
                    //3
                    right.Down(out next);
                    triangles[i++] = next.ID;
                } while (startVertex.Up(out next) && next.Up());
            } while (firstElementInRow.Right(out next) && next.Right());

            return triangles;
        }

        private void GenerateUV(Mesh mesh)
        {
            var uv = new Vector2[mesh.vertexCount];
            for (var i = 0; i < uv.Length; i++)
            {
                uv[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
            }
            mesh.uv = uv;
        }
        
        private IEnumerator ChangeGradientColor(Gradient gradient)
        {
            int x;
            //vertex-color
            var i = 0;
            
            for (x = 0; x < size.x; x++)
            {
                for (var z = 0; z < size.y; z++)
                {
                    var height = MeshGraph.Nodes[new Vector2((float)x * pointScale, (float)z * pointScale)].Pos.y;
                    colors[i] = gradient.Evaluate(Mathf.InverseLerp(minHeight, maxHeight, height));
                    i++;
                }
                //yield return null;
            }
            GetComponent<MeshFilter>().sharedMesh.colors = colors;
            yield break;
        }
        
        [Button("Generate")]
        private void Generate()
        {
            var mesh = GenerateGraph(out var vertices);

            mesh.vertices = vertices;
            
            mesh.triangles = GenerateTriangles();

            mesh.colors = GenerateColor(groundColors);
            colors = mesh.colors;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            GenerateUV(mesh);
            
            GetComponent<MeshCollider>().sharedMesh = mesh;
            GetComponent<MeshFilter>().mesh = mesh;

            foreach (var obj in spawner)
            {
                var spawn = obj.GetComponent<ISpawner>();
                if (spawn is null)
                {
                    throw new ISpawner.NoSpawnerException();
                }
                spawn.Spawn();
            }
#if UNITY_EDITOR
            UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
#endif
            GetComponent<NavMeshSurface>().BuildNavMesh();
        }

        [Button("Change Gradient")]
        private void ChangeGradient()
        {
            if (currentGradient == 0)
            {
                currentGradient++;
                StartCoroutine(ChangeGradientColor(groundColors));
            }
            else
            {
                currentGradient = 0;
                StartCoroutine(ChangeGradientColor(heightColors));
            }
        }


        private void Start()
        {
            Generate();
        }
        
        private void Awake()
        {
            if(!Instance)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("World instance already set!");
            }
        }
        
        private void Update()
        {
            //HandleAgents();
        }

        private void HandleAgents()
        {
            foreach (var agent in AgentList)
            {
                agent.Handle();
            }
        }

        public void Spawn(GameObject grass)
        {
            var center = grass.transform.position;
            var x = Random.Range(center.x - 3, center.x + 3);
            var z = Random.Range(center.z - 3, center.z + 3);
            
            Physics.Raycast(new Ray(new Vector3(x, 50, z), new Vector3(x, -50, z)), 
                out var hit, 200, LayerMask.GetMask("World", "Water"));
            
            if (hit.point == Vector3.zero || hit.transform.gameObject.layer == 4 || hit.point.y > 10)
            {
                return;
            }
            Instantiate(grass, hit.point, grass.transform.rotation, grass.transform.parent);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //graph
            if (showGraph)
            {
                foreach (var node in MovementGraph.NodeValues)
                {
                    Gizmos.color = node.NodeColor();
                    //Handles.Label(new Vector3(node.Pos.x, node.Pos.y+1, node.Pos.z), node.Pos.ToString());
                    Gizmos.DrawSphere(new Vector3(node.Pos.x, node.Pos.y, node.Pos.z), 0.2f*pointScale);
                }
                foreach (var edge in MovementGraph.EdgeValues)
                {
                    var vec1 = edge.Nodes[0].Pos;
                    var vec2 = edge.Nodes[1].Pos;
                    Gizmos.color = edge.ColorType;
                    Gizmos.DrawLine(new Vector3(vec1.x, 0, vec1.z), new Vector3(vec2.x, 0, vec2.z));
                }
            }
            //mesh
            if (showMesh)
            {
                foreach (var node in MeshGraph.NodeValues)
                {
                    Gizmos.color = node.NodeColor();
                    //Handles.Label(new Vector3(node.Pos.x, node.Pos.y+1, node.Pos.z), node.ID.ToString());
                    Handles.Label(new Vector3(node.Pos.x, node.Pos.y+1, node.Pos.z), node.Pos.y.ToString(CultureInfo.CurrentCulture));
                    Gizmos.DrawSphere(new Vector3(node.Pos.x, node.Pos.y, node.Pos.z), 0.2f*pointScale);
                }
                foreach (var edge in MeshGraph.EdgeValues)
                {
                    var vec1 = edge.Nodes[0].Pos;
                    var vec2 = edge.Nodes[1].Pos;
                    Gizmos.color = edge.ColorType;
                    Gizmos.DrawLine(new Vector3(vec1.x, vec1.y, vec1.z), new Vector3(vec2.x, vec2.y, vec2.z));
                }
            }
        }
#endif
    }
}