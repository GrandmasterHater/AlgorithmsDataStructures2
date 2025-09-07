using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures2
{
    public class Vertex<T>
    {
        public bool Hit;
        public T Value;

        public Vertex(T val)
        {
            Value = val;
            Hit = false;
        }
    }

    public class SimpleGraph<T>
    {
        public Vertex<T> [] vertex;
        public int [,] m_adjacency;
        public int max_vertex;
        
        public SimpleGraph(int size)
        {
            max_vertex = size;
            m_adjacency = new int [size,size];
            vertex = new Vertex<T> [size];
        }

        // Exercise 10, time complexity O(n^2), space complexity O(n)
        public List<Vertex<T>> DepthFirstSearch(int VFrom, int VTo)
        {
            foreach (Vertex<T> v in vertex)
            {
                v.Hit = false;
            }
            
            Stack<int> pathStack = new Stack<int>();
            pathStack = DepthFirstSearchRecursive(VFrom, VTo, pathStack);
            
            return pathStack != null 
                ? pathStack.Reverse().Select(i => vertex[i]).ToList()
                : new List<Vertex<T>>();
        }
        
        // Exercise 8, task 1, time complexity O(n), space complexity O(1)
        public void AddVertex(T value)
        {
            int emptyIndex = Array.IndexOf(vertex, null);
            
            if (emptyIndex == -1)
                throw new InvalidOperationException("Graph is full!");

            vertex[emptyIndex] = new Vertex<T>(value);
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
        
        private Stack<int> DepthFirstSearchRecursive(int VFrom, int VTo, Stack<int> pathStack)
        {
            vertex[VFrom].Hit = true;
            pathStack.Push(VFrom);
            
            if (VFrom == VTo)
                return pathStack;
            
            Stack<int> currentStack = null;
            
            for (int i = 0; i < max_vertex; ++i)
            {
                if (m_adjacency[VFrom, i] == 1 && !vertex[i].Hit)
                    currentStack = DepthFirstSearchRecursive(i, VTo, pathStack);
                
                if (currentStack != null)
                    return currentStack;
            }

            pathStack.Pop();
            return null;
        }

    }
}