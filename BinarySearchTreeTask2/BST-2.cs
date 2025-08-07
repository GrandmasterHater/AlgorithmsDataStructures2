using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures2
{
    public class TreePath<T> : List<BSTNode<T>>
    {
        public uint Length => this.Count == 0 ? 0 : (uint)this.Count - 1;
        public TreePath<T> DeepCopy()
        {
            TreePath<T> result = new TreePath<T>();
            result.AddRange(this);
            
            return result;
        }
    }
    
    public static class BST_2
    {
        // Exercise 2, task 1, time complexity O(n), space complexity O(n * h) where n - paths count, h - path length
        public static bool TreeEquals<T>(this BST<T> firstTree, BST<T> secondTree)
        {
            return TreeEqualsRecursive(firstTree.RootNode, secondTree.RootNode);
        }

        private static bool TreeEqualsRecursive<T>(BSTNode<T> firstTreeNode, BSTNode<T> secondTreeNode)
        {
            if (firstTreeNode == null && secondTreeNode == null)
                return true;

            bool result = false;
            
            if (firstTreeNode.IsNodeEquals(secondTreeNode))
            {
                result = TreeEqualsRecursive(firstTreeNode.LeftChild, secondTreeNode.LeftChild);
                result &= TreeEqualsRecursive(firstTreeNode.RightChild, secondTreeNode.RightChild);
            }

            return result;
        }

        private static bool IsNodeEquals<T>(this BSTNode<T> firstNode, BSTNode<T> secondNode)
        {
            return firstNode != null
                   && secondNode != null
                   && firstNode.NodeKey == secondNode.NodeKey 
                   && firstNode.NodeValue.Equals(secondNode.NodeValue);
        }
        
        // Exercise 2, task 2, time complexity O(n), space complexity O(n * h) where n - paths count, h - path length
        public static List<TreePath<T>> GetPathsToLeafWithLength<T>(this BST<T> tree, int length)
        {
            List<TreePath<T>> results = new List<TreePath<T>>();

            if (tree.RootNode != null && !tree.RootNode.IsLeaf)
                GetPathsToLeafRecursive(tree.RootNode, new TreePath<T>(), results);

            return results.Where(node => node.Length == length).ToList();
        }

        private static void GetPathsToLeafRecursive<T>(BSTNode<T> node, TreePath<T> path, List<TreePath<T>> result)
        {
            path.Add(node);
            
            if (node.IsLeaf)
            {
                result.Add(path);
                return;
            }

            TreePath<T> anotherPath = node.LeftChild != null && node.RightChild != null
                ? path.DeepCopy()
                : path;
            
            if (node.LeftChild != null)
                GetPathsToLeafRecursive(node.LeftChild, path, result);
            
            if (node.RightChild != null)
                GetPathsToLeafRecursive(node.RightChild, anotherPath, result);
        }
        
        // Exercise 2, task 3, time complexity O(n), space complexity O(n)
        public static List<TreePath<int>> GetPathsToLeafWithMaxSum(this BST<int> tree)
        {
            List<TreePath<int>> results = new List<TreePath<int>>();

            if (tree.RootNode != null && !tree.RootNode.IsLeaf)
            {
                GetPathsToLeafRecursive(tree.RootNode, new TreePath<int>(), results);
                int maxSum = results.Select(path => path.Sum(node => node.NodeValue)).Max();
                results = results.Where(path => path.Sum(node => node.NodeValue) == maxSum).ToList();
            }

            return results;
        }

        // Exercise 2, task 4, time complexity O(n), O(h) h - tree height
        public static bool IsTreeSymmetrically<T>(this BST<T> tree)
        {
            if (tree.RootNode == null || tree.RootNode.IsLeaf)
                return true;
            
            return IsTreeSymmetricallyRecursive(tree.RootNode.LeftChild, tree.RootNode.RightChild);
        }

        private static bool IsTreeSymmetricallyRecursive<T>(BSTNode<T> leftSideNode, BSTNode<T> rightSideNode)
        {
            bool isLeftNull = leftSideNode == null;
            bool isRightNull = rightSideNode == null;

            if (isLeftNull && isRightNull)
                return true;

            bool result = false;
            
            if (!isLeftNull && !isRightNull)
            {
                result = IsTreeSymmetricallyRecursive(leftSideNode.LeftChild, rightSideNode.RightChild);
                result &= IsTreeSymmetricallyRecursive(leftSideNode.RightChild, rightSideNode.LeftChild);
            }
            
            return result;
        }
    }
}

