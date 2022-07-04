using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Utility;
using World.Agents;
using World.Structure;
using Random = UnityEngine.Random;

namespace World.Environment
{
    public class World : MonoBehaviour
    {
        public List<Agent> AgentList;
        public Vector2Int size;
        [Min(0.15f)]
        public uint pointScale = 1;
        [Min(1)]
        public uint highScale = 1;
        [Min(0.001f)]
        public bool showMesh = false;
        public bool showGraph = false;
        
        public Graph MeshGraph;
        public Graph MovementGraph;

        public Color[] colors;
        public Gradient heightColors;
        
        [Header("Noise Settings")]
        public int randomizer = 10000;
        [Range(0, 10000000)]
        public int seed = 0;
        [Range(0, 20)]
        public float scale = 8;
        [Range(1, 8)] 
        public int octave = 1;
        [Range(0, 1)] 
        public float persistence = 0.4f;
        [Range(0, 1)] 
        public float lacunarity = 0.345f;

        [Button("Generate")]
        private void MethodOne()
        {
            Start();
        }


        private float maxHeight = 2;
        private float minHeight = float.MaxValue;
        
        private void Start()
        {
            maxHeight = float.MinValue;
            minHeight = float.MaxValue;
            
            MeshGraph = new Graph();
            MovementGraph = new Graph();
            
            var mesh = new Mesh();
            mesh.Clear();
            var vertices = new Vector3[size.x * size.y];
            var noiseMap = Noise.GenerateNoiseMap(size.x+1, size.y+1, 1, scale, octave, persistence, lacunarity, Vector3.zero);

            Node meshNode = null, graphNode = null;
            int z;
            for (var x = 0; x < size.x; x++)
            {
                for (z = 0; z < size.y; z++)
                {
                    var y = noiseMap[x, z]*highScale;
                    //graph for mesh
                    //origin
                    MeshGraph.AddNode(new Vector3(x*pointScale, y, (float)z*pointScale), out var newNode, Node.NodeType.Mesh);
                    vertices[newNode.ID] = new Vector3(newNode.Pos.x, newNode.Pos.y, newNode.Pos.z);
                    if (meshNode is not null)
                    {
                        MeshGraph.AddEdge(meshNode, newNode);
                    }
                    meshNode = newNode;
                    if (x > 0)
                    {
                        //inner squares
                        MeshGraph.AddEdge(meshNode, new Vector2(meshNode.Pos.x-1*pointScale, meshNode.Pos.z));
                        if (z > 0)
                        {
                            MeshGraph.AddEdge(meshNode, new Vector2(meshNode.Pos.x-1*pointScale, meshNode.Pos.z-1*pointScale));
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
                            MovementGraph.AddEdge(graphNode, new Vector2(pos.x-1f*pointScale, pos.z));
                            if (z > 0)
                            {
                                //back left
                                MovementGraph.AddEdge(graphNode, new Vector2(pos.x-1*pointScale, pos.z-1*pointScale));
                            } 
                            if (z + 1 < size.y-1)
                            {
                                //back right
                                MovementGraph.AddEdge(graphNode, new Vector2(pos.x-1*pointScale, pos.z+1*pointScale));
                            }
                        }
                    }
                }
                meshNode = null; 
                //graphNode = null;
            }

            mesh.vertices = vertices;

            var triangles = new int[(size.x * size.y) * 6];
            var currentPos = Vector3.zero;
            
            var next = MeshGraph.Nodes[currentPos];
            Node firstElementInRow;
            var i = 0;

            //

            do
            { 
                firstElementInRow = next; 
                do
                {
                    triangles[i++] = next.ID;
                    next.Up(out next);
                    triangles[i++] = next.ID;
                    next.Right(out next);
                    triangles[i++] = next.ID;
                    next.DownLeft(out next);
                    triangles[i++] = next.ID;
                    next.UpRight(out next);
                    triangles[i++] = next.ID;
                    next.Down(out next);
                    triangles[i++] = next.ID;
                    next.Left(out next);
                } while (next.Up(out next) && next.Up());
            } while (firstElementInRow.Right(out next) && next.Right());

            colors = new Color[MeshGraph.Nodes.Count];
            for (i = 0, z = 0; z < size.y; z++)
            {
                for (var x = 0; x < size.x; x++)
                {
                    try
                    {
                        var height = MeshGraph.Nodes[new Vector2((float)x*pointScale, (float)z*pointScale)].Pos.y;
                        colors[i] = heightColors.Evaluate(Mathf.InverseLerp(minHeight, maxHeight, height));
                        i++;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            
            mesh.triangles = triangles;
            mesh.colors = colors;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            GetComponent<MeshCollider>().sharedMesh = mesh;
            GetComponent<MeshFilter>().mesh = mesh;
        } 
 
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

        // Update is called once per frame
        private void Update()
        {
            HandleAgents();
        }

        private void HandleAgents()
        {
            foreach (var agent in AgentList)
            {
                agent.Handle();
            }
        }
    }
}