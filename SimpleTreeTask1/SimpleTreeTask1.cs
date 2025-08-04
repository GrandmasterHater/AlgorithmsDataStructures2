using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class SimpleTreeNode<T>
    {
        public T NodeValue; // значение в узле
        public SimpleTreeNode<T> Parent; // родитель или null для корня
        public List<SimpleTreeNode<T>> Children; // список дочерних узлов или null
	
        public bool IsLeaf => Children == null || Children.Count == 0;
        public int Level { get; set; }

        public SimpleTreeNode(T val, SimpleTreeNode<T> parent)
        {
            NodeValue = val;
            Parent = parent;
            Children = null;
        }
        
        internal void AddChildNode(SimpleTreeNode<T> childNode)
        {
            if (childNode == null)
                throw new ArgumentNullException(nameof(childNode));

            if (Children == null)
                Children = new List<SimpleTreeNode<T>>();
            
            Children.Add(childNode);
            childNode.SetParentNode(this);
        }
        
        internal void SetParentNode(SimpleTreeNode<T> parentNode)
        {
            Parent = parentNode;
        }
        
        internal void RemoveChild(SimpleTreeNode<T> childNode)
        {
            Children?.Remove(childNode);
        }
    }
	
    public class SimpleTree<T>
    {
        public SimpleTreeNode<T> Root; // корень, может быть null
        private int _nodeCount;

        public SimpleTree(SimpleTreeNode<T> root)
        {
            Root = root;
            _nodeCount = root is null ? 0 : 1;
        }
	
        // Exercise 1, time complexity O(1), space complexity O(1)
        public void AddChild(SimpleTreeNode<T> ParentNode, SimpleTreeNode<T> NewChild)
        {
            if (Root != null && ParentNode == null)
                throw new ArgumentNullException(nameof(ParentNode));
            
            if (Root == null && ParentNode == null)
                Root = NewChild;
            else
                ParentNode.AddChildNode(NewChild);
            
            ++_nodeCount;
        }

        // Exercise 1, time complexity O(c) where c - children count, space complexity O(h) where h - tree height
        public void DeleteNode(SimpleTreeNode<T> NodeToDelete)
        {
            if (NodeToDelete == null)
                throw new ArgumentNullException(nameof(NodeToDelete));

            if (Root == NodeToDelete)
                Root = null;
            else 
                NodeToDelete.Parent.RemoveChild(NodeToDelete);
            
            NodeToDelete.SetParentNode(null);

            int deletingCount = CountFromNodeRecursive(NodeToDelete, 0);
            _nodeCount -= deletingCount;
        }

        // Exercise 1, time complexity O(n), space complexity O(n)
        public List<SimpleTreeNode<T>> GetAllNodes()
        {
            List<SimpleTreeNode<T>> nodes = new List<SimpleTreeNode<T>>();

            if (Root != null)
                GetAllNodesRecursive(Root, nodes);
            
            return nodes;
        }
	
        // Exercise 1, time complexity O(n), space complexity O(n)
        public List<SimpleTreeNode<T>> FindNodesByValue(T val)
        {
            List<SimpleTreeNode<T>> nodes = new List<SimpleTreeNode<T>>();

            if (Root != null)
                FindNodeByValueRecursive(val, Root, nodes);
            
            return nodes;
        }
   
        // Exercise 1, time complexity O(1), space complexity O(1)
        public void MoveNode(SimpleTreeNode<T> OriginalNode, SimpleTreeNode<T> NewParent)
        {
            if (OriginalNode == null)
                throw new ArgumentNullException(nameof(OriginalNode));
            
            if (NewParent == null)
                throw new ArgumentNullException(nameof(NewParent));

            if (OriginalNode == Root)
                throw new InvalidOperationException();
            
            OriginalNode.Parent.RemoveChild(OriginalNode);
            NewParent.AddChildNode(OriginalNode);
        }
   
        // Exercise 1, time complexity O(1), space complexity O(1)
        public int Count()
        {
            return _nodeCount;
        }

        // Exercise 1, time complexity O(n), space complexity O(h) where h - tree height
        public int LeafCount()
        {
            int count = 0;
            
            if (Root != null)
                count = LeafCountRecursive(Root, 0);
            
            return count;
        }

        private void GetAllNodesRecursive(SimpleTreeNode<T> node, List<SimpleTreeNode<T>> nodes)
        {
            nodes.Add(node);

            if (node.IsLeaf)
                return;

            foreach (var child in node.Children)
            {
                GetAllNodesRecursive(child, nodes);
            }
        }
        
        private void FindNodeByValueRecursive(T value, SimpleTreeNode<T> node, List<SimpleTreeNode<T>> result)
        {
            if (node.NodeValue.Equals(value))
                result.Add(node);

            if (node.IsLeaf)
                return;

            foreach (var child in node.Children)
            {
                FindNodeByValueRecursive(value, child, result);
            }
        }
        
        private int CountFromNodeRecursive(SimpleTreeNode<T> node, int accumulator)
        {
            ++accumulator;
            
            if (node.IsLeaf)
                return accumulator;
            
            foreach (var child in node.Children)
            {
                accumulator = CountFromNodeRecursive(child, accumulator);
            }
            
            return accumulator;
        }
        
        private int LeafCountRecursive(SimpleTreeNode<T> node, int accumulator)
        {
            if (node.IsLeaf)
                return ++accumulator;
            
            foreach (var child in node.Children)
            {
                accumulator = LeafCountRecursive(child, accumulator);
            }
            
            return accumulator;
        }
    }
 
}

