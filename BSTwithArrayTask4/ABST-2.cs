using System;

namespace AlgorithmsDataStructures2
{
    public static class ABST_2
    {
        //Exercise 4, task 2, time complexity O(h) where h - tree height, space complexity O(1)
        public static int? GetLowestCommonAncestorWithIndexes(this aBST tree, int firstKey, int secondKey)
        {
            int? firstKeyIndex = tree.FindKeyIndex(firstKey);
            int? secondKeyIndex = tree.FindKeyIndex(secondKey);
            
            if (!firstKeyIndex.HasValue || !secondKeyIndex.HasValue)
                return null;
            
            for (int currentFirstIndex = firstKeyIndex.Value, currentSecondIndex = secondKeyIndex.Value;
                 currentFirstIndex >= 0 && currentSecondIndex >= 0;
                 (currentFirstIndex, currentSecondIndex) = GetNextParentIndexs(currentFirstIndex, currentSecondIndex))
            {
                if (currentFirstIndex == currentSecondIndex)
                    return tree.Tree[currentFirstIndex];
            }

            return null;
        }
        
        private static (int nextFirstIndex, int nextSecondIndex) GetNextParentIndexs(int currentFirstIndex, int currentSecondIndex)
        {
            return currentFirstIndex > currentSecondIndex
                ? (GetParentIndex(currentFirstIndex), currentSecondIndex)
                : (currentFirstIndex, GetParentIndex(currentSecondIndex));
        }
        
        //Exercise 4, task 2, time complexity O(n), space complexity O(h) where h - tree height
        public static int? GetLowestCommonAncestorKey(this aBST tree, int firstKey, int secondKey)
        {
            return GetLowestCommonAncestorKeyIndexRecursive(tree, 0, firstKey, secondKey);
        }

        private static int? GetLowestCommonAncestorKeyIndexRecursive(aBST tree, int currentIndex, int firstKey, int secondKey)
        {
            int? currentKey = tree.Tree[currentIndex];

            if (!currentKey.HasValue)
                return null;
            
            if (currentKey.Value >= Math.Min(firstKey, secondKey) && currentKey.Value <= Math.Max(firstKey, secondKey))
                return currentKey;
            
            int nextIndex = currentKey.Value > firstKey && currentKey.Value > secondKey 
                ? GetLeftChildIndex(currentIndex) 
                : GetRightChildIndex(currentIndex);
            
            return GetLowestCommonAncestorKeyIndexRecursive(tree, nextIndex, firstKey, secondKey);
        }
        
        public static int GetParentIndex(int index)
        {
            return (index - 1) / 2;
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