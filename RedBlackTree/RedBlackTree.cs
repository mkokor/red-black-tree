namespace RedBlackTree
{
    public class RedBlackTree<TKeyValue> where TKeyValue : IComparable
    {
        private readonly Node? root; // If a tree is empty, root is a null (it is sentinel node).
        private int numberOfElements;

        public RedBlackTree()
        {
            root = null;
            numberOfElements = 0;
        }

        public void Insert(TKeyValue value)
        {
        }

        private class Node
        {
            public TKeyValue KeyValue { get; set; }
            public NodeColor Color { get; set; }
            public Node? LeftChild { get; set; }
            public Node? RightChild { get; set; }
            public Node? Parent { get; set; } // The root node has sentinel node as a parent, so this property can be a null.

            public Node(TKeyValue keyValue, Node? parent)
            {
                KeyValue = keyValue;
                Color = NodeColor.RED; // A new node is always red.
                Parent = parent;
                LeftChild = RightChild = null; // The sentinental node is left and right child of a new node. 
            }

            public enum NodeColor
            {
                RED,
                BLACK
            }
        }
    }
}