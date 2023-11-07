namespace RedBlackTree.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message = "Input value is not valid.") : base(message) { }
    }
}