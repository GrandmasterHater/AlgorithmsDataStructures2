using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmsDataStructures2.TreeTraversalOrdersTask3
{
    [TestFixture]
    public class TTO_3
    {
        #region Cpnstructor
        
        [Test]
        public void Constructor_WhenTreeEmpty_RootIsNullAndCountZero()
        {
            BST<int> bst = new BST<int>(null);
            
            Assert.That(bst.RootNode, Is.Null);
            Assert.That(bst.Count(), Is.EqualTo(0));
        }
        
        [Test]
        public void Constructor_WhenTreeWithRoot_RootIsNotNullAndCountOne()
        {
            BSTNode<int> root = new BSTNode<int>(1, 1, null);
            BST<int> bst = new BST<int>(root);
            
            Assert.That(bst.RootNode, Is.Not.Null.And.EqualTo(root));
            Assert.That(bst.Count(), Is.EqualTo(1));
        }
        
        #endregion
        
        #region FindNodeByKey

        [Test]
        public void FindNodeByKey_WhenTreeEmpty_ReturnFindWithNullNode()
        {
            BST<int> bst = new BST<int>(null);

            BSTFind<int> find = bst.FindNodeByKey(1);
            
            Assert.That(find.Node, Is.Null);
            Assert.That(find.NodeHasKey, Is.False);
            Assert.That(find.ToLeft, Is.False);
        }
        
        [Test]
        public void FindNodeByKey_WhenTreeExistNode_ReturnFoundNode()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            BSTNode<int> expectedNode = nodes[nodes.Count / 2];
            BSTFind<int> find = bst.FindNodeByKey(expectedNode.NodeKey);
            
            Assert.That(find.Node, Is.EqualTo(expectedNode));
            Assert.That(find.NodeHasKey, Is.True);
            Assert.That(find.ToLeft, Is.False);
        }
        
        [Test]
        public void FindNodeByKey_WhenTreeNotExistNodeThatGreaterMax_ReturnNearestNodeForAddToRightChild()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            int expectedNodeKey = nodes.Max(node => node.NodeKey);
            BSTNode<int> expectedNode = nodes.Find(n => n.NodeKey == expectedNodeKey);
            int findingNodeKey = expectedNodeKey + 1;
            
            BSTFind<int> find = bst.FindNodeByKey(findingNodeKey);
            
            Assert.That(find.Node, Is.EqualTo(expectedNode));
            Assert.That(find.NodeHasKey, Is.False);
            Assert.That(find.ToLeft, Is.False);
        }
        
        [Test]
        public void FindNodeByKey_WhenTreeNotExistNodeThatLowerMin_ReturnNearestNodeForAddToLeftChild()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            int expectedNodeKey = nodes.Min(node => node.NodeKey);
            BSTNode<int> expectedNode = nodes.Find(n => n.NodeKey == expectedNodeKey);
            int findingNodeKey = expectedNodeKey - 1;
            
            BSTFind<int> find = bst.FindNodeByKey(findingNodeKey);
            
            Assert.That(find.Node, Is.EqualTo(expectedNode));
            Assert.That(find.NodeHasKey, Is.False);
            Assert.That(find.ToLeft, Is.True);
        }

        #endregion
        
        #region AddKeyValue
        
        [Test]
        public void AddKeyValue_WhenTreeIsEmpty_NodeBecomeRootCountIncrementedAndReturnTrue()
        {
            BST<int> bst = new BST<int>(null);
            int expectedNodeKey = 1;
            int expectedNodeValue = 5;

            bool result = bst.AddKeyValue(expectedNodeKey, expectedNodeValue);
            
            Assert.That(bst.RootNode.NodeKey, Is.EqualTo(expectedNodeKey));
            Assert.That(bst.RootNode.NodeValue, Is.EqualTo(expectedNodeValue));
            Assert.That(bst.RootNode.Parent, Is.Null);
            Assert.That(bst.Count(), Is.EqualTo(1));
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void AddKeyValue_WhenTreeHasNodeWithSameKey_ReturnFalseAndCountNotChanged()
        {
            BST<int> bst = new BST<int>(null);
            int expectedNodeKey = 1;
            int expectedNodeValue = 5;
            bst.AddKeyValue(expectedNodeKey, expectedNodeValue);
            int expectedCount = bst.Count();
            
            bool result = bst.AddKeyValue(expectedNodeKey, expectedNodeValue);
            
            Assert.That(result, Is.False);
            Assert.That(bst.Count(), Is.EqualTo(expectedCount));
        }
        
        [Test]
        public void AddKeyValue_WhenAddKeyThatGreaterExisted_AddNodeToRightIncrementCountAndReturnTrue()
        {
            BST<int> bst = new BST<int>(null);
            int expectedNodeKey = 5;
            int expectedNodeValue = 5;
            bst.AddKeyValue(3, 3);
            BSTFind<int> existNodeBeforeAddFind = bst.FindNodeByKey(expectedNodeKey);
            int expectedCount = bst.Count() + 1;
            
            bool result = bst.AddKeyValue(expectedNodeKey, expectedNodeValue);
            
            Assert.That(existNodeBeforeAddFind.NodeHasKey, Is.False);
            Assert.That(bst.RootNode.RightChild.NodeKey, Is.EqualTo(expectedNodeKey));
            Assert.That(bst.RootNode.RightChild.NodeValue, Is.EqualTo(expectedNodeValue));
            Assert.That(bst.RootNode.RightChild.Parent, Is.EqualTo(bst.RootNode));
            Assert.That(bst.Count(), Is.EqualTo(expectedCount));
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void AddKeyValue_WhenAddKeyThatLowerExisted_AddNodeToLeftIncrementCountAndReturnTrue()
        {
            BST<int> bst = new BST<int>(null);
            int expectedNodeKey = 5;
            int expectedNodeValue = 5;
            bst.AddKeyValue(8, 8);
            BSTFind<int> existNodeBeforeAddFind = bst.FindNodeByKey(expectedNodeKey);
            int expectedCount = bst.Count() + 1;
            
            bool result = bst.AddKeyValue(expectedNodeKey, expectedNodeValue);
            
            Assert.That(existNodeBeforeAddFind.NodeHasKey, Is.False);
            Assert.That(bst.RootNode.LeftChild.NodeKey, Is.EqualTo(expectedNodeKey));
            Assert.That(bst.RootNode.LeftChild.NodeValue, Is.EqualTo(expectedNodeValue));
            Assert.That(bst.RootNode.LeftChild.Parent, Is.EqualTo(bst.RootNode));
            Assert.That(bst.Count(), Is.EqualTo(expectedCount));
            Assert.That(result, Is.True);
        }
        
        #endregion

        #region FinMinMax

        [Test]
        public void FinMinMax_FindMinFromRoot_ReturnExpectedMinNode()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);

            int minNodeKey = nodes.Min(node => node.NodeKey);
            BSTNode<int> minNode = nodes.Find(node => node.NodeKey == minNodeKey);
            
            Assert.That(bst.FinMinMax(bst.RootNode, false), Is.EqualTo(minNode));
        }
        
        [Test]
        public void FinMinMax_FindMaxFromRoot_ReturnExpectedMaxNode()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);

            int maxNodeKey = nodes.Max(node => node.NodeKey);
            BSTNode<int> maxNode = nodes.Find(node => node.NodeKey == maxNodeKey);
            
            Assert.That(bst.FinMinMax(bst.RootNode, true), Is.EqualTo(maxNode));
        }
        
        [Test]
        public void FinMinMax_FindMinFromChildNode_ReturnExpectedMinNode()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);

            BSTNode<int> minNode = nodes.Find(node => node.NodeKey == 5);
            
            Assert.That(bst.FinMinMax(bst.RootNode.LeftChild.RightChild, false), Is.EqualTo(minNode));
        }
        
        [Test]
        public void FinMinMax_FindMaxFromChildRoot_ReturnExpectedMaxNode()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);

            BSTNode<int> maxNode = nodes.Find(node => node.NodeKey == 7);
            
            Assert.That(bst.FinMinMax(bst.RootNode.LeftChild.RightChild, true), Is.EqualTo(maxNode));
        }

        #endregion
        
        #region DeleteNodeByKey

        [Test]
        public void DeleteNodeByKey_WhenTreeEmpty_ReturnFalse()
        {
            BST<int> bst = new BST<int>(null);
            int keyToDelete = 5;
        
            bool result = bst.DeleteNodeByKey(keyToDelete);
            
            Assert.That(result, Is.False);
            Assert.That(bst.Count(), Is.EqualTo(0));
            Assert.That(bst.RootNode, Is.Null);
        }
        
        [Test]
        public void DeleteNodeByKey_WhenNodeNotExists_ReturnFalseAndCountNotChanged()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            int initialCount = bst.Count();
            int nonExistentKey = 100;
            
            bool result = bst.DeleteNodeByKey(nonExistentKey);
            
            Assert.That(result, Is.False);
            Assert.That(bst.Count(), Is.EqualTo(initialCount));
        }
        
        [Test]
        public void DeleteNodeByKey_WhenDeleteRootNode_ReturnTrueDecrementCountAndRootBecomesNull()
        {
            BST<int> bst = new BST<int>(null);
            bst.AddKeyValue(10, 10);
            
            bool result = bst.DeleteNodeByKey(10);
            
            Assert.That(result, Is.True);
            Assert.That(bst.RootNode, Is.Null);
            Assert.That(bst.Count(), Is.EqualTo(0));
        }
        
        [Test]
        public void DeleteNodeByKey_WhenDeleteLeftLeafNode_NodeRemovedFromParentsChildCountDecrementedAndReturnTrue()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            int leafKey = 1;
            BSTNode<int> parentOfLeaf = bst.FindNodeByKey(leafKey).Node.Parent;
            int expectedCount = bst.Count() - 1;
            
            bool result = bst.DeleteNodeByKey(leafKey);
            
            Assert.That(bst.FindNodeByKey(leafKey).NodeHasKey, Is.False);
            Assert.That(parentOfLeaf.LeftChild, Is.Null);
            Assert.That(bst.Count(), Is.EqualTo(expectedCount));
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteNodeByKey_WhenDeleteRightLeafNode_NodeRemovedFromParentsChildCountDecrementedAndReturnTrue()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            int leafKey = 3;
            BSTNode<int> parentOfLeaf = bst.FindNodeByKey(leafKey).Node.Parent;
            int initialCount = bst.Count();
            
            bool result = bst.DeleteNodeByKey(leafKey);
            
            Assert.That(bst.FindNodeByKey(leafKey).NodeHasKey, Is.False);
            Assert.That(parentOfLeaf.RightChild, Is.Null);
            Assert.That(bst.Count(), Is.EqualTo(initialCount - 1));
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteNodeByKey_WhenNodeHasOnlyLeftChild_NodeReplacedByLeftChildCountDecrementedAndReturnTrue()
        {
            BST<int> bst = new BST<int>(null);
            bst.AddKeyValue(10, 10);
            bst.AddKeyValue(5, 5);
            bst.AddKeyValue(3, 3);
            
            BSTNode<int> nodeToDelete = bst.FindNodeByKey(5).Node;
            BSTNode<int> leftChild = nodeToDelete.LeftChild;
            BSTNode<int> parent = nodeToDelete.Parent;
            int expectedCount = bst.Count() - 1;
            
            bool result = bst.DeleteNodeByKey(5);
            
            Assert.That(bst.FindNodeByKey(5).NodeHasKey, Is.False);
            Assert.That(parent.LeftChild, Is.EqualTo(leftChild));
            Assert.That(leftChild.Parent, Is.EqualTo(parent));
            Assert.That(bst.Count(), Is.EqualTo(expectedCount));
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteNodeByKey_WhenReplaceWithLeaf_LeafReplacedNodeCountDecrementedAndReturnTrue()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            int keyToDelete = 4; 
            BSTNode<int> nodeToDelete = bst.FindNodeByKey(keyToDelete).Node;
            BSTNode<int> parentOfDeleted = nodeToDelete.Parent;
            BSTNode<int> replacingNode = bst.FinMinMax(nodeToDelete.RightChild, false);
            BSTNode<int> nodeAfterReplacing = bst.FindNodeByKey(2).Node;
            int expectedCount = bst.Count() - 1;
            
            bool result = bst.DeleteNodeByKey(keyToDelete);
            
            Assert.That(bst.FindNodeByKey(keyToDelete).NodeHasKey, Is.False);
            Assert.That(parentOfDeleted.LeftChild, Is.EqualTo(replacingNode));
            Assert.That(parentOfDeleted.LeftChild.LeftChild, Is.EqualTo(nodeAfterReplacing));
            Assert.That(bst.Count(), Is.EqualTo(expectedCount));
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteNodeByKey_WhenReplaceWithNodeHavingRightChild_ReturnTrueAndStructureCorrect()
        {
            BST<int> bst = new BST<int>(null);
            bst.AddKeyValue(10, 10);  //              10
            bst.AddKeyValue(5, 5);    //         5          15
            bst.AddKeyValue(15, 15);  //      3    8      
            bst.AddKeyValue(3, 3);    //          6  9
            bst.AddKeyValue(8, 8);    //            7  
            bst.AddKeyValue(6, 6);
            bst.AddKeyValue(7, 7);
            bst.AddKeyValue(9, 9); 
        
            int keyToDelete = 5;
            BSTNode<int> nodeToDelete = bst.FindNodeByKey(keyToDelete).Node;
            BSTNode<int> nodeToDeleteLeftChild = nodeToDelete.LeftChild;
            BSTNode<int> nodeToDeleteRightChild = nodeToDelete.RightChild;
            BSTNode<int> nodeToDeleteParent = nodeToDelete.Parent;
            BSTNode<int> replacingNode = bst.FindNodeByKey(6).Node;
            BSTNode<int> replacingNodeParent = replacingNode.Parent;
            BSTNode<int> rightFromReplacingNode = replacingNode.RightChild;
            BSTNode<int> rightFromReplacingNodeExpectedParent = replacingNode.Parent;
            int replacingKey = replacingNode.NodeKey;
            int expectedCount = bst.Count() - 1;
            
            bool result = bst.DeleteNodeByKey(keyToDelete);
            
            Assert.That(bst.FindNodeByKey(keyToDelete).NodeHasKey, Is.False);
            Assert.That(bst.Count(), Is.EqualTo(expectedCount));
            Assert.That(result, Is.True);
            
            Assert.That(rightFromReplacingNode.Parent, Is.EqualTo(rightFromReplacingNodeExpectedParent));
            Assert.That(replacingNodeParent.LeftChild, Is.EqualTo(rightFromReplacingNode));
            
            Assert.That(replacingNode.Parent, Is.EqualTo(nodeToDeleteParent));
            Assert.That(replacingNode.LeftChild, Is.EqualTo(nodeToDeleteLeftChild));
            Assert.That(replacingNode.RightChild, Is.EqualTo(nodeToDeleteRightChild));
            Assert.That(replacingNode.Parent.LeftChild, Is.EqualTo(replacingNode));
            Assert.That(replacingNode.LeftChild.Parent, Is.EqualTo(replacingNode));
            Assert.That(replacingNode.RightChild.Parent, Is.EqualTo(replacingNode));
        }

        #endregion
        
        #region WideAllNodes

        [Test]
        public void WideAllNodesTest()
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            
            List<int> expectedKeysOrder = new List<int>() { 8, 4, 12, 2, 6, 10, 14, 1, 3, 5, 7, 9, 11, 13, 15 };
            
            Assert.That(bst.WideAllNodes().Select(node => node.KeyOfNode).ToList(), Is.EqualTo(expectedKeysOrder));
        }

        #endregion
        
        #region DeepAllNodes

        [TestCase(0, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(1, new int[] { 1, 3, 2, 5, 7, 6, 4, 9, 11, 10, 13, 15, 14, 12, 8 })]
        [TestCase(2, new int[] { 8, 4, 2, 1, 3, 6, 5, 7, 12, 10, 9, 11, 14, 13, 15 })]
        public void DeepAllNodesTests(int order, int[] expectedKeysOrder)
        {
            BST<int> bst = CreateTree(out List<BSTNode<int>> nodes);
            
            Assert.That(bst.DeepAllNodes(order).Select(node => node.KeyOfNode).ToArray(), Is.EqualTo(expectedKeysOrder));
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
            
            Assert.That(bst.DeepAllNodes(0).Select(node => node.KeyOfNode).ToList(), Is.EqualTo(expectedKeysOrder));
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
            
            Assert.That(restoredTree.WideAllNodes().Select(node => node.KeyOfNode).ToList(), Is.EqualTo(expectedNodes));
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