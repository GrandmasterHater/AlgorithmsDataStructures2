using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures2
{
    public class aBST
    {
        public int? [] Tree;
        
        public int Count { get; private set; }
	
        public aBST(int depth)
        {
            int tree_size = CalculateArraySize(depth);
            Tree = new int?[ tree_size ];
            for(int i=0; i<tree_size; i++) Tree[i] = null;
            Count = 0;
        }
	
        //Exercise 4, task 1, time complexity O(n), space complexity O(1)
        public int? FindKeyIndex(int key)
        {
            int? requiredIndex = null;
            bool isLeft = false;

            for (int index = 0; index < Tree.Length; index = isLeft ? ABST_2.GetLeftChildIndex(index) : ABST_2.GetRightChildIndex(index))
            {
                int? currentKey = Tree[index];

                if (!currentKey.HasValue)
                {
                    requiredIndex = -1 * index;
                    break;
                }

                if (currentKey.Value == key)
                {
                    requiredIndex = index;
                    break;
                }
                
                isLeft = currentKey.Value > key;
            }
            
            return requiredIndex;
        }
        //Exercise 4, task 1, time complexity O(n), space complexity O(1)
        public int AddKey(int key)
        {
            int? index = FindKeyIndex(key);
            
            bool canAddToTree = index.HasValue && (index == 0 && Count == 0 || index < 0 );
            int resultIndex = !index.HasValue ? -1 : Math.Abs(index.Value);

            if (canAddToTree)
            {
                Tree[resultIndex] = key;
                ++Count;
            }
            
            return resultIndex;
        }

        //Exercise 4, task 3, time complexity O(n), space complexity O(n) + O(w) where w - tree width
        public List<int> WideAllNodes()
        {
            List<int> result = new List<int>();

            if (Count == 0)
                return result;
            
            Queue<int> levelKeyIndexes = new Queue<int>();
            levelKeyIndexes.Enqueue(0);
            
            while (levelKeyIndexes.Count > 0)
            {
                int currentIndex = levelKeyIndexes.Dequeue();
                result.Add(Tree[currentIndex].Value);
                
                int leftChildIndex = ABST_2.GetLeftChildIndex(currentIndex);
                int rightChildIndex = ABST_2.GetRightChildIndex(currentIndex);

                if (leftChildIndex < Tree.Length && Tree[leftChildIndex].HasValue)
                    levelKeyIndexes.Enqueue(leftChildIndex);
                
                if (rightChildIndex < Tree.Length && Tree[rightChildIndex].HasValue)
                    levelKeyIndexes.Enqueue(rightChildIndex);
            }

            return result;
        }

        private int CalculateArraySize(int depth)
        {
            return Convert.ToInt32(Math.Pow(2, depth + 1) - 1);
        }
    }
}