using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures2.TreeTraversalOrdersTask3
{
    public static class TTO_2
    {
        #region InvertTree
        
        public static void InvertTree(this BST<int> tree, int inversionType)
        {
            if (tree.RootNode == null || tree.RootNode.IsLeaf)
                return;
            
            Queue<BSTNode<int>> nodesForInversion = new Queue<BSTNode<int>>();
            nodesForInversion.Enqueue(tree.RootNode);

            switch (inversionType)
            {
                case 0:
                    InvertTreeWthBFS(tree);
                    break;
                case 1:
                    InvertTreeWthDFS(tree);
                    break;
                
                default: 
                    throw new ArgumentException("Invalid inversion type!");
            }
        }

        // Exercise 3, task 3, time complexity O(n), space complexity O(w) where w - tree width
        private static void InvertTreeWthBFS(BST<int> tree)
        {
            Queue<BSTNode<int>> nodesForInversion = new Queue<BSTNode<int>>();
            nodesForInversion.Enqueue(tree.RootNode);

            while (nodesForInversion.Count > 0)
            {
                BSTNode<int> node = nodesForInversion.Dequeue();
                
                if(node.LeftChild != null)
                    nodesForInversion.Enqueue(node.LeftChild);
                
                if(node.RightChild != null)
                    nodesForInversion.Enqueue(node.RightChild);

                InvertNode(node);
            }
        }
        
        // Exercise 3, task 3, time complexity O(n), space complexity O(h) where h - tree height
        private static void InvertTreeWthDFS(BST<int> tree)
        {
            InvertTreeWthDFSRecursive(tree.RootNode);
        }

        private static void InvertTreeWthDFSRecursive(BSTNode<int> node)
        {
            if (node == null)
                return;
            
            InvertNode(node);

            InvertTreeWthDFSRecursive(node.LeftChild);
            InvertTreeWthDFSRecursive(node.RightChild);
        }

        private static void InvertNode(BSTNode<int> node)
        {
            BSTNode<int> leftNode = node.LeftChild;
            node.LeftChild = node.RightChild;
            node.RightChild = leftNode;
        }

        #endregion

        #region GetLevelWithMaxSumValue

        // Exercise 3, task 4, time complexity O(n), space complexity O(w) where w - tree width
        public static int GetLevelWithMaxSumValue(this BST<int> tree)
        {
            if (tree.RootNode == null)
                return 0;
            
            long maxSum = 0;
            int levelWithMaxSum = 0;
            
            Queue<BSTNode<int>> currentLevelNodes = new Queue<BSTNode<int>>();
            currentLevelNodes.Enqueue(tree.RootNode);

            for (int currentLevel = 0; currentLevelNodes.Count > 0; ++currentLevel)
            {
                long currentLevelSum = currentLevelNodes.Sum(node => node.NodeValue);

                if (currentLevelSum > maxSum)
                {
                    maxSum = currentLevelSum;
                    levelWithMaxSum = currentLevel;
                }

                currentLevelNodes = GetNextLevelNodes(currentLevelNodes);
            }

            return levelWithMaxSum;
        }

        private static Queue<BSTNode<int>> GetNextLevelNodes(Queue<BSTNode<int>> currentLevelNodes)
        {
            int nodesCount = currentLevelNodes.Count;
            
            for (var handledNodesCount = 0; handledNodesCount < nodesCount; handledNodesCount++)
            {
                var node = currentLevelNodes.Dequeue();
                
                if (node.LeftChild != null)
                    currentLevelNodes.Enqueue((BSTNode<int>)node.LeftChild);

                if (node.RightChild != null)
                    currentLevelNodes.Enqueue((BSTNode<int>)node.RightChild);
            }

            return currentLevelNodes;
        }

        #endregion

        #region RestoreTree

        // Exercise 3, task 5, time complexity O(n^2), space complexity O(n)
        public static BST<T> RestoreTree<T>(List<int> preOrderNodes, List<int> inOrderNodes)
        {
            if (preOrderNodes == null || inOrderNodes == null || preOrderNodes.Count != inOrderNodes.Count)
                throw new ArgumentException();
            
            return new BST<T>(RestoreTreeRecursive<T>(0, preOrderNodes.Count - 1, preOrderNodes, inOrderNodes));
        }

        private static BSTNode<T> RestoreTreeRecursive<T>(
            int firstSubtreeIndex, 
            int lastSubtreeIndex, 
            List<int> preOrderNodes, 
            List<int> inOrderNodes)
        {
            if (firstSubtreeIndex > lastSubtreeIndex)
                return null; 
            
            int nodeKey = preOrderNodes[0];
            int nodeIndex = inOrderNodes.IndexOf(nodeKey);
            BSTNode<T> node = new BSTNode<T>(nodeKey, default, null);
            preOrderNodes.RemoveAt(0);
            
            node.LeftChild = RestoreTreeRecursive<T>(firstSubtreeIndex, nodeIndex - 1, preOrderNodes, inOrderNodes);
            node.RightChild = RestoreTreeRecursive<T>(nodeIndex + 1, lastSubtreeIndex, preOrderNodes, inOrderNodes);
            
            return node;
        }

        #endregion
    }
}