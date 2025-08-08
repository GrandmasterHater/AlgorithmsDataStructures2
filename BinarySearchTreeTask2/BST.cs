using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class BSTNode<T>
    {
        public int NodeKey; // ключ узла
        public T NodeValue; // значение в узле
        public BSTNode<T> Parent; // родитель или null для корня
        public BSTNode<T> LeftChild; // левый потомок
        public BSTNode<T> RightChild; // правый потомок	

        public bool IsLeaf => LeftChild == null && RightChild == null;
	
        public BSTNode(int key, T val, BSTNode<T> parent)
        {
            NodeKey = key;
            NodeValue = val;
            Parent = parent;
            LeftChild = null;
            RightChild = null;
        }
    }

    // промежуточный результат поиска
    public class BSTFind<T>
    {
        // null если в дереве вообще нету узлов
        public BSTNode<T> Node;
	
        // true если узел найден
        public bool NodeHasKey;
	
        // true, если родительскому узлу надо добавить новый левым
        public bool ToLeft;
	
        public BSTFind() { Node = null; }
    }

    public class BST<T>
    {
        BSTNode<T> Root; // корень дерева, или null
        private int _count;

        public BSTNode<T> RootNode => Root;
        public BST(BSTNode<T> node)
        {
            Root = node;
            _count = Root is null ? 0 : 1;
        }
	
        // Exercise 2, time complexity O(log n) - fully binary tree, O(n) - one side tree, space complexity O(h) where h - tree height
        public BSTFind<T> FindNodeByKey(int key)
        {
            return Root != null ? FindNodeByKeyRecursive(Root, key) : new BSTFind<T>();
        }
	
        // Exercise 2, time complexity O(log n) - fully binary tree, O(n) - one side tree, space complexity O(h) where h - tree height
        public bool AddKeyValue(int key, T val)
        {
            if (Root == null)
            {
                Root = new BSTNode<T>(key, val, null);
                ++_count;
                return true;
            }
            
            BSTFind<T> find = FindNodeByKeyRecursive(Root, key);
            
            if (find.NodeHasKey)
                return false;

            if (find.ToLeft)
                find.Node.LeftChild = new BSTNode<T>(key, val, find.Node);
            else 
                find.Node.RightChild = new BSTNode<T>(key, val, find.Node);
            
            ++_count;
            
            return true;
        }
	
        // Exercise 2, time complexity O(h), space complexity O(h) where h - tree height
        public BSTNode<T> FinMinMax(BSTNode<T> FromNode, bool FindMax)
        {
            if (FromNode == null)
                throw new ArgumentNullException();
            
            return FinMinMaxRecursive(FromNode, FindMax);
        }
	
        // Exercise 2, time complexity O(h), space complexity O(h) where h - tree height
        public bool DeleteNodeByKey(int key)
        {
            if (Root == null)
                return false;

            BSTFind<T> findNodeToDelete = FindNodeByKey(key);

            if (!findNodeToDelete.NodeHasKey)
                return false;
            
            BSTNode<T> deletingNode = findNodeToDelete.Node;
            --_count;

            if (deletingNode == Root)
                return DeleteRoot();

            if (deletingNode.IsLeaf)
                return DeleteLeafNode(deletingNode);

            if (deletingNode.RightChild == null)
                return ReplaceByLeftChild(deletingNode);

            BSTNode<T> nodeToReplace = FinMinMax(deletingNode.RightChild, false);

            if (nodeToReplace.IsLeaf)
                return ReplaceByLeaf(deletingNode, nodeToReplace);
            
            return ReplaceByNode(deletingNode, nodeToReplace);
        }

        // Exercise 2, time complexity O(1), space complexity O(1)
        public int Count()
        {
            return _count;
        }
	
        private BSTFind<T> FindNodeByKeyRecursive(BSTNode<T> node, int key)
        {
            if (node.NodeKey == key)
                return new BSTFind<T> { Node = node, NodeHasKey = true };

            bool isLeft = node.NodeKey > key;
            BSTNode<T> nextNode = isLeft ? node.LeftChild : node.RightChild;

            if (nextNode == null)
                return new BSTFind<T> { Node = node, NodeHasKey = false, ToLeft = isLeft };
                
            return FindNodeByKeyRecursive(nextNode, key);
        }
        
        private BSTNode<T> FinMinMaxRecursive(BSTNode<T> FromNode, bool FindMax)
        {
            BSTNode<T> nextNode = FindMax ? FromNode.RightChild : FromNode.LeftChild;
            
            if (nextNode == null)
                return FromNode;

            return FinMinMaxRecursive(nextNode, FindMax);
        }
        
        private bool DeleteRoot()
        {
            Root = null;
            return true;
        }

        private bool DeleteLeafNode(BSTNode<T> node)
        {
            if (node.NodeKey < node.Parent.NodeKey)
                node.Parent.LeftChild = null;
            else
                node.Parent.RightChild = null;

            node.Parent = null;

            return true;
        }

        private bool ReplaceByLeftChild(BSTNode<T> node)
        {
            node.LeftChild.Parent = node.Parent;
            node.Parent.LeftChild = node.LeftChild;
            node.Parent = null;
            return true;
        }
        
        private void Replace(BSTNode<T> fromNode, BSTNode<T> toNode)
        {
            toNode.LeftChild = fromNode.LeftChild;
            fromNode.LeftChild.Parent = toNode;
            
            toNode.RightChild = fromNode.RightChild;
            fromNode.RightChild.Parent = toNode;
            
            toNode.Parent = fromNode.Parent;
            
            if (fromNode.NodeKey < fromNode.Parent.NodeKey)
                fromNode.Parent.LeftChild = toNode;
            else
                fromNode.Parent.RightChild = toNode;
        }

        private bool ReplaceByLeaf(BSTNode<T> deletingNode, BSTNode<T> replacingLeaf)
        {
            Replace(deletingNode, replacingLeaf);

            deletingNode.LeftChild = null;
            deletingNode.RightChild = null;
            deletingNode.Parent = null;

            return true;
        }
        
        private bool ReplaceByNode(BSTNode<T> deletingNode, BSTNode<T> replacingNode)
        {
            replacingNode.RightChild.Parent = replacingNode.Parent;
            replacingNode.Parent.LeftChild = replacingNode.RightChild;
            Replace(deletingNode, replacingNode);

            deletingNode.LeftChild = null;
            deletingNode.RightChild = null;
            deletingNode.Parent = null;

            return true;
        }
    }
}

