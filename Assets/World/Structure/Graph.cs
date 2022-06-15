using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Object = System.Object;

namespace World.Structure
{
    public class Graph
    {
        private readonly Dictionary<Vector3, Node> nodes = new();
        private readonly SymmetricVectorDict<Edge> edges = new();

        private int nodeID = 0;

        public Graph()
        {
            
        }

        public Dictionary<Vector3, Node> Notes() => nodes;
        public SymmetricVectorDict<Edge> Edges() => edges;

        public int NextID() => nodeID++;
        
        public bool AddNode(Vector3 pos)
        {
            return nodes.TryAdd(pos, new Node(nodeID++, pos));
        }

        public bool AddNode(Node node)
        {
            return nodes.TryAdd(node.Pos, node);
        }
        
        public bool AddNodes(IEnumerable<Node> newNodes)
        {
            var ret = true;
            foreach (var node in newNodes)
            {
                ret = AddNode(node);
            }
            return ret;
        }
        
        public bool AddEdge(Edge edge)
        {
            return edges.Add(edge.Nodes[0].Pos, edge.Nodes[1].Pos, edge);
        }
        
        public bool AddEdge(Node start, Node end)
        {
            var edge = new Edge(start, end);
            return edges.Add(start.Pos, end.Pos, edge);
        }

        public void AddEdges(IEnumerable<Edge> newEdges)
        {
            foreach (var edge in newEdges)
            {
                AddEdge(edge);
            }
        }

        public class SameNodeError : Exception
        {
            public new string Message = "The two nodes for the edge are the same!";
        }
    }
    
    public class Node
    {
        private const double Precision = 0.0001;
        public readonly int ID;
        public readonly Vector3 Pos;
        public readonly List<Edge> Edges = new();
        
        public Node(int id, Vector3 pos)
        {
            ID = id;
            Pos = pos;
        }
        
        public static bool operator ==(Node node, Node node2)
        {
            return node is not null && node2 is not null && node.ID == node2.ID;
        }

        public static bool operator !=(Node node, Node node2)
        {
            return !(node == node2);
        }
        
        public static bool operator > (Node node, Node node2)
        {
            return GT(node.Pos, node2.Pos);
        }

        public static bool operator <(Node node, Node node2)
        {
            return !(node > node2);
        }

        public static bool E(Vector3 a, Vector3 b)
        {
            return Math.Abs(a.x - b.x) < Precision && Math.Abs(a.y - b.y) < Precision && Math.Abs(a.z - b.z) < Precision;
        }
        
        // ReSharper disable once InconsistentNaming
        public static bool GT(Vector3 a, Vector3 b)
        {
            if (a == b)
            {
                return false;
            }
            if (a.x > b.x)
            {
                return true;
            }
            if (a.y > b.y)
            {
                return true;
            }
            return a.z > b.z;
        }
    }
    
    public class Edge
    {
        /// <summary>
        /// Objects of the two nodes
        /// </summary>
        public readonly Node[] Nodes = new Node[2];

        public Edge(Node start, Node end)
        {
            if (start == end)
            {
                throw new Graph.SameNodeError();
            }
            Nodes[0] = start;
            Nodes[1] = end;
        }
        
        public static bool operator ==(Edge node, Edge node2)
        {
            return node2 is not null && node is not null && 
                   ((node.Nodes[0] == node2.Nodes[0] && node.Nodes[1] == node2.Nodes[1]) || 
                    (node.Nodes[0] == node2.Nodes[1] && node.Nodes[1] == node2.Nodes[0]));
        }

        public static bool operator !=(Edge node, Edge node2)
        {
            return !(node == node2);
        }
    }

    public class SymmetricIntDict<TValue> : Dictionary<(int, int), TValue>
    {
        public TValue this[int x, int y] => this[(x, y), default];
        public new TValue this[(int, int) key] => this[key, default];

        private TValue this[(int, int) key, TValue defaultValue]
        {
            get
            {
                if(key.Item1 >= key.Item2 && TryGetValue(key, out var val))
                {
                    return val;
                }
                if (TryGetValue((key.Item2, key.Item1), out val))
                {
                    return val;
                }

                base[key] = defaultValue;
                return defaultValue;
            }
        }

        public new bool Add((int, int) key, TValue value)
        {
            return TryAdd(key.Item1 >= key.Item2 ? key : (key.Item2, key.Item1), value);
        }
        
        public bool Add(int key1, int key2, TValue value)
        {
            return Add((key1, key2), value);
        }
    }
    
    public class SymmetricVectorDict<TValue> : Dictionary<(Vector3, Vector3), TValue>
    {
        public TValue this[Vector3 x, Vector3 y] => this[(x, y), default];
        public new TValue this[(Vector3, Vector3) key] => this[key, default];

        private TValue this[(Vector3, Vector3) key, TValue defaultValue]
        {
            get
            {
                if(Node.GT(key.Item1, key.Item2) && TryGetValue(key, out var val))
                {
                    return val;
                }
                if (TryGetValue((key.Item2, key.Item1), out val))
                {
                    return val;
                }

                base[key] = defaultValue;
                return defaultValue;
            }
        }

        public new bool Add((Vector3, Vector3) key, TValue value)
        {
            return TryAdd(Node.GT(key.Item1, key.Item2) ? key : (key.Item2, key.Item1), value);
        }
        
        public bool Add(Vector3 key1, Vector3 key2, TValue value)
        {
            return Add((key1, key2), value);
        }
    }
}