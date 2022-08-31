using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace World.Structure
{
    public class Graph
    {
        //increment the id number for identify the node
        protected int NodeID = 0;
        //dict of nodes
        public Dictionary<Vector2?, Node> Nodes { get; } = new();
        //collection of nodes
        public Dictionary<Vector2?, Node>.ValueCollection NodeValues => Nodes.Values;
        //dict of all edges
        private readonly SymmetricVectorDict<Edge> edges = new();
        //dict of all edges
        public SymmetricVectorDict<Edge> Edges => edges;
        //collection of vector pair
        public Dictionary<(Vector2, Vector2), Edge>.KeyCollection EdgeKeys => edges.Keys;
        public Dictionary<(Vector2, Vector2), Edge>.ValueCollection EdgeValues => edges.Values;

        public int NextID() => NodeID++;

        public bool AddNode(Vector3 pos)
        {
            return AddNode(pos, out _);
        }
        
        public bool AddNode(Vector3 pos, out Node node, Node.NodeType type = Node.NodeType.Graph)
        {
            node = new Node(NodeID++, pos, type);
            var ret = Nodes.TryAdd(new Vector2(pos.x, pos.z), node);
            if (!ret)
            {
                Debug.LogError("Doubled node on position: " + pos);
            }
            return ret;
        }

        public bool AddNode(Node node)
        {
            return Nodes.TryAdd(new Vector2(node.Pos.x, node.Pos.z), node);
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
        
        public bool AddEdge([CanBeNull] Edge edge)
        {
            if (edge is null)
            {
                throw new NullReferenceException();
            }
            return edges.Add(edge.Nodes[0].Pos, edge.Nodes[1].Pos, edge);
        }
        
        public bool AddEdge([CanBeNull] Node start, [CanBeNull] Node end)
        {
            if (start is null || end is null)
            {
                throw new NullReferenceException();
            }
            var edge = new Edge(start, end);
            return edges.Add(new Vector2(start.Pos.x, start.Pos.z), new Vector2(end.Pos.x, end.Pos.z), edge);
        }
        
        public bool AddEdge([CanBeNull] Node start, Vector2? end)
        {
            if (start is null || end is null)
            {
                throw new NullReferenceException();
            }
            return AddEdge(start, Nodes[end]);
        }
        
        public bool AddEdge(Vector2? start, Vector2? end)
        {
            if (start is null || end is null)
            {
                throw new NullReferenceException();
            }
            return AddEdge(Nodes[start], Nodes[end]);
        }

        public void AddEdges(IEnumerable<Edge> newEdges)
        {
            foreach (var edge in newEdges)
            {
                AddEdge(edge);
            }
        }
        
        public enum Direction
        {
            North,
            Up = North,
            
            NorthEast,
            UpRight = NorthEast,
            
            East,
            Right = East,
            
            SouthEast,
            DownRight = SouthEast,
            
            South,
            Down = South,
            
            SouthWest,
            DownLeft = SouthWest,
            
            West,
            Left = West,

            NorthWest,
            UpLeft = NorthWest,
        }

        public class SameNodeError : Exception
        {
            // ReSharper disable once InconsistentNaming
            public new string Message = "The two nodes for the edge are the same.";
        }
        
        public class DiffNodeType : Exception
        {
            // ReSharper disable once InconsistentNaming
            public new string Message = "The nodes have different types and should not be connected.";
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
    
    public class SymmetricVectorDict<TValue> : Dictionary<(Vector2, Vector2), TValue>
    {
        public TValue this[Vector2 x, Vector2 y] => this[(x, y), default];
        public new TValue this[(Vector2, Vector2) key] => this[key, default];

        private TValue this[(Vector2, Vector2) key, TValue defaultValue]
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

        public new bool Add((Vector2, Vector2) key, TValue value)
        {
            return TryAdd(Node.GT(key.Item1, key.Item2) ? key : (key.Item2, key.Item1), value);
        }
        
        public bool Add(Vector2 key1, Vector2 key2, TValue value)
        {
            return Add((key1, key2), value);
        }
    }
}