namespace RedBlackTree.Tests;

public class InsertionTest
{
    [Fact]
    public void Insert_MultipleNumbers_CorrectInorderNodesResult()
    {
        RedBlackTree<int> redBlackTree = new();
        List<int> numbers = new() { 6, 11, 10, 2, 9, 7, 5, 13, 22, 27, 36, 12, 31 };

        numbers.ForEach(redBlackTree.Insert);

        string actualResult = redBlackTree.ToString();
        List<string> nodes = new()
        {
            "Index: 4; Value: 2; Color: black; LeftChild: sentinel; RightChild: 7; Parent: 1\n",
            "Index: 7; Value: 5; Color: red; LeftChild: sentinel; RightChild: sentinel; Parent: 4\n",
            "Index: 1; Value: 6; Color: black; LeftChild: 4; RightChild: 5; Parent: 3\n",
            "Index: 6; Value: 7; Color: red; LeftChild: sentinel; RightChild: sentinel; Parent: 5\n",
            "Index: 5; Value: 9; Color: black; LeftChild: 6; RightChild: sentinel; Parent: 1\n",
            "Index: 3; Value: 10; Color: black; LeftChild: 1; RightChild: 8; Parent: sentinel\n",
            "Index: 2; Value: 11; Color: black; LeftChild: sentinel; RightChild: 12; Parent: 8\n",
            "Index: 12; Value: 12; Color: red; LeftChild: sentinel; RightChild: sentinel; Parent: 2\n",
            "Index: 8; Value: 13; Color: black; LeftChild: 2; RightChild: 10; Parent: 3\n",
            "Index: 9; Value: 22; Color: black; LeftChild: sentinel; RightChild: sentinel; Parent: 10\n",
            "Index: 10; Value: 27; Color: red; LeftChild: 9; RightChild: 11; Parent: 8\n",
            "Index: 13; Value: 31; Color: red; LeftChild: sentinel; RightChild: sentinel; Parent: 11\n",
            "Index: 11; Value: 36; Color: black; LeftChild: 13; RightChild: sentinel; Parent: 10",
        };
        string expectedResult = nodes.Aggregate((result, element) => result + element);

        Assert.Equal(expectedResult, actualResult);
    }
}