using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using NaughtyAttributes;
using Player;
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
    public class WorldController : MonoBehaviour, ILog
    {
        public PlayerHandler player;
        
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
        public GameObject rubbish;
        public GameObject[] plantSpawner;
        public GameObject[] otherSpawner;
        

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
        public bool spawnPlants = false;
        public string LN() => "Time handler";
        
        public TimeHandler timeHandler;
        public ClimateHandler climateHandler;
        //analytics
        public FileWriter comp;
        public FileWriter compHandle;
        private readonly Stopwatch timer = new();
        
        private float maxHeight = 0;
        private float minHeight = float.MaxValue;

        /// <summary>
        /// Generate the graph for movement and world-mesh
        /// </summary>
        /// <param name="vertices"></param>
        /// <complexity>O(6n²+164n+1204) => O(6n²+164n), Θ(6n²+142n+841) => Θ(6n²+142n+841) => Θ(6n²+142n)</complexity>
        /// <returns></returns>
        private Mesh GenerateGraph(out Vector3[] vertices)
        {
            maxHeight = float.MinValue; //O(1)
            minHeight = float.MaxValue; //O(1)

            MeshGraph = new Graph(); //O(1)
            Grounds = new Dictionary<Vector2, Ground>(); //O(1)
            MovementGraph = new Graph(); //O(1)

            var mesh = new Mesh(); //O(1)
            mesh.Clear(); //O(1)
            vertices = new Vector3[size.x * size.y]; //O(1)
            var noiseMap =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed, scale, octave, persistence, lacunarity, Vector3.zero); //O(145n²)
            
            var noiseMapSand =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed+1, scale, octave, persistence, lacunarity, Vector3.zero); //O(145n²)
            
            var noiseMaSilt =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed+2, scale, octave, persistence, lacunarity, Vector3.zero); //O(145n²)
            
            var noiseMapClay =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed+3, scale, octave, persistence, lacunarity, Vector3.zero); //O(145n²)

            var noiseMapLoam =
                Noise.GenerateNoiseMap(size.x + 1, size.y + 1, seed+4, scale, octave, persistence, lacunarity, Vector3.zero); //O(145n²)
            
            Node meshNode = null, graphNode = null;
            int x;
            for (x = 0; x < size.x; x++) //O(n²+44n+484) Θ(n²+22n+121)
            {
                int z;
                for (z = 0; z < size.y; z++) //O(n*22) Θ(n*11)
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

                meshNode = null; //O(22)
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
                    ground.InitWater(arid);
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <complexity>O(n²+94n+2214) => O(n²+94n) => O(n²), Θ(n²+18n+86) => Θ(n²+18n) => Θ(n²), Ω(n²)</complexity>
        /// <returns></returns>
        private int[] GenerateTriangles()
        {
            var triangles = new int[size.x * size.y * 6]; //O(1)
            var currentPos = Vector3.zero; //O(1)

            var next = MeshGraph.Nodes[currentPos]; //O(1)
            Node firstElementInRow; //O(1)
            var i = 0; //O(1)

            //triangles
            do //O(n²+94n+2214) => O(n²+94n) => O(n²), Θ(n²+18n+81) => Θ(n²+18n) => Θ(n²), Ω(n²)
            {
                firstElementInRow = next;
                Node startVertex;
                do
                {
                    //0
                    startVertex = next; //O(1)
                    triangles[i++] = startVertex.ID; //O(1)
                    //1
                    startVertex.Up(out next); //O(14) Θ(7) Θ(1)
                    triangles[i++] = next.ID;
                    //2
                    next.Right(out var right); //O(14) Θ(7) Θ(1)
                    triangles[i++] = right.ID; //O(1)
                    //0
                    triangles[i++] = startVertex.ID; //O(1)
                    //2
                    triangles[i++] = right.ID; //O(1)
                    //3
                    right.Down(out next); //O(14) Θ(7) Θ(1)
                    triangles[i++] = next.ID;
                } while (startVertex.Up(out next) && next.Up()); //O(n+47)
            } while (firstElementInRow.Right(out next) && next.Right()); //O(n+47)

            return triangles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <complexity>O(n²+2) => O(n²), Θ(n²+2) => Θ(n²), Ω(n²)</complexity>
        /// <param name="mesh"></param>
        private void GenerateUV(Mesh mesh)
        {
            var uv = new Vector2[mesh.vertexCount]; //O(1)
            for (var i = 0; i < uv.Length; i++) //O(n²)
            {
                uv[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
            }
            mesh.uv = uv; //O(1)
        }

        [Button("Generate")]
        private void Generate()
        {
            //#if UNITY_EDITOR
            timer.Start();
            //#endif
            
            var mesh = GenerateGraph(out var vertices);
            
            //#if UNITY_EDITOR
            timer.Stop();
            comp.WriteData("GenerateGraph", timer.ElapsedTicks);
            timer.Reset();
            //#endif

            mesh.vertices = vertices;
            
            //#if UNITY_EDITOR
            timer.Start();
            //#endif
            
            mesh.triangles = GenerateTriangles();

            //#if UNITY_EDITOR
            timer.Stop();
            comp.WriteData("GenerateTriangles", timer.ElapsedTicks);
            timer.Reset();
            timer.Start();
            //#endif
            
            GenerateColorArray(mesh);
            
            //#if UNITY_EDITOR
            timer.Stop();
            comp.WriteData("GenerateColorArray", timer.ElapsedTicks);
            timer.Reset();
            //#endif
            
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            //#if UNITY_EDITOR
            timer.Start();
            //#endif
            
            GenerateUV(mesh);
            
            //#if UNITY_EDITOR
            timer.Stop();
            comp.WriteData("GenerateUV", timer.ElapsedTicks);
            timer.Reset();
            //#endif
            
            GetComponent<MeshCollider>().sharedMesh = mesh;
            GetComponent<MeshFilter>().mesh = mesh;

            //spawn plants
            if (spawnPlants)
            {
                //#if UNITY_EDITOR
                timer.Start();
                //#endif
                foreach (var obj in plantSpawner)
                {
                    var spawn = obj.GetComponent<Spawner>();
                    if (spawn is null)
                    {
                        throw new Spawner.NoSpawnerException();
                    }
                    spawn.Spawn(this);
                }
                //#if UNITY_EDITOR
                timer.Stop();
                comp.WriteData("SpawnPlants", timer.ElapsedTicks);
                timer.Reset();
                //#endif
            }
            
            //spawn others
            foreach (var obj in otherSpawner)
            {
                //#if UNITY_EDITOR
                timer.Start();
                //#endif
                var spawn = obj.GetComponent<Spawner>();
                if (spawn is null)
                {
                    throw new Spawner.NoSpawnerException();
                }
                spawn.Spawn(this);
                //#if UNITY_EDITOR
                timer.Stop();
                comp.WriteData("SpawnObjects", timer.ElapsedTicks);
                timer.Reset();
                //#endif
            }
            //init UI 
            player.UpdateStatisticsValue();
            
            GetComponent<MeshFilter>().sharedMesh.colors = textureColors;
//#if UNITY_EDITOR
//            UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
//#endif
//            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
        
        private void Start()
        {
            //init file logger for comp measurements
            comp = new FileWriter("complexity");
            compHandle = new FileWriter("complexityAgentHandle");

            //loads the time handler
            timeHandler = GetComponent<TimeHandler>();
            timeHandler.TimeHourElapsed += HandleAgents;
            
            //TODO do this with coroutine!
            /*
            timeHandler.TimeChangedToMidnight += delegate(object sender, EventArgs args)
            {
                //#if UNITY_EDITOR
                timer.Start();
                //#endif
                for (var i = 0; i < size.x; i++)
                {
                    for (var j = 0; j < size.y; j++)
                    {
                        var vec = new Vector2(i * pointScale, j * pointScale);
                        var selGround = Grounds[vec];
                        CalcWaterArea(selGround ,0);
                    }
                }
                //#if UNITY_EDITOR
                timer.Stop();
                comp.WriteData("CalcWater", timer.ElapsedTicks.ToString());
                timer.Reset();
                //#endif
            };*/
            
            //loads the climate handler
            climateHandler = GetComponentInChildren<ClimateHandler>();
            
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
                ILog.LE(LN, "World instance already set!");
            }
        }

        /// <summary>
        /// Handle all agents who are active per elapsed hour.
        /// </summary>
        /// <param name="sender">TimeHandler caller</param>
        /// <param name="e">Event data</param>
        /// <complexity>O(3p+1) => O(3p), Θ(p+1) => Θ(p), Ω(p)</complexity>
        public void HandleAgents(object sender, EventArgs e)
        {
            //#if UNITY_EDITOR
            timer.Start();
            //#endif
            StartCoroutine(IteratePlants()); //O(p) (all plants)
            foreach (var rAgent in removeList) //O(p), Θ(p/2), Ω(1)
            {
                rAgent.OnAfterDeath(this, EventArgs.Empty); //O(1)
            }
            removeList.Clear();//O(p), Θ(p/2), Ω(1)
            if (currentColor == ActiveColor.Arid) //O(1)
            {
                GetComponent<MeshFilter>().sharedMesh.colors = aridityColors; //O(1)
            }
            //#if UNITY_EDITOR
            timer.Stop();
            compHandle.WriteData("HandleAgents", timer.ElapsedTicks);
            timer.Reset();
            //#endif
        }

        /// <summary>
        /// Iterate over all plants and do one plant per update.
        /// </summary>
        /// <complexity>O((n+7)*(k+7)*(p+7)), Θ((n+6)*(k+6)*(p+6)), Ω(n)</complexity>
        /// <returns>Null yield because there is no waiting time.</returns>
        public IEnumerator IteratePlants()
        {
            foreach (Transform plantTypes in plants.transform)
            {
                //trees, bushes, grass
                foreach (Transform plant in plantTypes.transform) //O(n+7), Θ(n+6), Ω(n)
                {
                    if (plant.gameObject.activeSelf) //O(7), Θ(6), Ω(6)
                    {
                        var agent = plant.GetComponent<FloraAgent>();  //O(3) because 3 components
                        if(agent)
                        {
                            plant.GetComponent<FloraAgent>().OnHandle(this); //O(3) because 3 components
                        }
                        else
                        {
                            plant.GetComponentInParent<FloraAgent>()?.OnHandle(this); //O(4) because 3 components in parent
                        }
                        yield return null;
                    }
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consume"></param>
        /// <param name="ground"></param>
        public void CalcWaterArea(Ground ground, float consume)
        {
            var pos = new Vector2(ground.Node.Pos.x, ground.Node.Pos.z);

            var sumWater = 0f;

            //plant root length
            var area = 2;

            //to ignore the border elements to prevent desiccation from the border
            var runner = 0;
            
            for (var i = -area; i < area; i++)
            {
                for (var j = -area; j < area; j++)
                {
                    //TODO fix this for float point scale
                    var vec = new Vector2(pos.x + i * pointScale, pos.y + j * pointScale);
                    if (vec.x < 0 || vec.y < 0 || vec.x >= size.x * pointScale || vec.y >= size.y * pointScale)
                    {
                        continue;
                    }
                    runner++;
                    var selGround = Grounds[vec];
                    sumWater += selGround.CurrentWater;
                }
            }

            sumWater += consume;
            sumWater /= runner;
            //to prevent infinite small numbers
            sumWater = sumWater < 1 ? 0 : sumWater;

            for (var i = -area; i < area; i++)
            {
                for (var j = -area; j < area; j++)
                {
                    //TODO fix this for float point scale
                    var vec = new Vector2(pos.x + i * pointScale, pos.y + j * pointScale);
                    if (vec.x < 0 || vec.y < 0 || vec.x >= size.x * pointScale || vec.y >= size.y * pointScale)
                    {
                        continue;
                    }
                    var selGround = Grounds[vec];
                    //if the ground is under water
                    selGround.SetWater(selGround.Node.Pos.y - 0.3f < climateHandler.waterLevel ? selGround.WaterCapacity : sumWater);
                }
            }
        }
        
        /// <summary>
        /// Set the node color of the aridity
        /// </summary>
        /// <param name="id">Id of the node</param>
        /// <param name="value">Value from 0 to 1</param>
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
            rubbish.SetActive(false);
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
            rubbish.SetActive(false);
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
            rubbish.SetActive(false);
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
            rubbish.SetActive(true);
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
        
        public void Spawn(GameObject obj, Vector3 pos)
        {
            Instantiate(obj, pos, obj.transform.rotation, transform);
        }
        
        public void SpawnPlant(GameObject obj, Vector3 pos)
        {
            RegisterFloraAgent(Instantiate(obj, pos, new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f), plants.transform).GetComponent<FloraAgent>());
        }

        /// <summary>
        /// Register new plant agent to the handler system
        /// </summary>
        /// <param name="agent">agent to add</param>
        public void RegisterFloraAgent(FloraAgent agent)
        {
            var pos = agent.transform.position;
            //round variables to full numbers
            //TODO works only with a pointScale of 1!
            var vec = new Vector2(
                MathF.Round(pos.x, MidpointRounding.AwayFromZero),
                MathF.Round(pos.z, MidpointRounding.AwayFromZero));
            //TODO maybe improvement or perform earlier
            agent.world = this;
            agent.ground = Grounds[vec]; //connects the agent with the ground value
            
            //add modifier to world numbers
            player.o2Production += agent.o2Modifier;
            player.co2Consumption += agent.co2Modifier;
            player.waterConsumption += agent.waterConsumption;
            player.AddTree();
        }

        /// <summary>
        /// Removes an agent from the handler system
        /// </summary>
        public void RemoveAgent(Agent agent)
        {
            //TODO find a better way to deregister tree for the player statistics
            if (agent.GetType() == typeof(TreeAgent))
            {
                var tree = (TreeAgent)agent;
                player.RemoveTree(tree);
                player.co2Consumption -= tree.co2Modifier;
                player.o2Production -= tree.o2Modifier;
                player.waterConsumption -= tree.waterConsumption;
                player.UpdateStatisticsValue();
            }
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
                    //Handles.Label(new Vector3(node.Pos.x, node.Pos.y+1, node.Pos.z), node.ID.ToString());
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.white;
 
                    Handles.BeginGUI();
                    //Handles.Label(new Vector3(node.Pos.x, node.Pos.y+0.5f, node.Pos.z+0.5f), "X:" + node.Pos.x + " Z:" + node.Pos.z, style);
                    Handles.EndGUI();
                    
                    Gizmos.color = Color.gray;
                    Gizmos.DrawSphere(new Vector3(node.Pos.x, node.Pos.y, node.Pos.z), 0.2f*pointScale);
                }
                foreach (var edge in MeshGraph.EdgeValues)
                {
                    var vec1 = edge.Nodes[0].Pos;
                    var vec2 = edge.Nodes[1].Pos;
                    Gizmos.color = Color.blue;
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