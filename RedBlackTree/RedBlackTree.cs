using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace RedBlackTree
{
    public class RedBlackTree<TKeyValue> where TKeyValue : IComparable
    {
        #region NodeClass
        private class Node
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
        }
        #endregion

        #region NodeColorEnum
        public enum NodeColor
        {
            RED,
            BLACK
        }
        #endregion

        private Node? root; // If a tree is empty, root is a null (it is sentinel node).
        private int numberOfElements;

        public RedBlackTree()
        {
            root = null;
            numberOfElements = 0;
        }

        private List<Node>? GetNodesInOrder(Node subtreeRoot)
        {
            if (subtreeRoot is null)
                return null;
            List<Node> nodes = new();
            return nodes;
        }

        private static bool IsLessThan(TKeyValue firstOperand, TKeyValue secondOperand)
        {
            return firstOperand.CompareTo(secondOperand) < 0;
        }

        public override string ToString()
        {
            return "";/*Nodes.Select(node => node.ToString() + (node.Index == Nodes.Count - 1 ? "" : "\n"))
                        .Aggregate((result, value) => result + value);*/
        }

        public void Insert(TKeyValue value)
        {
            Node? currentNode = root;
            Node? currentNodeParent = null;
            while (currentNode is not null)
            {
                currentNodeParent = currentNode;
                currentNode = IsLessThan(value, currentNode.KeyValue) ? currentNode.LeftChild : currentNode.RightChild;
            }
            Node newNode = new(value, currentNodeParent);
            if (currentNodeParent is null)
                root = newNode;
            else if (IsLessThan(newNode.KeyValue, currentNodeParent.KeyValue))
                currentNodeParent.LeftChild = newNode;
            else
                currentNodeParent.RightChild = newNode;
            newNode.Index = numberOfElements;
            numberOfElements++;
            // Fix!
        }

        private void RotateLeft(Node criticalNode)
        {
            Node rotationNode = criticalNode.RightChild ?? throw new InvalidOperationException("Unable to do left rotation.");
            criticalNode.RightChild = rotationNode?.LeftChild;
            if (rotationNode?.LeftChild is not null)
                rotationNode.LeftChild.Parent = criticalNode;
            rotationNode.Parent = criticalNode.Parent; // The rotation node can not be a null (first line is throwing exception in case it is).
            if (criticalNode.Parent is null)
                root = rotationNode;
            else if (criticalNode == criticalNode.Parent.LeftChild)
                criticalNode.Parent.LeftChild = rotationNode;
            else
                criticalNode.Parent.RightChild = rotationNode;
            rotationNode.LeftChild = criticalNode;
            criticalNode.Parent = rotationNode;
        }

        private void RotateRight([DisallowNull] Node criticalNode)
        {
            Node rotationNode = criticalNode.LeftChild ?? throw new InvalidOperationException("Unable to do right rotation.");
            criticalNode.LeftChild = rotationNode?.RightChild;
            if (rotationNode?.RightChild is not null)
                rotationNode.RightChild.Parent = criticalNode;
            rotationNode.Parent = criticalNode.Parent; // The rotation node can not be a null (first line is throwing exception in case it is).
            if (criticalNode.Parent is null)
                root = rotationNode;
            else if (criticalNode == criticalNode.Parent.LeftChild)
                criticalNode.Parent.LeftChild = rotationNode;
            else
                criticalNode.Parent.RightChild = rotationNode;
            rotationNode.RightChild = criticalNode;
            criticalNode.Parent = rotationNode;
        }

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
        }
    }
}