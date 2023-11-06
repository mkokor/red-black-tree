namespace RedBlackTree
{
    public class RedBlackTree<KeyValueType> where KeyValueType : IComparable
    {
        private Node? root; // If a tree is empty, root is a null (it is sentinel node).
        private int numberOfElements;

        public RedBlackTree()
        {
            root = null;
            numberOfElements = 0;
        }

        private class Node
        {
            public KeyValueType KeyValue { get; set; }
            public NodeColor Color { get; set; }
            public Node? LeftChild { get; set; }
            public Node? RightChild { get; set; }
            public Node? Parent { get; set; } // The root node has sentinel node as a parent, so this property can be a null.

            public Node(KeyValueType keyValue, Node? parent)
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