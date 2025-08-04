namespace AlgorithmsDataStructures2
{
    public class SimpleTreeTask1_2
    {
        // Exercise 1, task 1, time complexity O(n), space complexity O(h) where h - tree height
        public static void CalculateNodeLevel<T>(SimpleTree<T> tree)
        {
            if (tree?.Root == null)
                return;
            
            CalculateNodeLevelRecursive(tree.Root, 0);
        }

        private static void CalculateNodeLevelRecursive<T>(SimpleTreeNode<T> node, int level)
        {
            node.Level = level;
            
            if (node.IsLeaf)
                return;

            foreach (var childNode in node.Children)
            {
                CalculateNodeLevelRecursive(childNode, level + 1);
            }
        }
        
        /*
         *  Exercise 1, task 2, time complexity O(1), space complexity O(1)
         *  1. Calculate node level from parent node as Parent.Level + 1.
         *  2. Use dictionary where key is node level and value is list of nodes with this level.
         */
    }
}