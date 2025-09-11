using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures2.Task11PathfindingWithBFS
{
    public static class PathfindingWithBFS_2
    {
        #region FindMaxDistance
        
        // Exercise 11, task 2, time complexity O(n^2), space complexity O(n)
        public static int FindMaxDistance<T>(this SimpleGraph<T> graph)
        {
            if (graph.vertex.Length == 0)
                return 0;
            
            (int vertexIndex, int distance) farthestVertex =  FindMaxDistanceFromVertex(0, graph);
            (int vertexIndex, int distance) maxVertex =  FindMaxDistanceFromVertex(farthestVertex.vertexIndex, graph);
            
            return maxVertex.distance;
        }

        private static (int vertexIndex, int distance) FindMaxDistanceFromVertex<T>(int vertexIndex, SimpleGraph<T> graph)
        {
            foreach (Vertex<T> vertex in graph.vertex)
            {
                vertex.Hit = false;
            }

            (int vertexIndex, int distance) currentVertex = (vertexIndex, 0);
            Queue<(int vertexIndex, int distance)> bfsQueue = new Queue<(int vertexIndex, int distance)>();
            graph.vertex[currentVertex.vertexIndex].Hit = true;
            bfsQueue.Enqueue(currentVertex);

            while (bfsQueue.Count > 0)
            {
                currentVertex = bfsQueue.Dequeue();

                CollectAdjacentVertices(graph, currentVertex, bfsQueue);
            }

            return currentVertex;
        }
        
        private static void CollectAdjacentVertices<T>(
            SimpleGraph<T> graph, 
            (int vertexIndex, int distance) vertex, 
            Queue<(int vertexIndex, int distance)> queue)
        {
            for (int i = 0; i < graph.max_vertex; ++i)
            {
                if (graph.IsEdge(vertex.vertexIndex, i) && !graph.vertex[i].Hit)
                {
                    graph.vertex[i].Hit = true;
                    queue.Enqueue((i, vertex.distance + 1));
                }
            }
        }
        
        #endregion
        
        #region FindCycles
        
        // Exercise 11, task 3, time complexity O(n^3), space complexity O(с*n)
        public static List<List<int>> FindCycles<T>(this SimpleGraph<T> graph)
        {
            if (graph.vertex.Length == 0)
                return new List<List<int>>();
            
            List<List<int>> cycles = new List<List<int>>();

            for (var index = 0; index < graph.vertex.Length; index++)
            {
                FindCyclesFromVertex(index, graph, cycles);
            }

            RemoveDuplicates(cycles);

            return cycles;
        }
        
        public static void FindCyclesFromVertex<T>(int vertexIndex, SimpleGraph<T> graph, List<List<int>> cycles)
        {
            foreach (Vertex<T> vertex in graph.vertex)
            {
                vertex.Hit = false;
            }

            BFSVertexInfo currentVertex = new BFSVertexInfo() { VertexIndex = vertexIndex, Distance = 0, Parent = null };
            Dictionary<int, BFSVertexInfo> visited = new Dictionary<int, BFSVertexInfo>();
            Queue<BFSVertexInfo> bfsQueue = new Queue<BFSVertexInfo>();
            graph.vertex[currentVertex.VertexIndex].Hit = true;
            bfsQueue.Enqueue(currentVertex);
            visited.Add(currentVertex.VertexIndex, currentVertex);

            while (bfsQueue.Count > 0)
            {
                currentVertex = bfsQueue.Dequeue();

                HandleAdjacentVertices(graph, currentVertex, bfsQueue, cycles, visited);
            }
        }

        private static void HandleAdjacentVertices<T>(SimpleGraph<T> graph,
            BFSVertexInfo vertex,
            Queue<BFSVertexInfo> queue,
            List<List<int>> cycles, 
            Dictionary<int, BFSVertexInfo> visited)
        {
            for (int i = 0; i < graph.max_vertex; ++i)
            {
                bool hasEdge = graph.IsEdge(vertex.VertexIndex, i);
                bool hasHit = graph.vertex[i].Hit;
                
                if (hasEdge && !hasHit)
                {
                    graph.vertex[i].Hit = true;
                    BFSVertexInfo vertexInfo = new BFSVertexInfo { 
                        VertexIndex = i,
                        Distance = vertex.Distance + 1, 
                        Parent = vertex 
                    };
                    visited.Add(i, vertexInfo);
                    queue.Enqueue(vertexInfo);
                }

                if (hasEdge && hasHit && i != vertex.Parent?.VertexIndex)
                {
                    List<int> cycle = CollectCycleVertices(vertex, visited[i]);
                    cycles.Add(cycle);
                }
            }
        }

        private static List<int> CollectCycleVertices(BFSVertexInfo leftVertex, BFSVertexInfo rightVertex)
        {
            List<int> leftVertices = new List<int>();
            List<int> rightVertices = new List<int>();

            BFSVertexInfo currentLeft = leftVertex;
            BFSVertexInfo currentRight = rightVertex;

            while (currentLeft.VertexIndex != currentRight.VertexIndex)
            {
                if (currentLeft.Distance < currentRight.Distance)
                {
                    rightVertices.Add(currentRight.VertexIndex);
                    currentRight = currentRight.Parent;
                }
                else if (currentLeft.Distance > currentRight.Distance)
                {
                    leftVertices.Add(currentLeft.VertexIndex);
                    currentLeft = currentLeft.Parent;
                }
                else
                {
                    leftVertices.Add(currentLeft.VertexIndex);
                    rightVertices.Add(currentRight.VertexIndex);
                    currentLeft = currentLeft.Parent;
                    currentRight = currentRight.Parent;
                }
            }

            leftVertices.Add(currentLeft.VertexIndex);
            rightVertices.Reverse();
            leftVertices.AddRange(rightVertices);
            leftVertices.Sort();

            return leftVertices;
        }
        
        private static void RemoveDuplicates(List<List<int>> cycles)
        {
            HashSet<string> hashSet = new HashSet<string>();
            var uniqueCycles = new List<List<int>>();

            foreach (var cycle in cycles)
            {
                string key = string.Join(",", cycle);
                
                if (!hashSet.Contains(key))
                {
                    hashSet.Add(key);
                    uniqueCycles.Add(cycle);
                }
            }

            cycles.Clear();
            cycles.AddRange(uniqueCycles);
        }
        
        internal class BFSVertexInfo
        {
            public int VertexIndex { get; set; }
            public int Distance { get; set; }
            public BFSVertexInfo Parent { get; set; }
        }

        #endregion
    }
}

