using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsDataStructures2.Task11;
using NUnit.Framework;

namespace AlgorithmsDataStructures2
{
    [TestFixture]
    public class GraphsWithTriangles_3
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
        
        #region WeakVertices

        [Test]
        public void WeakVertices_WhenGraphIsEmpty_ReturnsEmptyList()
        {
            var graph = new SimpleGraph<int>(3);

            Assert.That(graph.WeakVertices(), Is.Empty);
        }

        [Test]
        public void WeakVertices_WhenGraphHasNoTriangles_ReturnsAllVertices()
        {
            var graph = new SimpleGraph<int>(3);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);

            var result = graph.WeakVertices();

            Assert.That(graph.WeakVertices().Select(v => v.Value), Is.EquivalentTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void WeakVertices_WhenGraphHasTriangleAndLooseVertices_ReturnsOnlyLooseVertices()
        {
            var graph = new SimpleGraph<int>(5);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddVertex(5);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);

            graph.AddEdge(3, 4);

            var result = graph.WeakVertices();

            Assert.That(result.Select(v => v.Value), Is.EquivalentTo(new[] { 4, 5 }));
        }

        #endregion

        #region GetTrianglesCount

        [Test]
        public void GetTrianglesCount_WhenGraphIsEmpty_ReturnsZero()
        {
            var graph = new SimpleGraph<int>(3);

            Assert.That(graph.GetTrianglesCount(), Is.EqualTo(0));
        }

        [Test]
        public void GetTrianglesCount_WhenGraphHasNoTriangles_ReturnsZero()
        {
            var graph = new SimpleGraph<int>(3);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);

            Assert.That(graph.GetTrianglesCount(), Is.EqualTo(0));
        }

        [Test]
        public void GetTrianglesCount_WhenGraphHasOneTriangleAndLoosePair_ReturnsOne()
        {
            var graph = new SimpleGraph<int>(5);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddVertex(5);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);

            graph.AddEdge(3, 4);

            Assert.That(graph.GetTrianglesCount(), Is.EqualTo(1));
        }

        [TestCaseSource(nameof(MultipleTrianglesCases))]
        public int GetTrianglesCount_WhenGraphHasDifferentTrianglesCases_ReturnsExpected(SimpleGraph<int> graph)
        {
            return graph.GetTrianglesCount();
        }
        
        public static IEnumerable<TestCaseData> MultipleTrianglesCases()
        {
            yield return new TestCaseData(
                    BuildGraphWithTrianglesCase1()
                ).SetName("GetTrianglesCount_WhenGraphHasOneTriangle_ReturnsOne")
                .Returns(1);

            yield return new TestCaseData(
                    BuildGraphWithTrianglesCase2()
                ).SetName("GetTrianglesCount_WhenGraphHasTwoDisjointTriangles_ReturnsTwo")
                .Returns(2);

            yield return new TestCaseData(
                    BuildGraphWithTrianglesCase3()
                ).SetName("GetTrianglesCount_WhenGraphHasTwoTrianglesWithCommonEdge_ReturnsTwo")
                .Returns(2);
        }

        private static SimpleGraph<int> BuildGraphWithTrianglesCase1()
        {
            var graph = new SimpleGraph<int>(3);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);

            return graph;
        }

        private static SimpleGraph<int> BuildGraphWithTrianglesCase2()
        {
            var graph = new SimpleGraph<int>(6);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddVertex(5);
            graph.AddVertex(6);

            // Первый треугольник: 1-2-3
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);

            // Второй треугольник: 4-5-6
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 3);

            return graph;
        }

        private static SimpleGraph<int> BuildGraphWithTrianglesCase3()
        {
            var graph = new SimpleGraph<int>(4);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);

            // Первый треугольник: 1-2-3
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);

            // Второй треугольник: 2-3-4 (общая грань 2-3)
            graph.AddEdge(1, 3);
            graph.AddEdge(2, 3);

            return graph;
        }

        #endregion
        
        #region WeakVerticesByInterface

        [Test]
        public void WeakVerticesByInterface_WhenGraphIsEmpty_ReturnsEmptyList()
        {
            var graph = new SimpleGraph<int>(3);

            Assert.That(graph.WeakVerticesByInterface(), Is.Empty);
        }

        [Test]
        public void WeakVerticesByInterface_WhenGraphHasNoTriangles_ReturnsAllVertices()
        {
            var graph = new SimpleGraph<int>(3);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);

            var result = graph.WeakVerticesByInterface();

            Assert.That(graph.WeakVertices().Select(v => v.Value), Is.EquivalentTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void WeakVerticesByInterface_WhenGraphHasTriangleAndLooseVertices_ReturnsOnlyLooseVertices()
        {
            var graph = new SimpleGraph<int>(5);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddVertex(5);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 0);

            graph.AddEdge(3, 4);

            var result = graph.WeakVerticesByInterface();

            Assert.That(result.Select(v => v.Value), Is.EquivalentTo(new[] { 4, 5 }));
        }

        #endregion
    }
}