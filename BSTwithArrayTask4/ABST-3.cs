using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmsDataStructures2
{
    [TestFixture]
    public class ABST_3
    {
        #region Constructor

        [TestCase(3)]
        [TestCase(5)]
        [TestCase(7)]
        public void Constructor_PassRequiredDepth_TreeSizeCalculatedByGeometricProgression(int depth)
        {
            aBST abst = new aBST(depth);
            int expectedSize = Convert.ToInt32(Math.Pow(2, depth + 1) - 1);
            
            Assert.That(abst.Tree.Length, Is.EqualTo(expectedSize));
        }
        
        [Test]
        public void Constructor_CreateTree_TreeCreatedEmptyAndCountZero()
        {
            aBST abst = new aBST(3);
            
            Assert.That(abst.Tree, Is.All.Null);
            Assert.That(abst.Count, Is.Zero);
        }
        
        #endregion

        #region FindKeyIndex

        [Test]
        public void FindKeyIndex_WhenTreeIsEmpty_ReturnZero()
        {
            aBST abst = new aBST(3);
            
            Assert.That(abst.FindKeyIndex(1), Is.Zero);
        }
        
        [Test]
        public void FindKeyIndex_WhenTreeExistsValueAtRoot_ReturnZero()
        {
            aBST abst = new aBST(3);
            int key = 8;
            abst.AddKey(key);
            
            Assert.That(abst.FindKeyIndex(key), Is.Zero);
        }
        
        [Test]
        public void FindKeyIndex_WhenTreeExistsValueNotAtRoot_ReturnIndex()
        {
            aBST abst = new aBST(3);
            int key = 4;
            abst.AddKey(8);
            abst.AddKey(key);
            abst.AddKey(12);
            int expectedIndex = 1;
            
            Assert.That(abst.FindKeyIndex(key), Is.EqualTo(expectedIndex));
        }
        
        [Test]
        public void FindKeyIndex_WhenFilledTreeHasNotValueButExistPlace_ReturnIndexLessThanZero()
        {
            aBST abst = new aBST(3);
            int key = 4;
            abst.AddKey(8);
            abst.AddKey(12);
            int expectedIndex = -1 * 1;
            
            Assert.That(abst.FindKeyIndex(key), Is.LessThan(0).And.EqualTo(expectedIndex));
        }
        
        [Test]
        public void FindKeyIndex_WhenTreeIsFullAndNotExistsValue_ReturnNull()
        {
            aBST abst = new aBST(2);
            int key = 15;
            abst.AddKey(8);
            abst.AddKey(4);
            abst.AddKey(12);
            abst.AddKey(2);
            abst.AddKey(6);
            abst.AddKey(10);
            abst.AddKey(14);
            
            Assert.That(abst.FindKeyIndex(key), Is.Null);
        }

        #endregion

        #region AddKey

        [Test]
        public void AddKey_WhenTreeIsEmpty_KeyAddedCountIncrementedAndReturnZeroIndex()
        {
            aBST abst = new aBST(3);
            int key = 8;
            int expectedIndex = 0;
            int expectedCount = abst.Count + 1;
            
            Assert.That(abst.AddKey(key), Is.EqualTo(expectedIndex));
            Assert.That(abst.Tree[0], Is.EqualTo(key));
            Assert.That(abst.Count, Is.EqualTo(expectedCount));
        }
        
        [Test]
        public void AddKey_WhenTreeExistsKeyAtRoot_KeyExistsOnTreeCountNotChangedAndReturnZero()
        {
            aBST abst = new aBST(3);
            int key = 8;
            abst.AddKey(key);
            int expectedIndex = 0;
            int expectedCount = abst.Count;
            
            Assert.That(abst.AddKey(key), Is.EqualTo(expectedIndex));
            Assert.That(abst.Tree[0], Is.EqualTo(key));
            Assert.That(abst.Count, Is.EqualTo(expectedCount));
        }
        
        [Test]
        public void AddKey_WhenTreeExistsKey_KeyExistsOnTreeCountNotChangedAndReturnExpectedIndex()
        {
            aBST abst = new aBST(3);
            int key = 4;
            abst.AddKey(8);
            abst.AddKey(key);
            abst.AddKey(12);
            int expectedIndex = 1;
            int expectedCount = abst.Count;
            
            Assert.That(abst.AddKey(key), Is.EqualTo(expectedIndex));
            Assert.That(abst.Tree[expectedIndex], Is.EqualTo(key));
            Assert.That(abst.Count, Is.EqualTo(expectedCount));
        }
        
        [Test]
        public void AddKey_WhenFilledTreeDoesNotExistsKeyButExistPlace_KeyAddedAndReturnExpectedIndex()
        {
            aBST abst = new aBST(3);
            int key = 4;
            abst.AddKey(8);
            abst.AddKey(12);
            int expectedIndex = 1;
            int expectedCount = abst.Count + 1;
            
            Assert.That(abst.AddKey(key), Is.EqualTo(expectedIndex));
            Assert.That(abst.Tree[expectedIndex], Is.EqualTo(key));
            Assert.That(abst.Count, Is.EqualTo(expectedCount));
        }
        
        [Test]
        public void AddKey_WhenTreeIsFull_KeyNotAddedToTreeCountNotChangedAndReturnExpectedIndex()
        {
            aBST abst = new aBST(2);
            int key = 15;
            abst.AddKey(8);
            abst.AddKey(4);
            abst.AddKey(12);
            abst.AddKey(2);
            abst.AddKey(6);
            abst.AddKey(10);
            abst.AddKey(14);
            int expectedIndex = -1;
            int expectedCount = abst.Count;
            
            Assert.That(abst.AddKey(key), Is.EqualTo(expectedIndex));
            Assert.That(abst.Tree.Contains(key), Is.False);
            Assert.That(abst.Count, Is.EqualTo(expectedCount));
        }

        #endregion

        #region GetLowestCommonAncestor

        [TestCase(1, 13, 8)]
        [TestCase(1, 7, 4)]
        [TestCase(5, 7, 6)]
        [TestCase(1, 12, 8)]
        [TestCase(1, 6, 4)]
        [TestCase(6, 6, 6)]
        [TestCase(12, 14, 12)]
        [TestCase(13, 14, 14)]
        public void GetLowestCommonAncestorIndexTest(int firstKey, int secondKey, int expectedLca)
        {
            aBST abst = CreateTree(out List<int> keys);
            
            Assert.That(abst.GetLowestCommonAncestorWithIndexes(firstKey, secondKey), Is.EqualTo(expectedLca));
        }
        
        [TestCase(1, 13, 8)]
        [TestCase(1, 7, 4)]
        [TestCase(5, 7, 6)]
        [TestCase(1, 12, 8)]
        [TestCase(1, 6, 4)]
        [TestCase(6, 6, 6)]
        [TestCase(12, 14, 12)]
        [TestCase(13, 14, 14)]
        public void GetLowestCommonAncestorKeyTest(int firstKey, int secondKey, int expectedLca)
        {
            aBST abst = CreateTree(out List<int> keys);
            
            Assert.That(abst.GetLowestCommonAncestorKey(firstKey, secondKey), Is.EqualTo(expectedLca));
        }

        [Test]
        public void WideAllNodes_WithFullTree()
        {
            aBST abst = CreateTree(out List<int> keys);
            List<int> expectedKeysOrder = new List<int>() { 8, 4, 12, 2, 6, 10, 14, 1, 3, 5, 7, 9, 11, 13, 15 };
            
            Assert.That(abst.WideAllNodes(), Is.EqualTo(expectedKeysOrder));
        }
        
        [Test]
        public void WideAllNodes_WithNotFullTree()
        {
            aBST abst = new aBST(3);
            List<int> expectedKeysOrder = new List<int>() { 8, 4, 12, 2, 14, 1, 15 };

            foreach (int key in expectedKeysOrder)
            {
                abst.AddKey(key);
            }
            
            Assert.That(abst.WideAllNodes(), Is.EqualTo(expectedKeysOrder));
        }

        #endregion
        
        /*
         *             8
         *        /        \
         *       4         12
         *     /  \       /  \
         *    2    6    10   14
         *   / \  / \  / \  /  \
         *  1  3 5  7 9 11 13  15
         */
        private static aBST CreateTree(out List<int> nodes)
        {
            List<int> sortedKeys = Enumerable.Range(1, 15).ToList();
    
            List<int> insertionOrder = new List<int>();
            
            FillBalanced(sortedKeys, ref insertionOrder);

            aBST tree = new aBST(3);
            nodes = new List<int>();

            foreach (int key in insertionOrder)
            {
                tree.AddKey(key);
                nodes.Add(key);
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

