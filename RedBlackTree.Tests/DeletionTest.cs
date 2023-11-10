namespace RedBlackTree.Tests;

public class DeletionTest
{
    [Fact]
    public void Deletion_MultipleNumbers_CorrectInorderNodesResult()
    {
        RedBlackTree<int> redBlackTree = new();
        List<int> numbers = new() { 6, 11, 10, 2, 9, 7, 5, 13, 22, 27, 36, 12, 31 };
        List<int> numersToDelete = new() { 5, 27, 36, 12, 11 };

        numbers.ForEach(redBlackTree.Insert);
        numersToDelete.ForEach(redBlackTree.Delete);

        string actualResult = redBlackTree.ToString();
        List<string> nodes = new()
        {
            "Index: 4; Value: 2; Color: black; LeftChild: sentinel; RightChild: sentinel; Parent: 1\n",
            "Index: 1; Value: 6; Color: black; LeftChild: 4; RightChild: 5; Parent: 3\n",
            "Index: 6; Value: 7; Color: red; LeftChild: sentinel; RightChild: sentinel; Parent: 5\n",
            "Index: 5; Value: 9; Color: black; LeftChild: 6; RightChild: sentinel; Parent: 1\n",
            "Index: 3; Value: 10; Color: black; LeftChild: 1; RightChild: 9; Parent: sentinel\n",
            "Index: 8; Value: 13; Color: black; LeftChild: sentinel; RightChild: sentinel; Parent: 9\n",
            "Index: 9; Value: 22; Color: black; LeftChild: 8; RightChild: 13; Parent: 3\n",
            "Index: 13; Value: 31; Color: black; LeftChild: sentinel; RightChild: sentinel; Parent: 9"
        };
        string expectedResult = nodes.Aggregate((result, element) => result + element);

        Assert.Equal(expectedResult, actualResult);
    }
}