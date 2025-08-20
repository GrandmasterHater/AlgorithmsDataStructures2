using System;
using System.Linq;

namespace AlgorithmsDataStructures2.Task5
{
    public static class BalancedBST_2
    {
        //Exercise 5, task 3, time complexity O(n log n), space complexity O(n)
        public static int[] RemoveKey(int[] tree, int key)
        {
            int indexToRemove = FindKeyIndex(tree, key);

            if (indexToRemove == -1)
                return tree;
            
            (tree[indexToRemove], tree[tree.Length - 1]) = (tree[tree.Length - 1], tree[indexToRemove]);
            int newSize = tree.Length - 1;
            Array.Sort(tree, 0, newSize);
            
            int[] newTree = new int[newSize];
            BalancedBST.GenerateBSTArrayRecursive(tree, 0,0, newSize - 1, newTree);
            
            return newTree;
        }
        
        private static int FindKeyIndex(int[] tree, int key)
        {
            int requiredIndex = -1;
            bool isLeft = false;

            for (int index = 0; index < tree.Length; index = isLeft ? BalancedBST.GetLeftChildIndex(index) : BalancedBST.GetRightChildIndex(index))
            {
                int currentKey = tree[index];
                
                if (currentKey == key)
                {
                    requiredIndex = index;
                    break;
                }
                
                isLeft = currentKey > key;
            }
            
            return requiredIndex;
        }
    }
}

