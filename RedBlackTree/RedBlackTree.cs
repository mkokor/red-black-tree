using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Security.Cryptography.X509Certificates;
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
                string LeftChildString = LeftChild is null || LeftChild.Index == 0 ? "sentinel" : $"{LeftChild.Index}";
                string RightChildString = RightChild is null || RightChild.Index == 0 ? "sentinel" : $"{RightChild.Index}";
                string ParentString = Parent is null || Parent.Index == 0 ? "sentinel" : $"{Parent.Index}";
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
        private int uniqueIdentifierGenerator;

        public RedBlackTree()
        {
            uniqueIdentifierGenerator = 0;
            _sentinel = new Node(default, null, null, NodeColor.BLACK);
            root = _sentinel;
        }

        private List<Node> GetNodesInorder(Node treeRoot)
        {
            if (treeRoot == _sentinel)
                return new List<Node>();
            List<Node> nodes = new();
            nodes.AddRange(GetNodesInorder(treeRoot.LeftChild!));
            nodes.Add(treeRoot);
            nodes.AddRange(GetNodesInorder(treeRoot.RightChild!));
            return nodes;
        }

        private static bool IsLessThan(TKeyValue firstOperand, TKeyValue secondOperand)
        {
            _ = firstOperand ?? throw new ArgumentNullException(paramName: nameof(firstOperand));
            _ = secondOperand ?? throw new ArgumentNullException(paramName: nameof(secondOperand));
            return firstOperand?.CompareTo(secondOperand) < 0;
        }

        private static bool AreEqual(TKeyValue firstOperand, TKeyValue secondOperand)
        {
            _ = firstOperand ?? throw new ArgumentNullException(nameof(firstOperand));
            _ = secondOperand ?? throw new ArgumentNullException(nameof(secondOperand));
            return firstOperand?.CompareTo(secondOperand) == 0;
        }

        public override string ToString()
        {
            List<Node> nodes = GetNodesInorder(root);
            if (nodes.Count == 0)
                return "The tree is empty.";
            return nodes.Select(node => node.ToString() + (nodes.IndexOf(node) == nodes.Count - 1 ? "" : "\n"))
                        .Aggregate((result, value) => result + value);
        }

        private Node FindMinimum(Node treeRoot)
        {
            if (treeRoot is null || treeRoot == _sentinel)
                throw new ArgumentException("Tree root can not be null or sentinel.");
            while (treeRoot!.LeftChild != _sentinel)
                treeRoot = treeRoot.LeftChild!;
            return treeRoot;
        }

        private Node FindByValue(TKeyValue value)
        {
            Node result = root;
            while (result != _sentinel)
                if (AreEqual(result.KeyValue!, value))
                    break;
                else if (IsLessThan(result.KeyValue!, value))
                    result = result.RightChild!;
                else
                    result = result.RightChild!;
            if (result == _sentinel) throw new NotFoundException("Node with provided value does not exist.");
            return result;
        }

        #region Deletion
        private void Transplant(Node destination, Node source)
        {
            if (destination.Parent == _sentinel)
                root = source;
            else if (destination == destination.Parent!.LeftChild)
                destination.Parent.LeftChild = source;
            else destination.Parent.RightChild = source;
            source.Parent = destination.Parent;
        }

        // criticalNode - node thas is being deleted
        // replacementNode - node that the criticalNode is being replaced with
        // problematicNode - node that replaces replacementNode
        public void Delete(TKeyValue value)
        {
            Node criticalNode = FindByValue(value);
            Node replacement = criticalNode;
            NodeColor replacementOriginalColor = replacement.Color;
            Node problematicNode;
            if (criticalNode.LeftChild == _sentinel)
            {
                problematicNode = criticalNode.RightChild!;
                Transplant(criticalNode, criticalNode.RightChild!);
            }
            else if (criticalNode.RightChild == _sentinel)
            {
                problematicNode = criticalNode.LeftChild!;
                Transplant(criticalNode, criticalNode.LeftChild!);
            }
            else
            {
                replacement = FindMinimum(replacement.RightChild!);
                replacementOriginalColor = replacement.Color;
                problematicNode = replacement.RightChild!;
                if (replacement != criticalNode.RightChild)
                {
                    Transplant(replacement, replacement.RightChild!);
                    replacement.RightChild = criticalNode.RightChild;
                    replacement.RightChild!.Parent = replacement;
                }
                else problematicNode.Parent = replacement;
                Transplant(criticalNode, replacement);
                replacement.LeftChild = criticalNode.LeftChild;
                replacement.LeftChild!.Parent = replacement;
                replacement.Color = criticalNode.Color;
            }
            if (replacementOriginalColor == NodeColor.BLACK)
                FixDeletion(problematicNode);
        }

        private void FixDeletion(Node problematicNode)
        {
            while (problematicNode != root && problematicNode.Color == NodeColor.BLACK)
            {
                Node problematicNodeSibling;
                if (problematicNode == problematicNode.Parent!.LeftChild)
                {
                    problematicNodeSibling = problematicNode.Parent.RightChild!;
                    if (problematicNodeSibling.Color == NodeColor.RED) { }
                    if (problematicNodeSibling.LeftChild!.Color == NodeColor.BLACK && problematicNodeSibling.RightChild!.Color == NodeColor.BLACK) { }
                    else
                    {
                        if (problematicNodeSibling.RightChild!.Color == NodeColor.BLACK)
                        {
                            problematicNodeSibling.LeftChild.Color = NodeColor.BLACK;
                            problematicNodeSibling.Color = NodeColor.BLACK;
                            RotateRight(problematicNodeSibling);
                            problematicNodeSibling = problematicNode.Parent.RightChild!;
                        }
                        problematicNodeSibling.Color = problematicNode.Parent.Color;
                        problematicNode.Parent.Color = NodeColor.BLACK;
                        problematicNodeSibling.RightChild!.Color = NodeColor.BLACK;
                        RotateLeft(problematicNode.Parent);
                        problematicNode = root;
                    }
                }
            }
            problematicNode.Color = NodeColor.BLACK;
            _sentinel.Parent = null;
        }
        #endregion

        #region Insertion
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
            uniqueIdentifierGenerator++;
            newNode.Index = uniqueIdentifierGenerator;
            FixInsertion(newNode);
        }

        private void FixInsertion(Node criticalNode)
        {
            if (criticalNode == _sentinel) return;
            while (criticalNode.Parent!.Color == NodeColor.RED)
            {
                Node criticalNodeUncle;
                if (criticalNode.Parent == criticalNode.Parent.Parent!.LeftChild)
                {
                    criticalNodeUncle = criticalNode.Parent.Parent.RightChild!;
                    if (criticalNodeUncle.Color == NodeColor.RED)
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
                        criticalNode.Parent!.Color = NodeColor.BLACK;
                        criticalNode.Parent.Parent!.Color = NodeColor.RED;
                        RotateRight(criticalNode.Parent.Parent);
                    }
                }
                else
                {
                    criticalNodeUncle = criticalNode.Parent.Parent.LeftChild!;
                    if (criticalNodeUncle.Color == NodeColor.RED)
                    {
                        criticalNode.Parent.Color = NodeColor.BLACK;
                        criticalNodeUncle.Color = NodeColor.BLACK;
                        criticalNode.Parent.Parent.Color = NodeColor.RED;
                        criticalNode = criticalNode.Parent.Parent;
                    }
                    else
                    {
                        if (criticalNode == criticalNode.Parent.LeftChild)
                        {
                            criticalNode = criticalNode.Parent;
                            RotateRight(criticalNode);
                        }
                        criticalNode.Parent!.Color = NodeColor.BLACK;
                        criticalNode.Parent.Parent!.Color = NodeColor.RED;
                        RotateLeft(criticalNode.Parent.Parent);
                    }
                }
            }
            root.Color = NodeColor.BLACK;
        }
        #endregion

        #region Rotations
        private void RotateLeft(Node criticalNode)
        {
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

        private void RotateRight(Node criticalNode)
        {
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
        #endregion
    }
}