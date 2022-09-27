using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;
using World.Agents;
using World.Structure;
using Random = UnityEngine.Random;

namespace World.Environment
{
    public class WorldController : MonoBehaviour
    {
        [Header("Simulation Settings")]
        public Vector2Int size;
        [Min(0.15f)]
        public uint pointScale = 1;
        [Min(1)]
        public uint highScale = 1;

        public AnimationCurve highMultiplier;
        
        public static WorldController Instance;
        
        public Graph MeshGraph;
        public Graph MovementGraph;
        public Dictionary<Vector2, Ground> Grounds;

        public List<Agent> removeList;
        
        public GameObject water;
        public GameObject plants;
        public GameObject border;
        public GameObject[] spawner;

        public Gradient groundGradient;
        public Gradient heightGradient;
        public Gradient aridityGradient;

        public ActiveColor currentColor = ActiveColor.Texture;
        
        public Color[] textureColors;
        public Color[] groundTypeColors;
        public Color[] heightColors;
        public Color[] aridityColors;

        [Header("Noise Settings")]
        [Range(0, 10000000)]
        public int seed = 1;
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

        private TimeHandler timeHandler;

        private Mesh GenerateGraph(out Vector3[] vertices)
        {
            maxHeight = float.MinValue;
            minHeight = float.MaxValue;

            MeshGraph = new Graph();
            Grounds = new Dictionary<Vector2, Ground>();
            MovementGraph = new Graph();

            var mesh = new Mesh();
            mesh.Clear();
            vertices = new Vector3[size.x * size.y];
            var noiseMap =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed, scale, octave, persistence, lacunarity, Vector3.zero);
            
            var noiseMapSand =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed+1, scale, octave, persistence, lacunarity, Vector3.zero);
            
            var noiseMaSilt =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed+2, scale, octave, persistence, lacunarity, Vector3.zero);
            
            var noiseMapClay =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed+3, scale, octave, persistence, lacunarity, Vector3.zero);

            var noiseMapLoam =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed+4, scale, octave, persistence, lacunarity, Vector3.zero);
            
            Node meshNode = null, graphNode = null;
            int x;
            for (x = 0; x < size.x; x++)
            {
                int z;
                for (z = 0; z < size.y; z++)
                {
                    //high scale
                    var y = noiseMap[x, z] * highMultiplier.Evaluate(noiseMap[x, z]) * highScale; 
                    //graph for mesh
                    //origin
                    // ReSharper disable once SuggestVarOrType_SimpleTypes
                    var vec = new Vector3(x * pointScale, y, (float)z * pointScale);
                    MeshGraph.AddNode(vec, out var newNode);
                    Grounds.Add(new Vector2(vec.x, vec.z), new Ground(this, newNode, noiseMapSand[x, z], noiseMaSilt[x, z], noiseMapClay[x, z], noiseMapLoam[x, z]));
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

        private void GenerateColorArray(Mesh mesh)
        {
            //vertex-color
            var texture = new Color[MeshGraph.Nodes.Count];
            var groundType = new Color[MeshGraph.Nodes.Count];
            var aridity = new Color[MeshGraph.Nodes.Count];
            var heights = new Color[MeshGraph.Nodes.Count]; 

            var i = 0;
            for (var x = 0; x < size.x; x++)
            {
                for (var z = 0; z < size.y; z++)
                {
                    var node = MeshGraph.Nodes[new Vector2((float)x * pointScale, (float)z * pointScale)];
                    var height = node.Pos.y;
                    texture[i] = groundGradient.Evaluate(Mathf.InverseLerp(minHeight, maxHeight, height));
                    heights[i] = heightGradient.Evaluate(Mathf.InverseLerp(minHeight, maxHeight, height));
                    var arid = Mathf.InverseLerp(minHeight, maxHeight, height);
                    aridity[i] = aridityGradient.Evaluate(arid);
                    
                    var vec = new Vector2((float)x * pointScale, (float)z * pointScale);
                    var ground = Grounds[vec];
                    ground.SetAridity(arid, true);
                    groundType[i] = ground.typColor;
                    i++;
                }
            }
            heightColors = heights;
            aridityColors = aridity;
            textureColors = texture;
            groundTypeColors = groundType;
            mesh.colors = texture;
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

        [Button("Generate")]
        private void Generate()
        {
            var mesh = GenerateGraph(out var vertices);

            mesh.vertices = vertices;
            
            mesh.triangles = GenerateTriangles();

            GenerateColorArray(mesh);
            
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
                spawn.Spawn(size, this);
            }
            GetComponent<MeshFilter>().sharedMesh.colors = textureColors;
//#if UNITY_EDITOR
//            UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
//#endif
//            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
        
        private void Start()
        {
            timeHandler = GetComponent<TimeHandler>();
            timeHandler.TimeHourElapsed += HandleAgents;
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

        public void HandleAgents(object sender, HourElapsedEventArgs e)
        {
            foreach (Transform plants in plants.transform)
            {
                //trees, bushes, grass
                foreach (Transform plant in plants.transform)
                {
                    plant.GetComponent<FloraAgent>().OnHandle(this);
                }
            }
            foreach (var rAgent in removeList)
            {
                rAgent.OnAfterDeath(this, EventArgs.Empty);
            }
            removeList.Clear();
            if (currentColor == ActiveColor.Arid)
            {
                GetComponent<MeshFilter>().sharedMesh.colors = aridityColors;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consume"></param>
        /// <param name="ground"></param>
        public void CalcWaterArea(float consume, Ground ground)
        {
            //var sum = ground
        }
        
        public void UpdateGroundColor(int id, float value)
        {
            aridityColors[id] = aridityGradient.Evaluate(value);
        }

        [Button("Show Ground")]
        public void ActivateGroundTypeColors()
        {
            currentColor = ActiveColor.Type;
            water.SetActive(false);
            plants.SetActive(false);
            border.SetActive(false);
            GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Shader Graphs/NodeColor");
            GetComponent<MeshFilter>().sharedMesh.colors = groundTypeColors;
        }

        [Button("Show aridity")]
        public void ActivateHumidity()
        {
            currentColor = ActiveColor.Arid;
            water.SetActive(true);
            plants.SetActive(false);
            border.SetActive(false);
            GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Shader Graphs/NodeColor");
            GetComponent<MeshFilter>().sharedMesh.colors = aridityColors;
        }

        [Button("Show height map")]
        public void ActivateHeight()
        {
            currentColor = ActiveColor.Height;
            water.SetActive(false);
            plants.SetActive(false);
            border.SetActive(false);
            GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Shader Graphs/NodeColor");
            GetComponent<MeshFilter>().sharedMesh.colors = heightColors;
        }
        
        [Button("Show textures")]
        public void ActivateTextures()
        {
            currentColor = ActiveColor.Texture;
            water.SetActive(true);
            plants.SetActive(true);
            border.SetActive(true);
            GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Shader Graphs/TerrainColor");
            GetComponent<MeshFilter>().sharedMesh.colors = textureColors;
        }

        public void Spawn(GameObject obj)
        {
            var center = obj.transform.position;
            var x = Random.Range(center.x - 3, center.x + 3);
            var z = Random.Range(center.z - 3, center.z + 3);
            
            Physics.Raycast(new Ray(new Vector3(x, 50, z), new Vector3(x, -50, z)), 
                out var hit, 200, LayerMask.GetMask("World", "Water"));
            
            if (hit.point == Vector3.zero || hit.transform.gameObject.layer == 4 || hit.point.y > 10)
            {
                return;
            }
            Instantiate(obj, hit.point, obj.transform.rotation, obj.transform.parent);
        }

        /// <summary>
        /// Register new plant agent to the handler system
        /// </summary>
        /// <param name="agent">agent to add</param>
        public void RegisterFloraAgent(FloraAgent agent)
        {
            var pos = agent.transform.position;
            //round variables to full numbers
            //TODO works only with a pointScale of 1!!!!
            var vec = new Vector2(
                MathF.Round(pos.x, MidpointRounding.AwayFromZero),
                MathF.Round(pos.z, MidpointRounding.AwayFromZero));
            //TODO maybe improvement or perform earlier
            agent.ground = Grounds[vec]; //connects the agent with the ground value
        }

        /// <summary>
        /// Removes an agent from the handler system
        /// </summary>
        public void RemoveAgent(Agent agent)
        {
            removeList.Add(agent);
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
        public enum ActiveColor
        {
            Texture,
            Arid,
            Type,
            Height
        }
    }
}