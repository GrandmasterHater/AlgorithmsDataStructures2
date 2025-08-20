using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class BSTNode
    {
        public int NodeKey; // ключ узла
        public BSTNode Parent; // родитель или null для корня
        public BSTNode LeftChild; // левый потомок
        public BSTNode RightChild; // правый потомок	
        public int     Level; // глубина узла
	
        public BSTNode(int key, BSTNode parent)
        {
            NodeKey = key;
            Parent = parent;
            LeftChild = null;
            RightChild = null;
        }
    }	


    public class BalancedBST
    {
        private const int DISBALANCED_HEIGHT = -1;
        public BSTNode Root; // корень дерева
	
        public BalancedBST() 
        { 
            Root = null;
        }
		
        // Exercise 6, task 1, time complexity O(n), space complexity O(n)
        public void GenerateTree(int[] a) 
        {  
            Array.Sort(a);
            
            Root = GenerateBSTTreeRecursive(a, null, 0, a.Length - 1);
        }

        // Exercise 6, task 2, time complexity O(n), space complexity O(h) where h - tree height
        public bool IsBalanced(BSTNode root_node)
        {
            if (root_node == null)
                return false;
            
            return GetBalancedHeightRecursive(root_node) != DISBALANCED_HEIGHT;
        }

        private int GetBalancedHeightRecursive(BSTNode node)
        {
            if (node.LeftChild == null && node.RightChild == null)
                return node.Level;

            int leftHeight = node.LeftChild != null ? GetBalancedHeightRecursive(node.LeftChild) : node.Level;
            int rightHeight = leftHeight != DISBALANCED_HEIGHT && node.RightChild != null ? GetBalancedHeightRecursive(node.RightChild) : node.Level;

            bool isBalanced = leftHeight != DISBALANCED_HEIGHT && rightHeight != DISBALANCED_HEIGHT && Math.Abs(leftHeight - rightHeight) <= 1;
            
            return isBalanced ? Math.Max(leftHeight, rightHeight) : DISBALANCED_HEIGHT;
        }
        
        private static BSTNode GenerateBSTTreeRecursive(int[] orderedArray, BSTNode parent, int leftIndex, int rightIndex)
        {
            if (leftIndex > rightIndex) 
                return null;

            int middleIndex = (leftIndex + rightIndex) / 2;
            BSTNode currentNode = new BSTNode(orderedArray[middleIndex], parent);
            currentNode.Level = parent == null ? 0 : parent.Level + 1;
            
            currentNode.LeftChild = GenerateBSTTreeRecursive(orderedArray, currentNode, leftIndex, middleIndex - 1);
            currentNode.RightChild = GenerateBSTTreeRecursive(orderedArray, currentNode,middleIndex + 1, rightIndex);

            return currentNode;
        }
    }
}

