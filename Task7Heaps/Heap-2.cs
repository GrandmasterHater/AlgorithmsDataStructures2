using System;

namespace AlgorithmsDataStructures2.Task7Heaps
{
    public static class Heap_2
    {
        // Exercise 7, task 4, time complexity O(n), space complexity O(h) where h - tree height
        public static int GetMaxInRange(this Heap heap, int minValue, int maxValue)
        {
            const int NOT_EXISTS_VALUE = -1;
            
            if (heap.Count <= 0)
                return -1;
            
            return GetMaxInRangeRecursive(heap, 0, NOT_EXISTS_VALUE, minValue, maxValue);
        }

        // Exercise 7, task 5, time complexity O(n), space complexity O(h) where h - tree height
        public static int FindLessThanValue(this Heap heap, int targetValue)
        {
            const int NOT_EXISTS_VALUE = -1;
            
            if (heap.Count <= 0)
                return -1;
            
            return FindLessThanValueRecursive(heap, 0, NOT_EXISTS_VALUE, targetValue);
        }

        // Exercise 7, task 3, time complexity O(n log n), space complexity O(h) where h - tree height
        public static void Union(this Heap firstHeap, Heap secondHeap)
        {
            while (secondHeap.Count > 0)
            {
                firstHeap.Add(secondHeap.GetMax());
            }
        }

        private static int GetMaxInRangeRecursive(Heap heap, int index, int currentMaxValue, int minValue, int maxValue)
        {
            if (index >= heap.Count)
                return currentMaxValue;

            int currentValue = heap.HeapArray[index];

            if (currentValue < minValue)
                return currentMaxValue;
            
            if (currentValue > currentMaxValue && currentValue <= maxValue && currentValue >= minValue)
                currentMaxValue = currentValue;
            
            int leftChildIndex = heap.GetLeftChildIndex(index);
            int rightChildIndex = heap.GetRightChildIndex(index);

            int leftSubtreeMax = GetMaxInRangeRecursive(heap, leftChildIndex, currentMaxValue, minValue, maxValue);
            int rightSubtreeMax = GetMaxInRangeRecursive(heap, rightChildIndex, currentMaxValue, minValue, maxValue);
            
            return Math.Max(leftSubtreeMax, rightSubtreeMax);
        }
        
        private static int FindLessThanValueRecursive(Heap heap, int index, int currentMinValue, int targetValue)
        {
            if (index >= heap.Count)
                return currentMinValue;
            
            int currentValue = heap.HeapArray[index];

            if (currentValue < currentMinValue)
                return currentMinValue;
            
            if (currentValue > currentMinValue && currentValue < targetValue)
                currentMinValue = currentValue;
            
            int leftChildIndex = heap.GetLeftChildIndex(index);
            int rightChildIndex = heap.GetRightChildIndex(index);

            int leftSubtreeMin = FindLessThanValueRecursive(heap, leftChildIndex, currentMinValue, targetValue);
            int rightSubtreeMin = FindLessThanValueRecursive(heap, rightChildIndex, currentMinValue, targetValue);
            
            return Math.Max(leftSubtreeMin, rightSubtreeMin);
        }
    }
}

