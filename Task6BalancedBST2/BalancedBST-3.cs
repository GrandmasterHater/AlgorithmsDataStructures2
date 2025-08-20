using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmsDataStructures2
{
    [TestFixture]
    public class BalancedBST_3
    {
        #region GenerateTree
        
        [TestCase(new int[] { 1, 2, 3 }, 
            new int[] { 2, 1, 3 }, 
            new int[] { 0, 1, 1 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 
            new int[] { 4, 2, 6, 1, 3, 5, 7 }, 
            new int[] { 0, 1, 1, 2, 2, 2, 2 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 
            new int[] { 8, 4, 12, 2, 6, 10, 14, 1, 3, 5, 7, 9, 11, 13, 15 }, 
            new int[] { 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3 })]
        public void GenerateTree_OrderedArray_CreatesBalancedTreeWithCorrectLevels(int[] orderedArray, int[] expectedBreadthFirstTraversal, int[] expectedLevels)
        {
            BalancedBST bst = new BalancedBST();
            
            int[] shuffledArray = Shuffle(orderedArray);
            
            bst.GenerateTree(shuffledArray);
            
            List<BSTNode> actualNodes = BreadthFirstTraversal(bst.Root);
            int[] actualKeys = actualNodes.Select(node => node.NodeKey).ToArray();
            int[] actualLevels = actualNodes.Select(node => node.Level).ToArray();
        
            Assert.That(actualKeys, Is.EqualTo(expectedBreadthFirstTraversal));
            Assert.That(actualLevels, Is.EqualTo(expectedLevels));
        }
        
        #endregion

        #region IsTreeCorrect

        [Test]
        public void IsTreeCorrect_BalancedTree_ReturnsTrue()
        {
            BalancedBST bst = new BalancedBST();
            bst.GenerateTree(new []{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
            
            Assert.That(bst.IsTreeCorrect(), Is.True);
        }
        
        [Test]
        public void IsTreeCorrect_BalancedTreeWithSwappedNodes_ReturnsFalse()
        {
            BalancedBST bst = new BalancedBST();

            int[] array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            
            bst.GenerateTree(array);

            (bst.Root.LeftChild, bst.Root.RightChild) = (bst.Root.RightChild, bst.Root.LeftChild);

            Assert.That(bst.IsTreeCorrect(), Is.False);
        }
        
        [Test]
        public void IsTreeCorrect_BalancedTreeWithSwappedLeafNodes_ReturnsFalse()
        {
            BalancedBST bst = new BalancedBST();

            int[] array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            
            bst.GenerateTree(array);

            SwapLeafNodes(bst.Root);

            Assert.That(bst.IsTreeCorrect(), Is.False);
        }

        [Test]
        public void IsTreeCorrect_UnbalancedCorrectTree_ReturnsTrue()
        {
            BalancedBST bst = new BalancedBST();
            
            int[] array = { 8, 4, 12, 2, 10, 1, 9 };
            bst.GenerateTree(array);

            Assert.That(bst.IsTreeCorrect(), Is.True);
        }
        
        [Test]
        public void IsTreeCorrect_UnbalancedWithSwappedNodes_ReturnsFalse()
        {
            BalancedBST bst = new BalancedBST();
            
            int[] array = { 8, 4, 12, 2, 10, 1, 9 };
            bst.GenerateTree(array);
            
            (bst.Root.LeftChild, bst.Root.RightChild) = (bst.Root.RightChild, bst.Root.LeftChild);

            Assert.That(bst.IsTreeCorrect(), Is.False);
        }
        
        [Test]
        public void IsTreeCorrect_UnbalancedWithSwappedLeafNodes_ReturnsFalse()
        {
            BalancedBST bst = new BalancedBST();
            
            int[] array = { 8, 4, 12, 2, 10, 1, 9 };
            bst.GenerateTree(array);
            
            SwapLeafNodes(bst.Root);

            Assert.That(bst.IsTreeCorrect(), Is.False);
        }

        #endregion


        #region IsBalanced

        [Test]
        public void IsBalanced_BalancedTree_ReturnsTrue()
        {
            BalancedBST bst = new BalancedBST();
            bst.GenerateTree(new []{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
            
            Assert.That(bst.IsBalanced(bst.Root), Is.True);
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void IsBalanced_BalancedTreeWithHeightDifferenceEqualToOne_ReturnsTrue(bool isLeftChildNull)
        {
            BalancedBST bst = new BalancedBST();
            bst.GenerateTree(new []{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });

            if (isLeftChildNull)
                bst.Root.LeftChild.LeftChild.LeftChild = null;
            else
                bst.Root.RightChild.RightChild.RightChild = null;
            
            Assert.That(bst.IsBalanced(bst.Root), Is.True);
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void IsBalanced_UnbalancedTreeWithHeightDifferenceEqualToTwo_ReturnsFalse(bool isLeftChildNull)
        {
            BalancedBST bst = new BalancedBST();
            bst.GenerateTree(new []{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });

            if (isLeftChildNull)
                bst.Root.LeftChild.LeftChild = null;
            else
                bst.Root.RightChild.RightChild = null;
            
            Assert.That(bst.IsBalanced(bst.Root), Is.False);
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void IsBalanced_UnbalancedTreeWithHeightDifferenceGreaterThanTwo_ReturnsFalse(bool isLeftChildNull)
        {
            BalancedBST bst = new BalancedBST();
            bst.GenerateTree(new []{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });

            if (isLeftChildNull)
                bst.Root.LeftChild = null;
            else
                bst.Root.RightChild = null;
            
            Assert.That(bst.IsBalanced(bst.Root), Is.False);
        }

        #endregion
        
        private void SwapLeafNodes(BSTNode node)
        {
            while (node.LeftChild != null && node.RightChild != null)
            {
                node = node.LeftChild;
            }

            (node.Parent.LeftChild, node.Parent.RightChild) = (node.Parent.RightChild, node.Parent.LeftChild);
        }
        
        private int[] Shuffle(int[] numbers)
        {
            Random r = new Random();
        
            for (int i = 0; i < numbers.Length; i++)
            {
                int index = r.Next(i + 1);
                (numbers[i], numbers[index]) = (numbers[index], numbers[i]);
            }
        
            return numbers;
        }

        private List<BSTNode> BreadthFirstTraversal(BSTNode root)
        {
            List<BSTNode> result = new List<BSTNode>();
        
            if (root == null)
                return result;
            
            Queue<BSTNode> queue = new Queue<BSTNode>();
            queue.Enqueue(root);
        
            while (queue.Count > 0)
            {
                BSTNode current = queue.Dequeue();
                result.Add(current);
            
                if (current.LeftChild != null)
                    queue.Enqueue(current.LeftChild);
                
                if (current.RightChild != null)
                    queue.Enqueue(current.RightChild);
            }
        
            return result;
        }
    }
}

