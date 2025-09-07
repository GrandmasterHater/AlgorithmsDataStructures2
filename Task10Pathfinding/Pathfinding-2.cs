using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures2.Task10Pathfinding
{
    public static class Pathfinding_2
    {
        // Exercise 10, task 1, time complexity O(n^2), space complexity O(h) where h - call stack height
        public static bool IsConnectedGraph<T>(this SimpleGraph<T> graph)
        {
            if (graph.vertex.Length == 0)
                return false;
            
            foreach (Vertex<T> vertex in graph.vertex)
            {
                vertex.Hit = false;
            }

            int connectedCount = ConnectedVerticesRecursive(graph, 0, 0);
            
            return connectedCount == graph.vertex.Length;
        }

        // Exercise 10, task 2, time complexity O(n!), space complexity O(n)
        public static int GetLongestSimpleWayLength(this DirectedGraph graph)
        {
            HashSet<int> visited = new HashSet<int>();
            int maxLength = 0;

            for (int i = 0; i < graph.Size; ++i)
            {
                int length = GetLongestSimpleWayLengthRecursive(graph, i, visited, 0);
                maxLength = Math.Max(maxLength, length);
            }
            
            return maxLength;
        }

        private static int GetLongestSimpleWayLengthRecursive(DirectedGraph graph, int vertexIndex, HashSet<int> visited, int currentLength)
        {
            int maxLength = currentLength;
            visited.Add(vertexIndex);
            
            for (int j = 0; j < graph.Size; ++j)
            {
                if (graph[vertexIndex, j] == 1 && !visited.Contains(j))
                    maxLength = Math.Max(maxLength, GetLongestSimpleWayLengthRecursive(graph, j, visited, currentLength + 1));
            }

            visited.Remove(vertexIndex);
            
            return maxLength;
        }

        private static int ConnectedVerticesRecursive<T>(SimpleGraph<T> graph, int vertexIndex, int connectedCount)
        {
            graph.vertex[vertexIndex].Hit = true;
            ++connectedCount;

            for (int i = 0; i < graph.max_vertex; ++i)
            {
                if (graph.m_adjacency[vertexIndex, i] == 1 && !graph.vertex[i].Hit)
                    connectedCount = ConnectedVerticesRecursive(graph, i, connectedCount);
            }
            
            return connectedCount;
        }
    }
    
    public class DirectedGraph
    {
        private const int NOT_VISITED = 0;
        private const int PROCESSING = 1;
        private const int VISITED = 2;

        private int[,] _adjacency;
        private int _size;

        public int this [int i, int j] => _adjacency[i, j];
        public int Size => _size;

        public DirectedGraph(int[,] adjacency)
        {
            _adjacency = adjacency;
            _size = adjacency.GetLength(0);
        }

        // Exercise 8, task 2, time complexity O(n^2), space complexity O(n)
        public bool IsCyclic()
        {
            int[] vertexStates = new int[_size];

            for (int i = 0; i < _size; ++i)
            {
                if (vertexStates[i] == NOT_VISITED && HasCycleForVertexRecursive(i, vertexStates))
                    return true;
            }

            return false;
        }

        private bool HasCycleForVertexRecursive(int vertexIndex, int[] vertexStates)
        {
            vertexStates[vertexIndex] = PROCESSING;

            for (int j = 0; j < _size; ++j)
            {
                if (_adjacency[vertexIndex, j] == 1 && vertexStates[j] == PROCESSING)
                    return true;

                if (_adjacency[vertexIndex, j] == 1 && vertexStates[j] == NOT_VISITED && HasCycleForVertexRecursive(j, vertexStates))
                    return true;
            }
            
            vertexStates[vertexIndex] = VISITED;
            return false;
        }
    }
}