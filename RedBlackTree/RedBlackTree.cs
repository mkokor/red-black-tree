using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using RedBlackTree.Exceptions;

namespace RedBlackTree
{
    public class RedBlackTree<TKeyValue> where TKeyValue : IComparable
    {
        #region NodeClass
        private class Node
        {
            public int Index { get; set; } // This property is made for testing purposes.
            public TKeyValue? KeyValue { get; set; }
            public NodeColor Color { get; set; }

            // The sentinel does not have any child or a parent so following properties must be nullable.
            public Node? LeftChild { get; set; }
            public Node? RightChild { get; set; }
            public Node? Parent { get; set; }

            public Node(TKeyValue? keyValue, Node? parent, Node? sentinel, NodeColor color = NodeColor.RED)  // A new node is always red.
            {
                KeyValue = keyValue;
                Color = color;
                Parent = parent;
                LeftChild = RightChild = sentinel; // The sentinel is left and right child of a new node. 
            }

            public override string ToString()
            {
                string LeftChildString = LeftChild is null ? "null" : $"{LeftChild.Index}";
                string RightChildString = RightChild is null ? "null" : $"{RightChild.Index}";
                string ParentString = Parent is null ? "null" : $"{Parent.Index}";
                return $"Index: {Index}; Value: {KeyValue}; Color: {Color}; LeftChild: {LeftChildString}; RightChild: {RightChildString}; Parent: {ParentString}";
            }
        }
        #endregion

        #region NodeColorEnum
        public enum NodeColor
        {
            RED,
            BLACK
        }
        #endregion

        private Node root; // If a tree is empty, root is a null (it is sentinel node).
        private readonly Node _sentinel;
        private int numberOfElements;

        public RedBlackTree()
        {
            numberOfElements = 0;
            _sentinel = new Node(default, null, null, NodeColor.BLACK);
            root = _sentinel;
        }
        /*
                private List<Node>? GetNodesInOrder(Node subtreeRoot)
                {
                    if (subtreeRoot is null)
                        return null;
                    List<Node> nodes = new();
                    return nodes;
                }*/

        private static bool IsLessThan(TKeyValue firstOperand, TKeyValue secondOperand)
        {
            _ = firstOperand ?? throw new ArgumentNullException(paramName: nameof(firstOperand));
            _ = secondOperand ?? throw new ArgumentNullException(paramName: nameof(secondOperand));
            return firstOperand?.CompareTo(secondOperand) < 0;
        }

        public override string ToString()
        {
            return "";/*Nodes.Select(node => node.ToString() + (node.Index == Nodes.Count - 1 ? "" : "\n"))
                        .Aggregate((result, value) => result + value);*/
        }

        public void Insert(TKeyValue value)
        {
            Node currentNode = root;
            Node currentNodeParent = _sentinel;
            while (currentNode != _sentinel)
            {
                currentNodeParent = currentNode!;
                currentNode = IsLessThan(value, currentNode.KeyValue!) ? currentNode.LeftChild! : currentNode.RightChild!;
            }
            Node newNode = new(value, currentNodeParent, _sentinel);
            if (currentNodeParent == _sentinel)
                root = newNode;
            else if (IsLessThan(newNode.KeyValue!, currentNodeParent.KeyValue!))
                currentNodeParent.LeftChild = newNode;
            else
                currentNodeParent.RightChild = newNode;
            newNode.Index = numberOfElements;
            numberOfElements++;
            // Fix!
        }

        private void RotateLeft(Node criticalNode)
        {
            if (criticalNode.RightChild == _sentinel) throw new InvalidOperationException("Left rotation can not be done.");
            Node rotationNode = criticalNode.RightChild!;
            criticalNode.RightChild = rotationNode.LeftChild;
            if (rotationNode.LeftChild != _sentinel)
                rotationNode.LeftChild!.Parent = criticalNode;
            rotationNode.Parent = criticalNode.Parent;
            if (criticalNode.Parent == _sentinel)
                root = rotationNode;
            else if (criticalNode == criticalNode.Parent!.LeftChild)
                criticalNode.Parent.LeftChild = rotationNode;
            else
                criticalNode.Parent.RightChild = rotationNode;
            rotationNode.LeftChild = criticalNode;
            criticalNode.Parent = rotationNode;
        }

        private void RotateRight([DisallowNull] Node criticalNode)
        {
            if (criticalNode.LeftChild == _sentinel) throw new InvalidOperationException("Right rotation can not be done.");
            Node rotationNode = criticalNode.LeftChild!;
            criticalNode.LeftChild = rotationNode.RightChild;
            if (rotationNode.RightChild != _sentinel)
                rotationNode.RightChild!.Parent = criticalNode;
            rotationNode.Parent = criticalNode.Parent;
            if (criticalNode.Parent == _sentinel)
                root = rotationNode;
            else if (criticalNode == criticalNode.Parent!.LeftChild)
                criticalNode.Parent.LeftChild = rotationNode;
            else
                criticalNode.Parent.RightChild = rotationNode;
            rotationNode.RightChild = criticalNode;
            criticalNode.Parent = rotationNode;
        }
        /*
        private void FixTreeStructure([DisallowNull] Node criticalNode)
        {
            while (criticalNode.Parent is not null && criticalNode.Parent.Color == NodeColor.RED)
                    {
                        Node? criticalNodeUncle;
                        if (criticalNode.Parent == criticalNode.Parent.Parent?.LeftChild)
                        {
                            criticalNodeUncle = criticalNode.Parent.RightChild;
                            if (criticalNodeUncle?.Color == NodeColor.RED)
                            {
                                criticalNode.Parent.Color = NodeColor.BLACK;
                                criticalNodeUncle.Color = NodeColor.BLACK;
                                criticalNode.Parent.Parent.Color = NodeColor.RED;
                                criticalNode = criticalNode.Parent.Parent;
                            }
                            else
                            {
                                if (criticalNode == criticalNode.Parent.RightChild)
                                {
                                    criticalNode = criticalNode.Parent;
                                    RotateLeft(criticalNode);
                                }
                                criticalNode.Parent.Color = NodeColor.BLACK;
                                criticalNode.Parent.Parent.Color = NodeColor.RED;
                                RotateRight(criticalNode.Parent.Parent);
                            }
                        }
                    }
            }*/
    }
}