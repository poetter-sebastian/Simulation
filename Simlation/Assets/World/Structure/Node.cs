using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace World.Structure
{
    public class Node
    {
        private const double Precision = 0.01;
        public readonly int ID;
        public readonly Vector3 Pos;
        public readonly List<Edge> Edges = new();
        public readonly NodeType type;
        
        public Node(int id, Vector3 pos, NodeType nodeType = NodeType.Graph)
        {
            ID = id;
            Pos = pos;
            type = nodeType;
        }

        public bool Up(out Node node) => QueryEdges(Graph.Direction.Up, out node);
        public bool Up() => QueryEdges(Graph.Direction.Up, out _);
        
        public bool UpRight(out Node node) => QueryEdges(Graph.Direction.UpRight, out node);
        public bool UpRight() => QueryEdges(Graph.Direction.UpRight, out _);

        public bool Right(out Node node) => QueryEdges(Graph.Direction.Right, out node);
        public bool Right() => QueryEdges(Graph.Direction.Right, out _);

        public bool DownRight(out Node node) => QueryEdges(Graph.Direction.DownRight, out node);
        public bool DownRight() => QueryEdges(Graph.Direction.DownRight, out _);

        public bool Down(out Node node) => QueryEdges(Graph.Direction.Down, out node);
        public bool Down() => QueryEdges(Graph.Direction.Down, out _);

        public bool DownLeft(out Node node) => QueryEdges(Graph.Direction.DownLeft, out node);
        public bool DownLeft() => QueryEdges(Graph.Direction.DownLeft, out _);

        public bool Left(out Node node) => QueryEdges(Graph.Direction.Left, out node);
        public bool Left() => QueryEdges(Graph.Direction.Left, out _);

        public bool UpLeft(out Node node) => QueryEdges(Graph.Direction.UpLeft, out node);
        public bool UpLeft() => QueryEdges(Graph.Direction.UpLeft, out _);

        public bool QueryEdges(Graph.Direction dir, out Node node) //O(14) Θ(7) Θ(1)
        {
            node = null; //O(1)
            var ret = false; //O(1)
            foreach (var otherNode in Edges.Select(edge => edge.GetOtherNode(this)).Where(otherNode => Direction(dir, otherNode.Pos))) //O(8)
            {
                ret = true; //O(1)
                node = otherNode; //O(1)
                break;
            }

            return ret;
        }

        public bool Direction(Graph.Direction dir, Vector3 v) => dir switch //, O(8)
        {
            //north x==x,   z++
            //south x==x,   z--
            //east  x++,    z==z
            //west  x--,    z==z
            
            Graph.Direction.North => Math.Abs(Pos.x - v.x) < Precision && Pos.z < v.z, //O(1)
            Graph.Direction.NorthEast => Pos.x < v.x && Pos.z < v.z, //O(1)
            Graph.Direction.East => Pos.x < v.x && Math.Abs(Pos.z - v.z) < Precision, //O(1)
            Graph.Direction.SouthEast => Pos.x < v.x && Pos.z > v.z, //O(1)
            Graph.Direction.South => Math.Abs(Pos.x - v.x) < Precision && Pos.z > v.z, //O(1)
            Graph.Direction.SouthWest => Pos.x > v.x && Pos.z > v.z, //O(1)
            Graph.Direction.West => Pos.x > v.x && Math.Abs(Pos.z - v.z) < Precision, //O(1)
            Graph.Direction.NorthWest => Pos.x > v.x && Pos.z < v.z, //O(1)
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };

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

        public static bool E(Vector2 a, Vector2 b)
        {
            return Math.Abs(a.x - b.x) < Precision && Math.Abs(a.y - b.y) < Precision;
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
            return (a.z > b.z);
        }

        private bool Equals(Node other)
        {
            return ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Node)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }
        
        public enum NodeType
        {
            Mesh,
            Graph
        }

        public Color NodeColor() => NodeColor(type);
        
        public static Color NodeColor(NodeType nodeType) => nodeType switch
        {
            NodeType.Mesh => Color.red,
            NodeType.Graph => Color.cyan,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}