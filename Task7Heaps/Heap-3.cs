using System;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmsDataStructures2.Task7Heaps
{
    [TestFixture]
    public class Heap_3
    {
        #region MakeHeap

        [TestCase(0, 1)]
        [TestCase(1, 3)]
        [TestCase(2, 7)]
        [TestCase(3, 15)]
        public void MakeHeap_WhenCalled_ArraySizeCalculatedByDepth(int depth, int expectedSize)
        {
            Heap heap = CreateHeapWithDepth(depth, Array.Empty<int>());

            Assert.That(heap.HeapArray.Length, Is.EqualTo(expectedSize));
        }
        
        [Test]
        public void MakeHeap_PassNotEmptyInputArray_ArrayContainsAllElementsFromInputAndCountIsEqualToInputLength()
        {
            int[] input = { 5, 3, 8, 1 };
            Heap heap = CreateHeapWithDepth(2, input);

            Assert.That(heap.HeapArray.Take(heap.Count), Is.EquivalentTo(input));
            Assert.That(heap.Count, Is.EqualTo(input.Length));
        }

        #endregion

        #region GetMax

        [Test]
        public void GetMax_WhenHeapIsEmpty_ReturnsMinusOne()
        {
            Heap heap = CreateHeapWithDepth(2, Array.Empty<int>());

            Assert.That(heap.GetMax(), Is.EqualTo(-1));
        }
        
        [Test]
        public void GetMax_WhenHeapHasElements_ReturnsMaxAndDecreasesCount()
        {
            int expectedMaxValue = 8;
            Heap heap = CreateHeapWithDepth(2, new []{5, 3, expectedMaxValue, 1});
            int expectedCount = heap.Count - 1;

            Assert.That(heap.GetMax(), Is.EqualTo(expectedMaxValue));
            Assert.That(heap.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(new[] { 10, 5, 7, 6, 4, 3, 1 }, 10, new[] { 5, 7, 6, 4, 3, 1 })] // полностью заполненная куча из 2 уровней
        [TestCase(new[] { 10, 5, 7, 6 }, 10, new[] { 5, 7, 6 })] // частично заполненная куча из 2 уровней
        public void GetMax_WhenHeapHasElements_ReplacesRootAndMoveDown(
            int[] input, int expectedMax, int[] expectedHeap)
        {
            Heap heap = CreateHeapWithDepth(2, input);

            int max = heap.GetMax();

            Assert.That(max, Is.EqualTo(expectedMax));
            Assert.That(heap.HeapArray.Take(heap.Count), Is.EquivalentTo(expectedHeap));
            Assert.That(heap.AreHeapPropertiesRespected(), Is.True);
        }

        #endregion

        #region MyRegion
        
        [Test]
        public void Add_WhenHeapIsFull_ReturnFalse()
        {
            Heap heap = new Heap();
            
            heap.MakeHeap(new int[] {1, 2, 3, 4, 5, 6, 7}, 2);
            
            Assert.That(heap.Add(8), Is.False);
        }
        
        [TestCase(new int[] { }, 1, 7, new[] { 7 })]
        [TestCase(new[] { 10, 5 }, 1, 7, new[] { 10, 5, 7 })]     // 1 свободное место
        [TestCase(new[] { 10 }, 2, 7, new[] { 10, 7 })]        // несколько свободных мест
        public void Add_WhenHeapNotFull_ElementInsertedAndCountIncreased(
            int[] initialElements, int depth, int expectedKey, int[] expectedHeap)
        {
            var heap = CreateHeapWithDepth(depth, initialElements);
            int expectedCount = heap.Count + 1;

            bool result = heap.Add(expectedKey);

            Assert.That(result, Is.True);
            Assert.That(heap.Count, Is.EqualTo(expectedCount));
            Assert.That(heap.HeapArray.Take(heap.Count), Is.EquivalentTo(expectedHeap));
        }
        
        #endregion

        #region AreHeapPropertiesRespected 

        [TestCase(new int[] { }, 1)]
        [TestCase(new[] { 10, 5, 7 }, 1)]
        [TestCase(new[] { 20, 15 }, 2)]
        [TestCase(new[] { 20, 15 }, 2)]
        public void AreHeapPropertiesRespected_WhenHeapIsValid_ReturnsTrue(int[] input, int depth)
        {
            Heap heap = CreateHeapWithDepth(depth, input);

            Assert.That(heap.AreHeapPropertiesRespected(), Is.True);
        }

        [Test]
        public void AreHeapPropertiesRespected_WhenHeapIsInvalid_ReturnsFalse()
        {
            Heap heap = CreateHeapWithDepth(2, new int[] {1, 2, 3, 4, 5});
            
            (heap.HeapArray[0], heap.HeapArray[1]) = (heap.HeapArray[1], heap.HeapArray[0]);

            Assert.That(heap.AreHeapPropertiesRespected(), Is.False);
        }

        #endregion

        #region GetMaxInRa

        [Test]
        public void GetMaxInRange_WhenHeapIsEmpty_ReturnsMinusOne()
        {
            var heap = CreateHeapWithDepth(1, Array.Empty<int>());

            Assert.That(heap.GetMaxInRange(1, 10), Is.EqualTo(-1));
        }

        [TestCase(new[] { 10, 5, 7, 2 }, 1, 5, 5)]   // элемент в начале массива
        [TestCase(new[] { 10, 5, 7, 2 }, 7, 10, 10)] // элемент в конце массива
        [TestCase(new[] { 10, 5, 7, 2 }, 6, 8, 7)]   // элемент в середине массива
        public void GetMaxInRange_WhenHeapHasElements_ReturnsCorrectElement(int[] input, int min, int max, int expected)
        {
            var heap = CreateHeapWithDepth(2, input);

            Assert.That(heap.GetMaxInRange(min, max), Is.EqualTo(expected));
        }

        #endregion

        #region FindLessThanValue

        [Test]
        public void FindLessThanValue_WhenHeapIsEmpty_ReturnsMinusOne()
        {
            var heap = CreateHeapWithDepth(1, Array.Empty<int>());

            Assert.That(heap.FindLessThanValue(5), Is.EqualTo(-1));
        }

        [TestCase(new[] { 10, 5, 7, 2 }, 11, 10)]   // начало массива
        [TestCase(new[] { 10, 5, 7, 2 }, 3, 2)] // конец массива (правое поддерево)
        [TestCase(new[] { 10, 9, 8, 2 }, 9, 8)]  // середина массива (правое поддерево)
        [TestCase(new[] { 10, 5, 7, 2 }, 6, 5)]   // середина массива (левое поддерево)
        public void FindLessThanValue_WhenHeapHasElements_ReturnsClosestSmallerElement(int[] input, int target, int expected)
        {
            var heap = CreateHeapWithDepth(2, input);

            Assert.That(heap.FindLessThanValue(target), Is.EqualTo(expected));
        }

        #endregion

        #region Union

        [Test]
        public void Union_WhenTwoHeapsMerged_FirstHeapContainsAllElements()
        {
            int[] firstTreeNodes = new[] { 10, 5, 7 };
            int[] secondTreeNodes = new[] { 8, 3, 6 };
            
            var first = CreateHeapWithDepth(2, firstTreeNodes);
            var second = CreateHeapWithDepth(2, secondTreeNodes);

            first.Union(second);

            Assert.That(first.Count, Is.EqualTo(6));
            Assert.That(first.HeapArray.Take(first.Count), Is.SupersetOf(firstTreeNodes));
            Assert.That(first.HeapArray.Take(first.Count), Is.SupersetOf(secondTreeNodes));
            Assert.That(first.AreHeapPropertiesRespected(), Is.True);
        }

        #endregion
        
        private Heap CreateHeapWithDepth(int depth, int[] elements)
        {
            var heap = new Heap();
            heap.MakeHeap(elements, depth);
            return heap;
        }
    }
}

