using System;
using UnityEngine;

namespace World.Structure
{
    public class Edge
    {
        /// <summary>
        /// Objects of the two nodes
        /// </summary>
        public readonly Node[] Nodes = new Node[2];

        public Node Node => Nodes[0];
        public Node Node2 => Nodes[1];

        public Node.NodeType Type => Node.type;
        
        public Color ColorType => Node.NodeColor();

        public Edge(Node start, Node end)
        {
            if (start == end)
            {
                throw new Graph.SameNodeError();
            }

            if (start.type != end.type)
            {
                throw new Graph.DiffNodeType();
            }
            Nodes[0] = start;
            start.Edges.Add(this);
            Nodes[1] = end;
            end.Edges.Add(this);
        }

        public static bool operator ==(Edge edge, Edge edge2)
        {
            return edge2 is not null && edge is not null && 
                   ((edge.Node == edge2.Node && edge.Node2 == edge2.Node2) || 
                    (edge.Node == edge2.Node2 && edge.Node2 == edge2.Node));
        }

        public static bool operator !=(Edge node, Edge node2)
        {
            return !(node == node2);
        }

        private bool Equals(Edge other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Edge)obj);
        }
        
        public override int GetHashCode()
        {
            return (Nodes != null ? Nodes.GetHashCode() : 0);
        }

        public Node GetOtherNode(Node node)
        {
            return Node != node ? Node : Node2;
        }
    }
}