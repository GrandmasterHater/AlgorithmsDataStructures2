using System;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmsDataStructures2.Task5
{
    [TestFixture]
    public class BalancedBST_3
    {
        [TestCase(1, new []{2, 1, 3})]
        [TestCase(2, new []{4, 2, 6, 1, 3, 5, 7})]
        [TestCase(3, new []{8, 4, 12, 2, 6, 10, 14, 1, 3, 5, 7, 9, 11, 13, 15})]
        public void GenerateBBSTArrayTest(int depth, int[] expectedNumbers)
        {
            int arraySize = BalancedBST.CalculateArraySize(depth);
            int[] numbers = Enumerable.Range(1, arraySize).ToArray();
            int[] array = Shuffle(numbers.ToArray());
            int[] tree = BalancedBST.GenerateBBSTArray(array);
            
            Assert.That(tree, Is.EqualTo(expectedNumbers));
        }
        
        [TestCase(new int[] { 8, 4, 12, 2, 6, 10, 14 }, 15)]
        public void RemoveKey_WhenKeyNotExists_ReturnOriginalArray(int[] tree, int key)
        {
            Assert.That(BalancedBST_2.RemoveKey(tree, key), Is.EqualTo(tree));
        }
        
        [TestCase(new int[] { 8, 4, 12, 2, 6, 10, 14 }, 4, new int[] { 10, 6, 14, 2, 8, 12 })]
        [TestCase(new int[] { 8, 4, 12, 2, 6, 10, 14, 1, 3, 5, 7, 9, 11, 13, 15}, 4, new int[] { 9, 5, 13, 2, 7, 11, 15, 1, 3, 6, 8, 10, 12, 14 })]
        public void RemoveKey_WhenKeyExists_ReturnNewBalancedTreeWithoutKey(int[] tree, int key, int[] expectedTree)
        {
            Assert.That(BalancedBST_2.RemoveKey(tree, key), Is.EqualTo(expectedTree));
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
    }
}

