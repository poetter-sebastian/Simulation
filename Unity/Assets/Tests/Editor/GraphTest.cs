using NUnit.Framework;
using UnityEngine;
using World.Structure;
using Random = System.Random;

namespace Tests.Editor
{
    public class GraphTest
    {
        [Test]
        public void AddNode()
        {
            var graph = new Graph();
            var test = graph.AddNodes(new []{new Node(graph.NextID(), Vector3.up), 
                new Node(graph.NextID(), Vector3.zero), 
                new Node(graph.NextID(), Vector3.up)});
            Assert.AreEqual(2, graph.NodeValues.Count);
            Assert.False(test);
        }
        
        [Test]
        public void CompareNodes()
        {
            var graph = new Graph();
            var rand = new Random();
            var node1 = new Node(graph.NextID(), new Vector3(10, 10, 10));

            for (var i = 0; i < 1000; i++)
            {
                var random = rand.Next(0, 1000);
                var vec = new Vector3(random, random, random);
                var node2 = new Node(graph.NextID(), vec);
                switch (random)
                {
                    case > 10:
                        Assert.True(node1 < node2);
                        break;
                    case 10:
                        Assert.True(node1.Pos == node2.Pos);
                        break;
                    default:
                        Assert.True(node1 > node2);
                        break;
                }
            }
        }
        
        [Test]
        public void AddEdge()
        {
            var graph = new Graph();
            var node1 = new Node(graph.NextID(), Vector3.zero);
            var node2 = new Node(graph.NextID(), Vector3.up);
            graph.AddEdge(node1, node2);
            graph.AddEdge(node2, node1);
            Assert.AreEqual(1, graph.EdgeKeys.Count);
        }
        
        [Test]
        public void CompareEdges()
        {
            var graph = new Graph();
            var node1 = new Node(graph.NextID(), Vector3.zero);
            var node2 = new Node(graph.NextID(), Vector3.up);
            var node3 = new Node(graph.NextID(), Vector3.right);
            
            var edge1 = new Edge(node1, node2); 
            Assert.True(graph.AddEdge(node1, node2));
            
            var edge2 = new Edge(node2, node1);
            Assert.False(graph.AddEdge(node2, node1));
            
            var edge3 = new Edge(node2, node3);
            Assert.True(graph.AddEdge(node2, node3));
            
            Assert.AreEqual(2, graph.EdgeKeys.Count);
            
            Assert.True(edge1 == edge2);
            Assert.False(edge1 == edge3);
        }

        [Test]
        public void CreateGraph()
        {
            var graph = new Graph();
            var size = new Vector2Int(100, 100);
            
            for (var i = 0; i < size.x; i += 2)
            {
                for (var j = 0; j < size.y; j += 2)
                {
                    graph.AddNode(new Vector3(i, 0, j));
                    graph.AddNode(new Vector3(i+1, 0, j));
                    graph.AddNode(new Vector3(i, 0, j+1));
                    graph.AddNode(new Vector3(i+1, 0, j+1));
                    if (i > 0 && j > 0)
                    {
                        graph.AddEdge(new Vector3(i, 0, j), new Vector3(i-1, 0, j));
                        graph.AddEdge(new Vector3(i, 0, j), new Vector3(i, 0, j-1));
                        graph.AddEdge(new Vector3(i, 0, j+1), new Vector3(i-1, 0, j+1));
                        graph.AddEdge(new Vector3(i+1, 0, j), new Vector3(i+1, 0, j-1));
                    }
                    else if(i > 0)
                    {
                        graph.AddEdge(new Vector3(i, 0, j), new Vector3(i-1, 0, j));
                        graph.AddEdge(new Vector3(i, 0, j+1), new Vector3(i-1, 0, j+1));
                    }
                    else if(j > 0)
                    {
                        graph.AddEdge(new Vector3(i, 0, j), new Vector3(i, 0, j-1));
                        graph.AddEdge(new Vector3(i+1, 0, j), new Vector3(i+1, 0, j-1));
                    }
                    graph.AddEdge(new Vector3(i, 0, j), new Vector3(i+1, 0, j));
                    graph.AddEdge(new Vector3(i, 0, j), new Vector3(i, 0, j+1));
                    graph.AddEdge(new Vector3(i+1, 0, j+1), new Vector3(i+1, 0, j));
                    graph.AddEdge(new Vector3(i+1, 0, j+1), new Vector3(i, 0, j+1));
                }
            }
            Assert.AreEqual(size.x*size.y, graph.NodeValues.Count);
        }
    }
}