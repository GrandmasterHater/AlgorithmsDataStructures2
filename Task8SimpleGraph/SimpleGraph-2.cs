using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2.Task8
{
    public class DirectedGraph
    {
        private const int NOT_VISITED = 0;
        private const int PROCESSING = 1;
        private const int VISITED = 2;

        private int[,] _adjacency;
        private int _size;

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

