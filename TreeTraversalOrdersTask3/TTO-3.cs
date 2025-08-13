using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmsDataStructures2.TreeTraversalOrdersTask3
{
    [TestFixture]
    public class TTO_3
    {
        #region WideAllNodes

        [Test]
        public void WideAllNodesTest()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            
            List<int> expectedKeysOrder = new List<int>() { 8, 4, 12, 2, 6, 10, 14, 1, 3, 5, 7, 9, 11, 13, 15 };
            
            Assert.That(bst.WideAllNodes().Select(node => node.NodeKey).ToList(), Is.EqualTo(expectedKeysOrder));
        }

        #endregion
        
        #region DeepAllNodes

        [TestCase(0, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(1, new int[] { 1, 3, 2, 5, 7, 6, 4, 9, 11, 10, 13, 15, 14, 12, 8 })]
        [TestCase(2, new int[] { 8, 4, 2, 1, 3, 6, 5, 7, 12, 10, 9, 11, 14, 13, 15 })]
        public void DeepAllNodesTests(int order, int[] expectedKeysOrder)
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            
            Assert.That(bst.DeepAllNodes(order).Select(node => node.NodeKey).ToArray(), Is.EqualTo(expectedKeysOrder));
        }
        
        #endregion

        #region InvertTree

        [TestCase(0)]
        [TestCase(1)]
        public void InvertTreeTest(int inversionType)
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            bst.InvertTree(inversionType);
            
            List<int> expectedKeysOrder = new List<int>() { 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            
            Assert.That(bst.DeepAllNodes(0).Select(node => node.NodeKey).ToList(), Is.EqualTo(expectedKeysOrder));
        }

        #endregion

        #region GetLevelWithMaxSumValue

        [Test]
        public void GetLevelWithMaxSumValueTest()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            
            Assert.That(bst.GetLevelWithMaxSumValue(), Is.EqualTo(3));
        }

        #endregion

        #region RestoreTree
        
        
        /*       1
         *     /   \
         *    2     3
         *   / \   / \
         *  4   5 6   7
         */
        [TestCase(new int[] { 1, 2, 4, 5, 3, 6, 7 }, new int[] { 4, 2, 5, 1, 6, 3, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 })]
        /*       1
         *        \
         *         2
         *          \
         *           3
         *            \
         *             4
         */
        [TestCase(new int[] { 1, 2, 3, 4 }, new int[] { 4, 3, 2, 1 }, new int[] { 1, 2, 3, 4 })]
        /*       1
         *     /   \
         *    2     3
         *   /       \
         *  5         4
         *  \
         *   6
         *    \
         *     7
         */
        [TestCase(new int[] { 1, 2, 5, 6, 7, 3, 4 }, new int[] { 5, 6, 7, 2, 1, 3, 4 }, new int[] { 1, 2, 3, 5, 4, 6, 7 })]
        public void RestoreTreeTest(int[] preOrderNodes, int[] inOrderNodes, int[] expectedNodes)
        {
            BST<int> restoredTree = TTO_2.RestoreTree<int>(preOrderNodes.ToList(), inOrderNodes.ToList());
            
            Assert.That(restoredTree.WideAllNodes().Select(node => node.NodeKey).ToList(), Is.EqualTo(expectedNodes));
        }

        #endregion
        
        /*
         *            8
         *       /        \
         *      4         12
         *     / \       /  \
         *    2    6    10   14
         *   / \  / \  / \  /  \
         *  1  3 5  7 9 11 13  15
         */
        private static BST<int> CreateTree(out List<BSTNode<int>> nodes)
        {
            List<int> sortedKeys = Enumerable.Range(1, 15).ToList();
    
            List<int> insertionOrder = new List<int>();
            
            FillBalanced(sortedKeys, ref insertionOrder);

            BST<int> tree = new BST<int>(null);
            nodes = new List<BSTNode<int>>();

            foreach (int key in insertionOrder)
            {
                tree.AddKeyValue(key, key);
                BSTNode<int> node = tree.FindNodeByKey(key).Node;
                nodes.Add(node);
            }

            return tree;
            
            void FillBalanced(List<int> keys, ref List<int> ordered)
            {
                if (keys.Count == 0) 
                    return;
                
                int mid = keys.Count / 2;
                ordered.Add(keys[mid]);
                FillBalanced(keys.GetRange(0, mid), ref ordered);
                FillBalanced(keys.GetRange(mid + 1, keys.Count - mid - 1), ref ordered);
            }
        }
    }
}