using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2.TreeTraversalOrdersTask3
{
    public abstract class BSTNode
    {
        public abstract int KeyOfNode { get; set; } 
        
        public abstract BSTNode ParentNode { get; set; } 
        public abstract BSTNode LeftChildNode { get; set; }  
        public abstract BSTNode RightChildNode { get; set; } 	
        
        public bool IsLeaf => LeftChildNode == null && RightChildNode == null;
    }
    
    public class BSTNode<T> : BSTNode
    {
        public int NodeKey; // ключ узла
        public T NodeValue; // значение в узле
        public BSTNode<T> Parent; // родитель или null для корня
        public BSTNode<T> LeftChild; // левый потомок
        public BSTNode<T> RightChild; // правый потомок	
	
        public BSTNode(int key, T val, BSTNode<T> parent)
        {
            NodeKey = key;
            NodeValue = val;
            Parent = parent;
            LeftChild = null;
            RightChild = null;
        }

        public override int KeyOfNode
        {
            get => NodeKey; 
            set => NodeKey = value;
        }
        public override BSTNode ParentNode 
        {
            get => Parent; 
            set => Parent = (BSTNode<T>)value;
        }
        
        public override BSTNode LeftChildNode 
        {
            get => LeftChild; 
            set => LeftChild = (BSTNode<T>)value;
        }
        
        public override BSTNode RightChildNode 
        {
            get => RightChild; 
            set => RightChild = (BSTNode<T>)value;
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
            
            return (BSTNode<T>)FinMinMaxRecursive(FromNode, FindMax);
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

            BSTNode nodeToReplace = FinMinMax((BSTNode<T>)deletingNode.RightChild, false);

            if (nodeToReplace.IsLeaf)
                return ReplaceByLeaf(deletingNode, nodeToReplace);
            
            return ReplaceByNode(deletingNode, nodeToReplace);
        }

        // Exercise 2, time complexity O(1), space complexity O(1)
        public int Count()
        {
            return _count;
        }
        
        // Exercise 3, time complexity O(n), space complexity O(n)
        public List<BSTNode> WideAllNodes()
        {
            List<BSTNode> nodes = new List<BSTNode>();
            
            if (Root == null)
                return nodes;
            
            nodes.Add(Root);
            
            return WideAllNodesRecursive(nodes, 1);
        }

        // Exercise 3, time complexity O(n), space complexity O(n)
        public List<BSTNode> DeepAllNodes(int order)
        {
            List<BSTNode> nodes = new List<BSTNode>();
            
            if (Root == null)
                return nodes;
            
            switch (order)
            {
                case 0:
                    return InOrderDeepAllNodesRecursive(Root, nodes);
                case 1:
                    return PostOrderDeepAllNodesRecursive(Root, nodes);
                case 2:
                    return PreOrderDeepAllNodesRecursive(Root, nodes);
                
                default:
                    throw new ArgumentException("Invalid order of traversal");
            }
        }

        private List<BSTNode> InOrderDeepAllNodesRecursive(BSTNode node, List<BSTNode> nodes)
        {
            if (node.LeftChildNode != null)
                InOrderDeepAllNodesRecursive(node.LeftChildNode, nodes);
            
            nodes.Add(node);

            if (node.RightChildNode != null)
                InOrderDeepAllNodesRecursive(node.RightChildNode, nodes);
            
            return nodes;
        }
        
        private List<BSTNode> PostOrderDeepAllNodesRecursive(BSTNode node, List<BSTNode> nodes)
        {
            if (node.LeftChildNode != null)
                PostOrderDeepAllNodesRecursive(node.LeftChildNode, nodes);

            if (node.RightChildNode != null)
                PostOrderDeepAllNodesRecursive(node.RightChildNode, nodes);
            
            nodes.Add(node);
            
            return nodes;
        }
        
        private List<BSTNode> PreOrderDeepAllNodesRecursive(BSTNode node, List<BSTNode> nodes)
        {
            nodes.Add(node);
            
            if (node.LeftChildNode != null)
                PreOrderDeepAllNodesRecursive(node.LeftChildNode, nodes);

            if (node.RightChildNode != null)
                PreOrderDeepAllNodesRecursive(node.RightChildNode, nodes);
            
            return nodes;
        }

        private List<BSTNode> WideAllNodesRecursive(List<BSTNode> nodes, int lastLevelNodesCount)
        {
            int addedNodesCount = 0;
            int initialCount = nodes.Count;
            
            for (int index = nodes.Count - lastLevelNodesCount ; index < initialCount; ++index)
            {
                BSTNode node = nodes[index];

                if (node.LeftChildNode != null)
                {
                    nodes.Add(node.LeftChildNode);
                    ++addedNodesCount;
                }

                if (node.RightChildNode != null)
                {
                    nodes.Add(node.RightChildNode);
                    ++addedNodesCount;
                }
            }
            
            if (addedNodesCount == 0)
                return nodes;
            
            return WideAllNodesRecursive(nodes, addedNodesCount);
        }
	
        private BSTFind<T> FindNodeByKeyRecursive(BSTNode node, int key)
        {
            if (node.KeyOfNode == key)
                return new BSTFind<T> { Node = (BSTNode<T>)node, NodeHasKey = true };

            bool isLeft = node.KeyOfNode > key;
            BSTNode nextNode = isLeft ? node.LeftChildNode : node.RightChildNode;

            if (nextNode == null)
                return new BSTFind<T> { Node = (BSTNode<T>)node, NodeHasKey = false, ToLeft = isLeft };
                
            return FindNodeByKeyRecursive(nextNode, key);
        }
        
        private BSTNode FinMinMaxRecursive(BSTNode FromNode, bool FindMax)
        {
            BSTNode nextNode = FindMax ? FromNode.RightChildNode : FromNode.LeftChildNode;
            
            if (nextNode == null)
                return FromNode;

            return FinMinMaxRecursive(nextNode, FindMax);
        }
        
        private bool DeleteRoot()
        {
            Root = null;
            return true;
        }

        private bool DeleteLeafNode(BSTNode node)
        {
            if (node.KeyOfNode < node.ParentNode.KeyOfNode)
                node.ParentNode.LeftChildNode = null;
            else
                node.ParentNode.RightChildNode = null;

            node.ParentNode = null;

            return true;
        }

        private bool ReplaceByLeftChild(BSTNode node)
        {
            node.LeftChildNode.ParentNode = node.ParentNode;
            node.ParentNode.LeftChildNode = node.LeftChildNode;
            node.ParentNode = null;
            return true;
        }
        
        private void Replace(BSTNode fromNode, BSTNode toNode)
        {
            toNode.LeftChildNode = fromNode.LeftChildNode;
            fromNode.LeftChildNode.ParentNode = toNode;
            
            toNode.RightChildNode = fromNode.RightChildNode;
            fromNode.RightChildNode.ParentNode = toNode;
            
            toNode.ParentNode = fromNode.ParentNode;
            
            if (fromNode.KeyOfNode < fromNode.ParentNode.KeyOfNode)
                fromNode.ParentNode.LeftChildNode = toNode;
            else
                fromNode.ParentNode.RightChildNode = toNode;
        }

        private bool ReplaceByLeaf(BSTNode deletingNode, BSTNode replacingLeaf)
        {
            Replace(deletingNode, replacingLeaf);

            deletingNode.LeftChildNode = null;
            deletingNode.RightChildNode = null;
            deletingNode.ParentNode = null;

            return true;
        }
        
        private bool ReplaceByNode(BSTNode deletingNode, BSTNode replacingNode)
        {
            replacingNode.RightChildNode.ParentNode = replacingNode.ParentNode;
            replacingNode.ParentNode.LeftChildNode = replacingNode.RightChildNode;
            Replace(deletingNode, replacingNode);

            deletingNode.LeftChildNode = null;
            deletingNode.RightChildNode = null;
            deletingNode.ParentNode = null;

            return true;
        }
    }
}