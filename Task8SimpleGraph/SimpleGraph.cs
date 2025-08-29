using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AlgorithmsDataStructures2
{
    public class Vertex
    {
        public int Value;
        public Vertex(int val)
        {
            Value = val;
        }
    }
  
    public class SimpleGraph
    {
        public Vertex [] vertex;
        public int [,] m_adjacency;
        public int max_vertex;
	
        public SimpleGraph(int size)
        {
            max_vertex = size;
            m_adjacency = new int [size,size];
            vertex = new Vertex [size];
        }
	
        // Exercise 8, task 1, time complexity O(n), space complexity O(1)
        public void AddVertex(int value)
        {
            int emptyIndex = Array.IndexOf(vertex, null);
            
            if (emptyIndex == -1)
                throw new InvalidOperationException("Graph is full!");

            vertex[emptyIndex] = new Vertex(value);
        }
        
        // Exercise 8, task 2, time complexity O(n), space complexity O(1)
        public void RemoveVertex(int v)
        {
            ThrowIfOutOfRange(v);
            
            for (int i = 0; i < max_vertex; ++i)
            {
                m_adjacency[v,i] = 0;
                m_adjacency[i,v] = 0;
            }

            vertex[v] = null;
        }
	
        // Exercise 8, task 2, time complexity O(1), space complexity O(1)
        public bool IsEdge(int v1, int v2)
        {
            ThrowIfOutOfRange(v1);
            ThrowIfOutOfRange(v2);
            
            return m_adjacency[v1,v2] == 1 && m_adjacency[v2,v1] == 1;
        }
	
        // Exercise 8, task 2, time complexity O(1), space complexity O(1)
        public void AddEdge(int v1, int v2)
        {
            ThrowIfOutOfRange(v1);
            ThrowIfOutOfRange(v2);
            
            m_adjacency[v1,v2] = 1;
            m_adjacency[v2,v1] = 1;
        }
	
        // Exercise 8, task 2, time complexity O(1), space complexity O(1)
        public void RemoveEdge(int v1, int v2)
        {
            ThrowIfOutOfRange(v1);
            ThrowIfOutOfRange(v2);
            
            m_adjacency[v1,v2] = 0;
            m_adjacency[v2,v1] = 0;
        }
        
        private void ThrowIfOutOfRange(int v)
        {
            if (v < 0 || v >= max_vertex)
                throw new ArgumentOutOfRangeException("Vertex index out of range");
        }
    }
}  

