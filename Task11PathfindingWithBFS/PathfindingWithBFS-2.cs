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
        
        public static List<List<int>> FindCycles<T>(this SimpleGraph<T> graph)
        {
            foreach (Vertex<T> vertex in graph.vertex)
            {
                vertex.Hit = false;
            }
            
            List<List<int>> cycles = new List<List<int>>();

            BFSVertexInfo currentVertex = new BFSVertexInfo() { VertexIndex = 0, Distance = 0, Parent = null };
            Queue<BFSVertexInfo> bfsQueue = new Queue<BFSVertexInfo>();
            graph.vertex[currentVertex.VertexIndex].Hit = true;
            bfsQueue.Enqueue(currentVertex);

            while (bfsQueue.Count > 0)
            {
                currentVertex = bfsQueue.Dequeue();

                HandleAdjacentVertices(graph, currentVertex, bfsQueue, cycles);
            }

            return cycles;
        }
        
        private static void HandleAdjacentVertices<T>(
            SimpleGraph<T> graph, 
            BFSVertexInfo vertex, 
            Queue<BFSVertexInfo> queue,
            List<List<int>> cycles)
        {
            for (int i = 0; i < graph.max_vertex; ++i)
            {
                bool hasEdge = graph.IsEdge(vertex.vertexIndex, i);
                bool hasHit = graph.vertex[i].Hit;
                
                if (hasEdge && !hasHit)
                {
                    graph.vertex[i].Hit = true;
                    queue.Enqueue(new BFSVertexInfo {VertexIndex = i, Distance = vertex.Distance, Parent = vertex});
                }

                if (hasEdge && hasHit && i != vertex.vertexIndex)
                {
                    List<int> cycle = CollectCycleVertices(graph, vertex, i, queue);
                    cycles.Add(cycle);
                }
            }
        }

        private static List<int> CollectCycleVertices<T>(SimpleGraph<T> graph, BFSVertexInfo leftVertex, BFSVertexInfo rightVertex, Queue<BFSVertexInfo> queue)
        {
            List<int> vertices = new List<int>();

            BFSVertexInfo currentLeft, currentRight;

            for (currentLeft = leftVertex, currentRight = rightVertex, leftVertex = nextLeft, rightVertex = nextRight;
                 currentLeft.VertexIndex != currentRight.VertexIndex;
                 currentLeft = nextLeft, currentRight = nextRight)
            {
                if (currentLeft.Distance < currentRight.Distance)
                {
                    nextRight = currentRight.Parent;
                    vertices.Insert(vertices.Length - 1, currentRight.VertexIndex);
                }
                else if (currentLeft.Distance > currentRight.Distance)
                {
                    nextLeft = currentLeft.Parent;
                    vertices.Insert(0, currentLeft.VertexIndex);
                }
                else
                {
                    nextLeft = currentLeft.Parent;
                    nextRight = currentRight.Parent;
                    vertices.Insert(0, currentLeft.VertexIndex);
                    vertices.Insert(vertices.Length - 1, currentRight.VertexIndex);
                }
            }

            vertices.Insert(0, currentLeft.VertexIndex);

            return vertices;
        }

        #endregion
    }
    
    internal class BFSVertexInfo
    {
        public int VertexIndex { get; set; }
        public int Distance { get; set; }
        public BFSVertexInfo Parent { get; set; }
    }
}

