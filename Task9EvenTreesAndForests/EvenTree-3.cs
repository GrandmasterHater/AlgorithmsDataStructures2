using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmsDataStructures2
{
    [TestFixture]
    public class EvenTree_3
    {
        #region SimpleTree
        
        #region AddChild
        [Test]
        public void AddChild_WhenParentArgumentNullAndTreeHasRoot_ThrowArgumentNullException()
        {
            SimpleTree<int> tree = new SimpleTree<int>(new SimpleTreeNode<int>(3, null));

            Assert.Throws<ArgumentNullException>(() => tree.AddChild(null, new SimpleTreeNode<int>(1, null)));
        }
        
        [Test]
        public void AddChild_WhenChildArgumentNull_ThrowArgumentNullException()
        {
            SimpleTree<int> tree = new SimpleTree<int>(null);

            Assert.Throws<ArgumentNullException>(() => tree.AddChild(new SimpleTreeNode<int>(1, null), null));
        }
        
        [Test]
        public void AddChild_AddNewChildNode_ChildNodeAdded()
        { 
            SimpleTree<int> tree = new SimpleTree<int>(new SimpleTreeNode<int>(5, null));
            
            SimpleTreeNode<int> parentNode = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> childNode = new SimpleTreeNode<int>(2, null);
            tree.AddChild(parentNode, childNode);

            Assert.That(childNode.Parent, Is.EqualTo(parentNode));
            Assert.That(parentNode.Children.Contains(childNode), Is.True);
        }
        
        [Test]
        public void AddChild_WhenRootIsNull_ChildBecomeRoot()
        { 
            SimpleTree<int> tree = new SimpleTree<int>(null);
            
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            tree.AddChild(null, rootNode);

            Assert.That(tree.Root, Is.EqualTo(rootNode));
        }
        
        [Test]
        public void AddChild_WhenRootIsNull_CountIncremented()
        { 
            int expectedCount = 1;
            SimpleTree<int> tree = new SimpleTree<int>(null);
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(2, null);
            
            tree.AddChild(null, rootNode);

            Assert.That(tree.Count(), Is.EqualTo(expectedCount));
        }
        
        [Test]
        public void AddChild_WhenRootIsNotNull_CountIncremented()
        { 
            int expectedCount = 3;
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            SimpleTreeNode<int> parentNode = new SimpleTreeNode<int>(2, null);
            tree.AddChild(rootNode, parentNode);
            
            SimpleTreeNode<int> childNode = new SimpleTreeNode<int>(3, null);
            tree.AddChild(parentNode, childNode);

            Assert.That(tree.Count(), Is.EqualTo(expectedCount));
        }
        
        [TestCaseSource(nameof(AddChildNotValidArguments))]
        public void AddChild_WhenArgumentsNotValid_CountNotChanged(SimpleTreeNode<int> parentNode, SimpleTreeNode<int> childNode)
        { 
            SimpleTree<int> tree = new SimpleTree<int>(new SimpleTreeNode<int>(3, null));
            int expectedCount = 1;

            try
            {
                tree.AddChild(parentNode, childNode);
            }
            catch {}

            Assert.That(tree.Count(), Is.EqualTo(expectedCount));
        }
        
        [TestCaseSource(nameof(AddChildNotValidArguments))]
        public void AddChild_WhenArgumentsNotValid_RootNotChanged(SimpleTreeNode<int> parentNode, SimpleTreeNode<int> childNode)
        { 
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            SimpleTreeNode<int> expectedRoot = tree.Root;

            try
            {
                tree.AddChild(parentNode, childNode);
            }
            catch {}

            Assert.That(tree.Root, Is.Not.Null.And.EqualTo(expectedRoot));
        }
        
        #endregion

        #region DeleteNode

        [Test]
        public void DeleteNode_WhenNodeToDeleteNull_ThrowArgumentNullException()
        {
            SimpleTree<int> tree = new SimpleTree<int>(null);

            Assert.Throws<ArgumentNullException>(() => tree.DeleteNode(null));
        }
        
        [Test]
        public void DeleteNode_WhenTreeExistNode_NodeDeleted()
        {
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> leafNode = new SimpleTreeNode<int>(1, null);
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            tree.AddChild(rootNode, leafNode);
            
            tree.DeleteNode(leafNode);
            
            Assert.That(rootNode.Children.Contains(leafNode), Is.False);
        }
        
        [Test]
        public void DeleteNode_WhenDeleteExistedNode_CountDecremented()
        {
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> leafNode = new SimpleTreeNode<int>(1, null);
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            tree.AddChild(rootNode, leafNode);
            
            tree.DeleteNode(leafNode);
            
            Assert.That(tree.Count(), Is.EqualTo(1));
        }
        
        [Test]
        public void DeleteNode_WhenDeleteRootNode_RootIsNull()
        {
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            
            tree.DeleteNode(rootNode);
            
            Assert.That(tree.Root, Is.Null);
        }
        
        [Test]
        public void DeleteNode_WhenArgumentNotValid_CountNotChanged()
        {
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);

            try
            {
                tree.DeleteNode(null);
            }
            catch { }
            
            Assert.That(tree.Count(), Is.EqualTo(1));
        }
        
        [Test]
        public void DeleteNode_WhenDeleteNodeWithSubtree_CountDecrementAllSuntreeNodes()
        {
            SimpleTree<int> tree = CreateSimpleTree(out List<SimpleTreeNode<int>> nodes);
            int expectedCount = nodes.Count - 3;
            SimpleTreeNode<int> firstNode = nodes[1];
            
            tree.DeleteNode(firstNode);
            
            Assert.That(tree.Count(), Is.EqualTo(expectedCount));
        }
        
        [Test]
        public void DeleteNode_WhenArgumentNotValid_RootNotChanged()
        {
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);

            try
            {
                tree.DeleteNode(null);
            }
            catch { }
            
            Assert.That(tree.Root, Is.Not.Null.And.EqualTo(rootNode));
        }

        #endregion

        #region GetAllNodes

        [Test]
        public void GetAllNodes_WhenTreeExistNodes_ReturnAllNodes()
        {
            SimpleTree<int> tree = CreateSimpleTree(out List<SimpleTreeNode<int>> expectedNodes);
            
            List<SimpleTreeNode<int>> actualNodes = tree.GetAllNodes()
                .OrderBy(node => node.NodeValue)
                .ToList();

            Assert.That(actualNodes, Is.EquivalentTo(expectedNodes));
        }
        
        [Test]
        public void GetAllNodes_WhenTreeIsEmpty_ReturnEmptyList()
        {
            SimpleTree<int> tree = new SimpleTree<int>(null);
            
            List<SimpleTreeNode<int>> actualNodes = tree.GetAllNodes();

            Assert.That(actualNodes, Is.Empty);
        }

        #endregion

        #region FindNodeByValue

        [Test]
        public void FindNodeByValue_WhenTreeHasValues_ReturnListWithFoundNodes()
        {
            int expectedNodeValue = 5;
            SimpleTree<int> tree = CreateSimpleTree(out List<SimpleTreeNode<int>> nodes);
            nodes[2].NodeValue = expectedNodeValue;
            nodes[3].NodeValue = expectedNodeValue;
            List<SimpleTreeNode<int>> expectedNodes = nodes.Where(node => node.NodeValue == expectedNodeValue).ToList();
            
            List<SimpleTreeNode<int>> actualNodes = tree.FindNodesByValue(expectedNodeValue);

            Assert.That(actualNodes, Is.EquivalentTo(expectedNodes));
            Assert.That(actualNodes.Select(node => node.NodeValue), Is.All.EqualTo(expectedNodeValue));
        }
        
        [Test]
        public void FindNodeByValue_WhenTreeHasNotValues_ReturnEmptyList()
        {
            int expectedNodeValue = 5;
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> firstNode = new SimpleTreeNode<int>(2, null);
            SimpleTreeNode<int> secondNode = new SimpleTreeNode<int>(3, null);
            
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            tree.AddChild(rootNode, firstNode);
            tree.AddChild(rootNode, secondNode);
            
            List<SimpleTreeNode<int>> actualNodes = tree.FindNodesByValue(expectedNodeValue);

            Assert.That(actualNodes, Is.Empty);
        }
        
        [Test]
        public void FindNodeByValue_WhenTreeIsEmpty_ReturnEmptyList()
        {
            SimpleTree<int> tree = new SimpleTree<int>(null);
            
            List<SimpleTreeNode<int>> actualNodes = tree.FindNodesByValue(5);

            Assert.That(actualNodes, Is.Empty);
        }

        #endregion

        #region MoveNode

        [Test]
        public void MoveNode_WhenOriginalNodeIsNull_ArgumentNullException()
        {
            SimpleTree<int> tree = new SimpleTree<int>(null);
            SimpleTreeNode<int> node = new SimpleTreeNode<int>(1, null);

            Assert.Throws<ArgumentNullException>(() => tree.MoveNode(null,  node));
        }
        
        [Test]
        public void MoveNode_WhenNewParentIsNull_ArgumentNullException()
        {
            SimpleTree<int> tree = new SimpleTree<int>(null);
            SimpleTreeNode<int> node = new SimpleTreeNode<int>(1, null);

            Assert.Throws<ArgumentNullException>(() => tree.MoveNode(node, null));
        }
        
        [Test]
        public void MoveNode_WhenMoveRootNode_InvalidOperationException()
        {
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> firstNode = new SimpleTreeNode<int>(2, null);
            SimpleTreeNode<int> secondNode = new SimpleTreeNode<int>(3, null);
            
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            tree.AddChild(rootNode, firstNode);
            tree.AddChild(rootNode, secondNode);
            
            Assert.Throws<InvalidOperationException>(() => tree.MoveNode(rootNode, firstNode));
        }
        
        [Test]
        public void MoveNode_WhenMoveNodeToAnotherParent_NodeDeletedFromOldParentAndAddedToNewParent()
        {
            SimpleTree<int> tree = CreateSimpleTree(out List<SimpleTreeNode<int>> nodes);
            SimpleTreeNode<int> rootNode = nodes[0];
            SimpleTreeNode<int> firstNode = nodes[1];
            SimpleTreeNode<int> secondNode = nodes[2];
            List<SimpleTreeNode<int>> expectedChilds = firstNode.Children;
            
            tree.MoveNode(firstNode, secondNode);
            
            Assert.That(rootNode.Children.Contains(firstNode), Is.False);
            Assert.That(secondNode.Children.Contains(firstNode), Is.True);
            Assert.That(firstNode.Children, Is.EquivalentTo(expectedChilds));
        }

        #endregion
        
        #endregion
        
        #region SimpleTreeNode

        #region AddChildNode
        
        [Test]
        public void AddChildNode_WhenChildNodeIsNull_ArgumentNullException()
        {
            SimpleTreeNode<int> node = new SimpleTreeNode<int>(1, null);
            
            Assert.Throws<ArgumentNullException>(() =>  node.AddChildNode(null));
        }

        [Test]
        public void AddChildNode_AddNewChildNode_ChildNodeAdded()
        {
            SimpleTreeNode<int> node = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> childNode = new SimpleTreeNode<int>(2, null);
            
            node.AddChildNode(childNode);
            
            Assert.That(node.Children.Contains(childNode), Is.True);
        }
        
        [Test]
        public void AddChildNode_AddNewChildNode_ParentSet()
        {
            SimpleTreeNode<int> node = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> childNode = new SimpleTreeNode<int>(2, null);
            
            node.AddChildNode(childNode);
            
            Assert.That(childNode.Parent, Is.EqualTo(node));
        }

        #endregion
        
        #region RemoveChild
        
        [Test]
        public void RemoveChild_WhenNodeExists_ArgumentNullException()
        {
            SimpleTreeNode<int> parentNode = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> firstChildNode = new SimpleTreeNode<int>(2, null);
            SimpleTreeNode<int> secondChildNode = new SimpleTreeNode<int>(3, null);
            
            parentNode.AddChildNode(firstChildNode);
            parentNode.AddChildNode(secondChildNode);
            
            parentNode.RemoveChild(firstChildNode);
            
            Assert.That(parentNode.Children.Contains(firstChildNode), Is.False);
        }
        
        [Test]
        public void RemoveChild_WhenExistSeveralNodes_OnlyExpectedNodeRemoved()
        {
            SimpleTreeNode<int> parentNode = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> firstChildNode = new SimpleTreeNode<int>(2, null);
            SimpleTreeNode<int> secondChildNode = new SimpleTreeNode<int>(3, null);
            
            parentNode.AddChildNode(firstChildNode);
            parentNode.AddChildNode(secondChildNode);
            
            parentNode.RemoveChild(firstChildNode);
            
            Assert.That(parentNode.Children.Contains(secondChildNode), Is.True);
        }
        
        [Test]
        public void RemoveChild_WhenNodeIsLeaf_ReturnWithoutActions()
        {
            SimpleTreeNode<int> node = new SimpleTreeNode<int>(1, null);
            
            node.RemoveChild(null);
            
            Assert.Pass();
        }
        
        #endregion
        
        #region IsLeaf
        
        [Test]
        public void IsLeaf_WhenNodeHasNoChildren_ReturnTrue()
        {
            SimpleTreeNode<int> node = new SimpleTreeNode<int>(1, null);
            
            Assert.That(node.IsLeaf, Is.True);
        }
        
        [Test]
        public void IsLeaf_WhenNodeHasChildren_ReturnFalse()
        {
            SimpleTreeNode<int> node = new SimpleTreeNode<int>(1, null);
            node.AddChildNode(new SimpleTreeNode<int>(2, null));
            
            Assert.That(node.IsLeaf, Is.False);
        }
        
        #endregion
        
        #endregion

        #region EvenTrees

        [TestCase(
            new int[] { 1, 2, 1, 3, 1, 4, 2, 5, 2, 6, 3, 7, 7, 10, 4, 8, 5, 9 }, // parent-child пары
            new int[] { 2, 5, 1, 2, 3, 7, 1, 4 } // ожидаемые рёбра для удаления
        )]
        [TestCase(
            new int[] { 1, 2, 2, 3 }, // нечётное число вершин
            new int[] { } 
        )]
        [TestCase(
            new int[] { 1, 2, 1, 3, 2, 4, 3, 5 }, // нечётное число вершин
            new int[] { } 
        )]
        public void EvenTrees_WhenTreeBuiltWithAddChild_ReturnsExpectedEdges(int[] edges, int[] expected)
        {
            var tree = BuildTree(edges);
            
            Assert.That(tree.EvenTrees(), Is.EqualTo(expected));
        }

        #endregion

        #region BSTTree

        
        #region Constructor
        
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
        
        [Test]
        public void Constructor_PassRootWithTree_CountCalculatedByNodes()
        {
            BSTNode<int> root = new BSTNode<int>(1, 1, null);
            root.LeftChild = new BSTNode<int>(0, 0, root);
            root.RightChild = new BSTNode<int>(2, 2, root);
            root.RightChild.RightChild = new BSTNode<int>(3, 3, root.RightChild);
            int expectedCount = 4;
            
            BST<int> bst = new BST<int>(root);
            
            Assert.That(bst.RootNode, Is.Not.Null.And.EqualTo(root));
            Assert.That(bst.Count(), Is.EqualTo(expectedCount));
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
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);
            BSTNode<int> expectedNode = nodes[nodes.Count / 2];
            BSTFind<int> find = bst.FindNodeByKey(expectedNode.NodeKey);
            
            Assert.That(find.Node, Is.EqualTo(expectedNode));
            Assert.That(find.NodeHasKey, Is.True);
            Assert.That(find.ToLeft, Is.False);
        }
        
        [Test]
        public void FindNodeByKey_WhenTreeNotExistNodeThatGreaterMax_ReturnNearestNodeForAddToRightChild()
        {
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);
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
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);
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
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);

            int minNodeKey = nodes.Min(node => node.NodeKey);
            BSTNode<int> minNode = nodes.Find(node => node.NodeKey == minNodeKey);
            
            Assert.That(bst.FinMinMax(bst.RootNode, false), Is.EqualTo(minNode));
        }
        
        [Test]
        public void FinMinMax_FindMaxFromRoot_ReturnExpectedMaxNode()
        {
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);

            int maxNodeKey = nodes.Max(node => node.NodeKey);
            BSTNode<int> maxNode = nodes.Find(node => node.NodeKey == maxNodeKey);
            
            Assert.That(bst.FinMinMax(bst.RootNode, true), Is.EqualTo(maxNode));
        }
        
        [Test]
        public void FinMinMax_FindMinFromChildNode_ReturnExpectedMinNode()
        {
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);

            BSTNode<int> minNode = nodes.Find(node => node.NodeKey == 5);
            
            Assert.That(bst.FinMinMax(bst.RootNode.LeftChild.RightChild, false), Is.EqualTo(minNode));
        }
        
        [Test]
        public void FinMinMax_FindMaxFromChildRoot_ReturnExpectedMaxNode()
        {
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);

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
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);
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
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);
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
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);
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
            BST<int> bst = CreateBSTTree(out List<BSTNode<int>> nodes);
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

        #endregion

        #region GetBalancedBinaryTree

        [Test]
        public void GetBalancedBinaryTreeTest()
        {
            BST<int> bst = new BST<int>(null);
            
            bst.AddKeyValue(4, 4);
            bst.AddKeyValue(3, 3);
            bst.AddKeyValue(2, 2);
            bst.AddKeyValue(1, 1);
            
            BST<int> balancedBst = bst.GetBalancedBinaryTree();
            
            Assert.That(balancedBst.RootNode.NodeKey, Is.EqualTo(3));
            Assert.That(balancedBst.RootNode.RightChild.NodeKey, Is.EqualTo(4));
            Assert.That(balancedBst.RootNode.LeftChild.NodeKey, Is.EqualTo(2));
            Assert.That(balancedBst.RootNode.LeftChild.LeftChild.NodeKey, Is.EqualTo(1));
        }

        #endregion
        
        #region GetEvenSubTreeCount
        
        [TestCase(new int[] { 1, 2, 1, 3, 1, 4, 2, 5, 2, 6, 3, 7, 7, 10, 4, 8, 5, 9 }, 1, 4)]
        [TestCase(new int[] { 1, 2, 1, 3, 1, 4, 2, 5, 2, 6, 3, 7, 7, 10, 4, 8, 5, 9 }, 2, 1)]
        [TestCase(new int[] { 1, 2, 1, 3, 1, 4, 2, 5, 2, 6, 3, 7, 7, 10, 4, 8, 5, 9 }, 3, 1)]
        public void GetEvenSubTreeCount_FromNode_ReturnsExpectedCount(int[] parentChildPairs, int startValue, int expectedCount)
        {
            SimpleTree<int> tree = BuildTree(parentChildPairs);
            SimpleTreeNode<int> startNode = tree.GetAllNodes().Find(n => n.NodeValue == startValue);

            Assert.That(startNode.GetEvenSubTreeCount(), Is.EqualTo(expectedCount));
        }
        
        #endregion
        
        private SimpleTree<int> BuildTree(int[] edges)
        {
            var tree = new SimpleTree<int>(null);
            var nodes = new Dictionary<int, SimpleTreeNode<int>>();

            for (int i = 0; i < edges.Length; i += 2)
            {
                int parentVal = edges[i];
                int childVal = edges[i + 1];

                if (!nodes.ContainsKey(parentVal))
                {
                    var parentNode = new SimpleTreeNode<int>(parentVal, null);
                    nodes[parentVal] = parentNode;
                    if (tree.Root == null)
                        tree.AddChild(null, parentNode);
                }

                if (!nodes.ContainsKey(childVal))
                {
                    var childNode = new SimpleTreeNode<int>(childVal, null);
                    nodes[childVal] = childNode;
                }

                tree.AddChild(nodes[parentVal], nodes[childVal]);
            }

            return tree;
        }
        
        private static IEnumerable AddChildNotValidArguments
        {
            get
            {
                SimpleTreeNode<int> parentNode = new SimpleTreeNode<int>(1, null);
                SimpleTreeNode<int> childNode = new SimpleTreeNode<int>(2, null);
                yield return new TestCaseData(null, childNode);
                yield return new TestCaseData(parentNode, null);
                yield return new TestCaseData(null, null);
            }
        }

        /// <summary>
        ///        (1)           
        ///      /    \
        ///    (2)    (3)
        ///   /  \   /  \
        /// (4) (5)(6)  (7)
        /// </summary>
        private static SimpleTree<int> CreateSimpleTree(out List<SimpleTreeNode<int>> nodes)
        {
            SimpleTreeNode<int> rootNode       = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> firstNode      = new SimpleTreeNode<int>(2, null);
            SimpleTreeNode<int> secondNode     = new SimpleTreeNode<int>(3, null);
            SimpleTreeNode<int> firstLeafNode  = new SimpleTreeNode<int>(4, null);
            SimpleTreeNode<int> secondLeafNode = new SimpleTreeNode<int>(5, null);
            SimpleTreeNode<int> thirdLeafNode  = new SimpleTreeNode<int>(6, null);
            SimpleTreeNode<int> fourthLeafNode = new SimpleTreeNode<int>(7, null);

            nodes = new List<SimpleTreeNode<int>>
            {
                rootNode, firstNode, secondNode, firstLeafNode, secondLeafNode, thirdLeafNode, fourthLeafNode
            };
            
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            tree.AddChild(rootNode, firstNode);
            tree.AddChild(rootNode, secondNode);
            tree.AddChild(firstNode, firstLeafNode);
            tree.AddChild(firstNode, secondLeafNode);
            tree.AddChild(secondNode, thirdLeafNode);
            tree.AddChild(secondNode, fourthLeafNode);
            
            return tree;
        }
        
        private static BST<int> CreateBSTTree(out List<BSTNode<int>> nodes)
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

