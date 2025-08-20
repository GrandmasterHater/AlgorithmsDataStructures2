namespace AlgorithmsDataStructures2
{
    public static class BalancedBST_2
    {
        // Exercise 6, task 2, time complexity O(n), space complexity O(h) where h - tree height
        public static bool IsTreeCorrect(this BalancedBST tree)
        {
            return IsTreeCorrectRecursive(tree.Root);
        }

        public static bool IsTreeCorrectRecursive(BSTNode node)
        {
            if (node == null)
                return true;
            
            bool isNodeCorrect = (node.LeftChild == null || node.LeftChild.NodeKey < node.NodeKey) 
                && (node.RightChild == null || node.RightChild.NodeKey >= node.NodeKey);

            return isNodeCorrect && IsTreeCorrectRecursive(node.LeftChild) && IsTreeCorrectRecursive(node.RightChild);
        }
    }
}

