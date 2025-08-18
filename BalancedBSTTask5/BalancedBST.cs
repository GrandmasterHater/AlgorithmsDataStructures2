using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures2
{
    public static class BalancedBST
    {
        //Exercise 5, task 1, time complexity O(n), space complexity O(n)
        public static int[] GenerateBBSTArray(int[] a)
        {
            Array.Sort(a);
            int[] tree = new int[a.Length];
            
            GenerateBSTArrayRecursive(a, 0, 0, a.Length - 1, tree);
            
            return tree;
        }
        
        public static int CalculateArraySize(int depth)
        {
            return Convert.ToInt32(Math.Pow(2, depth + 1) - 1);
        }

        public static void GenerateBSTArrayRecursive(int[] orderedArray, int currentIndex, int leftIndex, int rightIndex, int[] treeArray)
        {
            if (leftIndex > rightIndex) 
                return;

            int middleIndex = Convert.ToInt32(Math.Ceiling((leftIndex + rightIndex) / 2.0d));
            treeArray[currentIndex] = orderedArray[middleIndex];
            
            GenerateBSTArrayRecursive(orderedArray, GetLeftChildIndex(currentIndex), leftIndex, middleIndex - 1, treeArray);
            GenerateBSTArrayRecursive(orderedArray, GetRightChildIndex(currentIndex),middleIndex + 1, rightIndex, treeArray);
        }
        
        public static int GetLeftChildIndex(int index)
        {
            return 2 * index + 1;
        }

        public static int GetRightChildIndex(int index)
        {
            return 2 * index + 2;
        }
    }
}

