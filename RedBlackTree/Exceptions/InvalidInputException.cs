namespace RedBlackTree.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message = "Invalid input.") : base(message) { }
    }
}