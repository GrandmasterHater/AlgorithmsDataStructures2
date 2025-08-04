using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmsDataStructures2
{
    [TestFixture]
    public class SimpleTreeTask1_3
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
            SimpleTree<int> tree = CreateTree(out List<SimpleTreeNode<int>> nodes);
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
            SimpleTree<int> tree = CreateTree(out List<SimpleTreeNode<int>> expectedNodes);
            
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
            SimpleTree<int> tree = CreateTree(out List<SimpleTreeNode<int>> nodes);
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
            SimpleTree<int> tree = CreateTree(out List<SimpleTreeNode<int>> nodes);
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
        
        #region LeafCount

        [Test]
        public void LeafCount_WhenTreeHasLeafs_ReturnLeafCount()
        {
            int expectedLeafCount = 2;
            SimpleTreeNode<int> rootNode = new SimpleTreeNode<int>(1, null);
            SimpleTreeNode<int> firstNode = new SimpleTreeNode<int>(2, null);
            SimpleTreeNode<int> secondNode = new SimpleTreeNode<int>(3, null);
            
            SimpleTree<int> tree = new SimpleTree<int>(rootNode);
            tree.AddChild(rootNode, firstNode);
            tree.AddChild(rootNode, secondNode);

            Assert.That(tree.LeafCount(), Is.EqualTo(expectedLeafCount));
        }
        
        
        [Test]
        public void LeafCount_WhenTreeIsEnpty_ReturnZero()
        {
            SimpleTree<int> tree = new SimpleTree<int>(null);

            Assert.That(tree.LeafCount(), Is.Zero);
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
        
        #region CalculateNodeLevel

        [Test]
        public void CalculateNodeLevelTest()
        {
            SimpleTree<int> tree = CreateTree(out List<SimpleTreeNode<int>> nodes);
            SimpleTreeTask1_2.CalculateNodeLevel(tree);
            
            List<int> expectedNodeLevels = new List<int> { 0, 1, 1, 2, 2, 2, 2 };
            
            Assert.That(nodes.Select(node => node.Level).ToList(), Is.EquivalentTo(expectedNodeLevels));
        }
        
        #endregion

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
        private static SimpleTree<int> CreateTree(out List<SimpleTreeNode<int>> nodes)
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
    }
}

