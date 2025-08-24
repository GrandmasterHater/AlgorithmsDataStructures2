using System;
using System.Collections.Generic;
using System.Dynamic;

namespace AlgorithmsDataStructures2
{
    public class Heap
    {
        public int [] HeapArray; // хранит неотрицательные числа-ключи
		
        public Heap() { HeapArray = null; }

        public int Count { get; private set; }

        // Exercise 7, task 1, time complexity O(n log n), space complexity O(2^d) where d - heap depth
        public void MakeHeap(int[] a, int depth)
        {
            int size = CalculateArraySize(depth);
            HeapArray = new int[size];
            Count = 0;
            
            for (int i = 0; i < a.Length; ++i)
            {
                Add(a[i]);
            }
        }
		
        // Exercise 7, task 2, time complexity O(log n), space complexity O(h) where h - tree height
        public int GetMax()
        {
            if (Count == 0)
                return -1;

            int max = HeapArray[0];
            --Count;
            HeapArray[0] = HeapArray[Count];
            MoveDownRecursive(0);
            
            return max;
        }

        // Exercise 7, task 2, time complexity O(log n), space complexity O(h) where h - tree height
        public bool Add(int key)
        {
            if (Count == HeapArray.Length)
                return false;

            int insertIndex = Count;
            HeapArray[insertIndex] = key;
            MoveUpRecursive(insertIndex);
            ++Count;
            
            return true;
        }
        
        // Exercise 7, task 3, time complexity O(n), space complexity O(1)
        public bool AreHeapPropertiesRespected()
        {
            for (int i = 0; i < Count; i++)
            {
                int leftChild = GetLeftChildIndex(i);
                int rightChild = GetRightChildIndex(i);
                
                bool isNodeCorrect = (leftChild >= Count || HeapArray[i] >= HeapArray[leftChild]) 
                    && (rightChild >= Count || HeapArray[i] >= HeapArray[rightChild]);

                if (!isNodeCorrect)
                    return false;
            }

            return true;
        }
        
        public int GetLeftChildIndex(int index)
        {
            return 2 * index + 1;
        }

        public int GetRightChildIndex(int index)
        {
            return 2 * index + 2;
        }

        private void MoveUpRecursive(int index)
        {
            int parentIndex = GetParentIndex(index);
            
            bool isSwapAvailable = parentIndex >= 0 && HeapArray[parentIndex] < HeapArray[index];

            if (isSwapAvailable)
                (HeapArray[parentIndex], HeapArray[index]) = (HeapArray[index], HeapArray[parentIndex]);
            else
                return;

            MoveUpRecursive(parentIndex);
        }

        private void MoveDownRecursive(int index)
        {
            int leftChildIndex = GetLeftChildIndex(index);
            int rightChildIndex = GetRightChildIndex(index);
            
            if (leftChildIndex >= Count)
                return;
            
            int maxChildValueIndex = rightChildIndex >= Count || HeapArray[leftChildIndex] > HeapArray[rightChildIndex] 
                ? leftChildIndex 
                : rightChildIndex;
            
            if (HeapArray[index] >= HeapArray[maxChildValueIndex])
                return;
            
            (HeapArray[index], HeapArray[maxChildValueIndex]) = (HeapArray[maxChildValueIndex], HeapArray[index]);
            
            MoveDownRecursive(maxChildValueIndex); 
        }
        
        private int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }
        
        private int CalculateArraySize(int depth)
        {
            return Convert.ToInt32(Math.Pow(2, depth + 1) - 1);
        }
    }
}

