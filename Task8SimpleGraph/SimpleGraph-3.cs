using System;
using NUnit.Framework;

namespace AlgorithmsDataStructures2.Task8SimpleGraph
{
    [TestFixture]
    public class SimpleGraph_3
    {
        #region AddVertex
        
        [Test]
        public void AddVertex_WhenGraphHasFreeSlot_VertexIsAdded()
        {
            var graph = new SimpleGraph(2);

            graph.AddVertex(10);

            Assert.That(graph.vertex[0].Value, Is.EqualTo(10));
            Assert.That(graph.m_adjacency[0, 0], Is.EqualTo(0));
            Assert.That(graph.m_adjacency[0, 1], Is.EqualTo(0));
            Assert.That(graph.m_adjacency[1, 0], Is.EqualTo(0));
        }

        [Test]
        public void AddVertex_WhenGraphIsFull_ThrowsException()
        {
            var graph = new SimpleGraph(2);
            graph.AddVertex(1);
            graph.AddVertex(2);

            Assert.That(() => graph.AddVertex(3), Throws.TypeOf<InvalidOperationException>());
        }
        
        #endregion

        #region RemoveVertex
        
        [TestCase(0)]
        [TestCase(1)]
        public void RemoveVertex_WhenVertexExists_VertexRemovedAndEdgesCleared(int index)
        {
            var graph = new SimpleGraph(2);
            graph.AddVertex(100);
            graph.AddVertex(200);
            graph.AddEdge(0, 1);
            
            int beforeRemoveValueFirst = graph.m_adjacency[0, 1];
            int beforeRemoveValueSecond = graph.m_adjacency[1, 0];

            graph.RemoveVertex(index);

            Assert.That(beforeRemoveValueFirst, Is.EqualTo(1));
            Assert.That(beforeRemoveValueSecond, Is.EqualTo(1));
            Assert.That(graph.vertex[index], Is.Null);
            Assert.That(graph.m_adjacency[index, 0], Is.Zero);
            Assert.That(graph.m_adjacency[0, index], Is.Zero);
        }

        [Test]
        public void RemoveVertex_WhenIndexIsOutOfRange_ThrowsException()
        {
            var graph = new SimpleGraph(2);

            Assert.That(() => graph.RemoveVertex(5), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        #endregion

        #region AddEdge
        
        [Test]
        public void AddEdge_WhenVerticesExist_EdgeIsAdded()
        {
            var graph = new SimpleGraph(2);
            
            int beforeAddValueFirst = graph.m_adjacency[0, 1];
            int beforeAddValueSecond = graph.m_adjacency[1, 0];
            
            graph.AddVertex(1);
            graph.AddVertex(2);

            graph.AddEdge(0, 1);

            Assert.That(beforeAddValueFirst, Is.Zero);
            Assert.That(beforeAddValueSecond, Is.Zero);
            Assert.That(graph.m_adjacency[0, 1], Is.EqualTo(1));
            Assert.That(graph.m_adjacency[1, 0], Is.EqualTo(1));
        }

        [Test]
        public void AddEdge_WhenIndexIsOutOfRange_ThrowsException()
        {
            var graph = new SimpleGraph(2);
            graph.AddVertex(1);

            Assert.That(() => graph.AddEdge(0, 5),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        #endregion

        #region RemoveEdge
        
        [Test]
        public void RemoveEdge_WhenEdgeExists_EdgeIsRemoved()
        {
            var graph = new SimpleGraph(2);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(0, 1);
            
            int beforeRemoveValueFirst = graph.m_adjacency[0, 1];
            int beforeRemoveValueSecond = graph.m_adjacency[1, 0];

            graph.RemoveEdge(0, 1);

            Assert.That(beforeRemoveValueFirst, Is.EqualTo(1));
            Assert.That(beforeRemoveValueSecond, Is.EqualTo(1));
            Assert.That(graph.m_adjacency[0, 1], Is.Zero);
            Assert.That(graph.m_adjacency[1, 0], Is.Zero);
        }

        [Test]
        public void RemoveEdge_WhenIndexIsOutOfRange_ThrowsException()
        {
            var graph = new SimpleGraph(2);
            graph.AddVertex(1);

            Assert.That(() => graph.RemoveEdge(0, 10),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        #endregion


        #region IsEdge
        
        [Test]
        public void IsEdge_WhenEdgeExists_ReturnsTrue()
        {
            var graph = new SimpleGraph(2);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(0, 1);

            Assert.That(graph.IsEdge(0, 1), Is.True);
        }

        [Test]
        public void IsEdge_WhenNoEdge_ReturnsFalse()
        {
            var graph = new SimpleGraph(2);
            graph.AddVertex(1);
            graph.AddVertex(2);

            Assert.That(graph.IsEdge(0, 1), Is.False);
        }

        [Test]
        public void IsEdge_WhenIndexIsOutOfRange_ThrowsException()
        {
            var graph = new SimpleGraph(2);
            graph.AddVertex(1);

            Assert.That(() => graph.IsEdge(0, 10), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        #endregion
        
        #region IsCyclic
        
        [TestCase(new int[] { 0, 1, 0,   
                                       0, 0, 1,   
                                       1, 0, 0 }, TestName = "SimpleTriangleCycle")]
        
        [TestCase(new int[] { 1 }, TestName = "SelfLoopCycle")]
        
        [TestCase(new int[] { 0, 1, 0, 0,  
                                       0, 0, 1, 0,   
                                       0, 0, 0, 1,   
                                       0, 1, 0, 0 }, TestName = "CycleNotFromFirstVertex")]
        
        [TestCase(new int[] { 0, 1, 0, 0, 0,   
                                       0, 0, 1, 0, 0,   
                                       0, 0, 0, 0, 1,   
                                       0, 1, 1, 0, 0,   
                                       1, 0, 0, 1, 0 }, TestName = "CycleInDisconnectedGraph")]
        
        [TestCase(new int[] { 0, 1, 
                                       1, 0 }, TestName = "TwoVertexCycle")]
        public void IsCyclic_WhenGraphHasCycle_ReturnTrue(int[] matrixFlat)
        {
            DirectedGraph graph = CreateDirectedGraphFromMatrix(matrixFlat);

            Assert.That(graph.IsCyclic(), Is.True, "Expected cycle to be detected in graph");
        }

        [TestCase(new int[] { 0, 1, 0, 
                                       0, 0, 1, 
                                       0, 0, 0 }, TestName = "SimpleLinearPath")]
        
        [TestCase(new int[] { 0, 0, 0,
                                       0, 0, 0, 
                                       0, 0, 0 }, TestName = "GraphWithNoEdges")]
        
        [TestCase(new int[] { 0, 1, 1, 0, 
                                       0, 0, 0, 1, 
                                       0, 0, 0, 1, 
                                       0, 0, 0, 0 }, TestName = "DiamondShapedAcyclicGraph")]
        
        [TestCase(new int[] { 0, 1, 0, 0, 0, 0, 
                                       0, 0, 1, 0, 0, 0, 
                                       0, 0, 0, 1, 0, 0, 
                                       0, 0, 0, 0, 0, 1,
                                       0, 0, 0, 0, 0, 0, 
                                       0, 0, 0, 0, 0, 0 }, TestName = "TwoSeparateAcyclicChains")]
        
        [TestCase(new int[] { 0, 1, 1, 1, 
                                       0, 0, 0, 0, 
                                       0, 0, 0, 0, 
                                       0, 0, 0, 0 }, TestName = "StarShapedGraph")]
        
        [TestCase(new int[] { 0, 1, 
                                       0, 0 }, TestName = "SingleEdgeGraph")]
        public void IsCyclic_WhenGraphHasNoCycle_ReturnFalse(int[] matrixFlat)
        {
            DirectedGraph graph = CreateDirectedGraphFromMatrix(matrixFlat);
            
            Assert.That(graph.IsCyclic(), Is.False, "Expected no cycle in graph");
        }
        
        #endregion

        private DirectedGraph CreateDirectedGraphFromMatrix(int[] matrixFlat)
        {
            int size = (int)Math.Sqrt(matrixFlat.Length);
            int[,] graphMatrix = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrixFlat[i * size + j] == 1)
                    {
                        graphMatrix[i, j] = 1;
                    }
                }
            }

            return new DirectedGraph(graphMatrix);
        }
    }
}

