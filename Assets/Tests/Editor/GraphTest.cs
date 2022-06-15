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
            Assert.AreEqual(2, graph.Notes().Count);
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
                if (random > 10)
                {
                    Assert.True(node1 < node2);
                }
                else
                {
                    Assert.True(node1 > node2);
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
            Assert.AreEqual(1, graph.Edges().Count);
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
            
            Assert.AreEqual(2, graph.Edges().Count);
            
            Assert.True(edge1 == edge2);
            Assert.False(edge1 == edge3);
        }
    }
}