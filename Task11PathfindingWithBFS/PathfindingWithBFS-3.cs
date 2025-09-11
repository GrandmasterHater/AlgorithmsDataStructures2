using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmsDataStructures2.Task11PathfindingWithBFS
{
    [TestFixture]
    public class PathfindingWithBFS_3
    {
        #region AddVertex
        
        [Test]
        public void AddVertex_WhenGraphHasFreeSlot_VertexIsAdded()
        {
            var graph = new SimpleGraph<int>(2);

            graph.AddVertex(10);

            Assert.That(graph.vertex[0].Value, Is.EqualTo(10));
            Assert.That(graph.m_adjacency[0, 0], Is.EqualTo(0));
            Assert.That(graph.m_adjacency[0, 1], Is.EqualTo(0));
            Assert.That(graph.m_adjacency[1, 0], Is.EqualTo(0));
        }

        [Test]
        public void AddVertex_WhenGraphIsFull_ThrowsException()
        {
            var graph = new SimpleGraph<int>(2);
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
            var graph = new SimpleGraph<int>(2);
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
            var graph = new SimpleGraph<int>(2);

            Assert.That(() => graph.RemoveVertex(5), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        #endregion

        #region AddEdge
        
        [Test]
        public void AddEdge_WhenVerticesExist_EdgeIsAdded()
        {
            var graph = new SimpleGraph<int>(2);
            
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
            var graph = new SimpleGraph<int>(2);
            graph.AddVertex(1);

            Assert.That(() => graph.AddEdge(0, 5),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        #endregion

        #region RemoveEdge
        
        [Test]
        public void RemoveEdge_WhenEdgeExists_EdgeIsRemoved()
        {
            var graph = new SimpleGraph<int>(2);
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
            var graph = new SimpleGraph<int>(2);
            graph.AddVertex(1);

            Assert.That(() => graph.RemoveEdge(0, 10),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        #endregion
        
        #region IsEdge
        
        [Test]
        public void IsEdge_WhenEdgeExists_ReturnsTrue()
        {
            var graph = new SimpleGraph<int>(2);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(0, 1);

            Assert.That(graph.IsEdge(0, 1), Is.True);
        }

        [Test]
        public void IsEdge_WhenNoEdge_ReturnsFalse()
        {
            var graph = new SimpleGraph<int>(2);
            graph.AddVertex(1);
            graph.AddVertex(2);

            Assert.That(graph.IsEdge(0, 1), Is.False);
        }

        [Test]
        public void IsEdge_WhenIndexIsOutOfRange_ThrowsException()
        {
            var graph = new SimpleGraph<int>(2);
            graph.AddVertex(1);

            Assert.That(() => graph.IsEdge(0, 10), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        #endregion
        
        #region DepthFirstSearch

        [Test]
        public void DepthFirstSearch_PathExists_ReturnsCorrectPath()
        {
            SimpleGraph<int> graph = new SimpleGraph<int>(4);
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            
            Assert.That(graph.DepthFirstSearch(0, 3).Select(v => v.Value).ToList(), Is.EqualTo(new[] {0, 1, 2, 3}));
        }

        [Test]
        public void DepthFirstSearch_NoPath_ReturnsEmptyList()
        {
            var graph = new SimpleGraph<int>(4);
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(0, 1);
            graph.AddEdge(2, 3);
            
            Assert.That(graph.DepthFirstSearch(0, 3).Select(v => v.Value).ToList(), Is.Empty);
        }

        [Test]
        public void DepthFirstSearch_FromVertexToItself_ReturnsSingleVertex()
        {
            SimpleGraph<int> graph = new SimpleGraph<int>(4);
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            
            Assert.That(graph.DepthFirstSearch(2, 2).Select(v => v.Value).ToList(), Is.EqualTo(new[] {2}));
        }

        #endregion

        #region BreadthFirstSearch

        [Test]
        public void BreadthFirstSearch_PathExists_ReturnsCorrectPath()
        {
            SimpleGraph<int> graph = new SimpleGraph<int>(4);
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            
            Assert.That(graph.BreadthFirstSearch(0, 3).Select(v => v.Value).ToList(), Is.EqualTo(new[] {0, 1, 2, 3}));
        }

        [Test]
        public void BreadthFirstSearch_NoPath_ReturnsEmptyList()
        {
            var graph = new SimpleGraph<int>(4);
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(0, 1);
            graph.AddEdge(2, 3);
            
            Assert.That(graph.BreadthFirstSearch(0, 3).Select(v => v.Value).ToList(), Is.Empty);
        }

        [Test]
        public void BreadthFirstSearch_FromVertexToItself_ReturnsSingleVertex()
        {
            SimpleGraph<int> graph = new SimpleGraph<int>(4);
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            
            Assert.That(graph.BreadthFirstSearch(2, 2).Select(v => v.Value).ToList(), Is.EqualTo(new[] {2}));
        }

        #endregion

        #region FindMaxDistance

        [Test]
        public void FindMaxDistance_EmptyGraph_ReturnsZero()
        {
            var graph = new SimpleGraph<int>(0);
            Assert.That(graph.FindMaxDistance(), Is.EqualTo(0));
        }

        [Test]
        public void FindMaxDistance_SingleNodeGraph_ReturnsZero()
        {
            var graph = new SimpleGraph<int>(1);
            graph.AddVertex(1);
            Assert.That(graph.FindMaxDistance(), Is.EqualTo(0));
        }

        [Test]
        public void FindMaxDistance_GraphWith15Nodes_ReturnsCorrectDiameter()
        {
            var graph = new SimpleGraph<int>(15);
            
            for (int i = 0; i < 15; i++)
                graph.AddVertex(i);
            
            //       0
            //      / \
            //     1   2
            //    /|   |\
            //   3 4   5 6
            //  /|      \
            // 7 8       9
            //          / \
            //         10 11
            //         12
            //         13
            //         14
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(1, 4);
            graph.AddEdge(2, 5);
            graph.AddEdge(2, 6);
            graph.AddEdge(3, 7);
            graph.AddEdge(3, 8);
            graph.AddEdge(5, 9);
            graph.AddEdge(9, 10);
            graph.AddEdge(9, 11);
            graph.AddEdge(10, 12);
            graph.AddEdge(12, 13);
            graph.AddEdge(13, 14);

            Assert.That(graph.FindMaxDistance(), Is.EqualTo(10));
        }

        #endregion

        #region FindCycles

        [Test]
        public void FindCycles_WhenGraphIsEmpty_ReturnsEmptyList()
        {
            var graph = new SimpleGraph<int>(0);

            var cycles = graph.FindCycles();

            Assert.That(cycles, Is.Empty);
        }

        [Test]
        public void FindCycles_WhenGraphHasNoCycles_ReturnsEmptyList()
        {
            var graph = new SimpleGraph<int>(3);

            for (int i = 0; i < 3; i++)
                graph.AddVertex(i);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);

            var cycles = graph.FindCycles();

            Assert.That(cycles, Is.Empty);
        }

        [Test]
        public void FindCycles_WhenGraphHasOneCycle_ReturnsOneCycle()
        {
            var graph = new SimpleGraph<int>(3);
            for (int i = 0; i < 3; i++) 
                graph.AddVertex(i);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);
            int expectedCyclesCount = 1;

            var cycles = graph.FindCycles();

            Assert.That(cycles.Count, Is.EqualTo(expectedCyclesCount));
            Assert.That(cycles[0], Does.Contain(0).And.Contain(1).And.Contain(2));
        }

        [Test]
        public void FindCycles_WhenGraphHasMultipleCycles_ReturnsAllCycles()
        {
            var graph = new SimpleGraph<int>(5);
            for (int i = 0; i < 5; i++) graph.AddVertex(i);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 0);
            graph.AddEdge(0, 2);
            graph.AddEdge(3, 4);
            graph.AddEdge(2, 4);
            int expectedCyclesCount = 6;

            var cycles = graph.FindCycles();

            Assert.That(cycles.Count, Is.EqualTo(expectedCyclesCount));
            Assert.That(cycles, Has.Some.Matches<List<int>>(c => c.Contains(0) && c.Contains(1) && c.Contains(2)));
            Assert.That(cycles, Has.Some.Matches<List<int>>(c => c.Contains(0) && c.Contains(2) && c.Contains(3)));
        }
        
        [Test]
        public void FindCycles_WhenGraphHasTwoDisconnectedTriangles_ReturnsTwoCycles()
        {
            var graph = new SimpleGraph<int>(6);
            for (int i = 0; i < 6; i++) graph.AddVertex(i);

            // Первый треугольник 0-1-2-0
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);

            // Второй треугольник 3-4-5-3
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 3);
            int expectedCyclesCount = 2;

            var cycles = graph.FindCycles();

            Assert.That(cycles.Count, Is.EqualTo(expectedCyclesCount));
            Assert.That(cycles, Has.Some.Matches<List<int>>(c => c.Contains(0) && c.Contains(1) && c.Contains(2)));
            Assert.That(cycles, Has.Some.Matches<List<int>>(c => c.Contains(3) && c.Contains(4) && c.Contains(5)));
        }

        #endregion
    }
}

