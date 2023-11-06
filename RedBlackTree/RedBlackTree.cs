using System.Runtime.CompilerServices;

namespace RedBlackTree
{
    public class RedBlackTree<TKeyValue> where TKeyValue : IComparable
    {
        public Node? Root { get; private set; } // If a tree is empty, root is a null (it is sentinel node).

        public List<Node> Nodes { get; private set; }
        public int NumberOfElements { get; private set; }

        public RedBlackTree()
        {
            Root = null;
            NumberOfElements = 0;
            Nodes = new List<Node>();
        }

        private static bool IsLessThan(TKeyValue firstOperand, TKeyValue secondOperand)
        {
            return firstOperand.CompareTo(secondOperand) < 0;
        }

        public override string ToString()
        {
            string result = "";
            foreach (Node node in Nodes)
                result += node.ToString() + (node.Index == Nodes.Count - 1 ? "" : "\n");
            return result;
        }

        public void Insert(TKeyValue value)
        {
            Node? currentNode = Root;
            Node? currentNodeParent = null;
            while (currentNode is not null)
            {
                currentNodeParent = currentNode;
                currentNode = IsLessThan(value, currentNode.KeyValue) ? currentNode.LeftChild : currentNode.RightChild;
            }
            Node newNode = new(value, currentNodeParent);
            if (currentNodeParent is null)
                Root = newNode;
            else if (IsLessThan(newNode.KeyValue, currentNodeParent.KeyValue))
                currentNodeParent.LeftChild = newNode;
            else
                currentNodeParent.RightChild = newNode;
            Nodes.Add(newNode);
            newNode.Index = Nodes.Count - 1;
            // Fix!
        }

        public class Node
        {
            public int Index { get; set; } // This property is made for testing purposes.
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

            public override string ToString()
            {
                string LeftChildString = LeftChild is null ? "null" : $"{LeftChild.Index}";
                string RightChildString = RightChild is null ? "null" : $"{RightChild.Index}";
                string ParentString = Parent is null ? "null" : $"{Parent.Index}";
                return $"Index: {Index}; Value: {KeyValue}; Color: {Color}; LeftChild: {LeftChildString}; RightChild: {RightChildString}; Parent: {ParentString}";
            }

            public enum NodeColor
            {
                RED,
                BLACK
            }
        }
    }
}