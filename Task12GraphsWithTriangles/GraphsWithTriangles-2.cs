using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public static class GraphsWithTriangles_2
    {
        #region GetTrianglesCount
        
        // Exercise 12, task 1, time complexity O(n^3), space complexity O(n)
        public static int GetTrianglesCount<T>(this SimpleGraph<T> graph)
        {
            List<int> adjacentVertices = new List<int>();
            HashSet<string> uniqueTriangles = new HashSet<string>();

            for (int i = 0; i < graph.max_vertex; ++i)
            {
                adjacentVertices.Clear();
                CollectAdjacentVertices(graph, i, adjacentVertices);

                CalculateTrianglesCountForVertex(graph, i, adjacentVertices, uniqueTriangles);
            }
            
            return uniqueTriangles.Count;
        }

        private static void CalculateTrianglesCountForVertex<T>(
            SimpleGraph<T> graph, 
            int vertexIndex,
            List<int> adjacentVertices, 
            HashSet<string> uniqueTriangles)
        {
            int[] triangle = new int[3];
            
            for (int i = 0; i < adjacentVertices.Count - 1; i++)
            {
                for (int j = i + 1; j < adjacentVertices.Count; j++)
                {
                    if (graph.IsEdge(adjacentVertices[i], adjacentVertices[j]))
                    {
                        triangle[0] = vertexIndex;
                        triangle[1] = adjacentVertices[i];
                        triangle[2] = adjacentVertices[j];
                        Array.Sort(triangle);
                        
                        uniqueTriangles.Add($"{triangle[0]};{triangle[1]};{triangle[2]}");  
                    }
                }
            }
        }
        
        #endregion

        #region WeakVerticesByInterface

        // Exercise 12, task 2, time complexity O(n^3), space complexity O(n)
        public static List<Vertex<T>> WeakVerticesByInterface<T>(this SimpleGraph<T> graph)
        {
            List<Vertex<T>> weakVertices = new List<Vertex<T>>();
            List<int> adjacentVertices = new List<int>();

            for (int i = 0; i < graph.Size; ++i)
            {
                adjacentVertices.Clear();
                CollectAdjacentVertices(graph, i, adjacentVertices);
                 
                if (adjacentVertices.Count > 0 && !IsInTriangle(graph, adjacentVertices))
                    weakVertices.Add(graph[i]);
            }
            
            return weakVertices;
        }
        
        private static bool IsInTriangle<T>(SimpleGraph<T> graph, List<int> adjacentVertices)
        {
            for (int i = 0; i < adjacentVertices.Count - 1; i++)
            {
                for (int j = i + 1; j < adjacentVertices.Count; j++)
                {
                    if (graph.IsEdge(adjacentVertices[i], adjacentVertices[j]))
                        return true;
                }
            }

            return false;
        }

        #endregion
        
        private static void CollectAdjacentVertices<T>(SimpleGraph<T> graph, int currentVertexIndex, List<int> adjacentVertices)
        {
            for (int i = 0; i < graph.max_vertex; ++i)
            {
                if (graph.IsEdge(currentVertexIndex, i))
                    adjacentVertices.Add(i);
            }
        }
    }
}