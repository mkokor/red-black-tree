using RedBlackTree;
using RedBlackTree.Exceptions;

internal class Program
{
  private static readonly List<int> _availableOptions = new() { 1, 2, 3 };
  private static readonly RedBlackTree<int> _redBlackTree = new();

  private delegate void InputValidation(string? inputValue);

  private static string GetUserInput(string inputRequestMessage, InputValidation validateInput)
  {
    while (true)
    {
      Console.Write(inputRequestMessage);
      try
      {
        string input = Console.ReadLine() ?? throw new InvalidInputException();
        validateInput(input);
        return input;
      }
      catch (Exception)
      {
        Console.WriteLine("Invalid input! Try again.");
      }
    }
  }

  #region InsertionValueInput
  private static void ValidateInsertionValueInput(string? inputValue)
  {
    try
    {
      string choice = inputValue ?? throw new InvalidInputException();
      int.Parse(choice.Trim());
    }
    catch (Exception)
    {
      throw new InvalidInputException();
    }
  }

  // Program will work only with integer values.
  private static int GetInsertionValue()
  {
    string validInput = GetUserInput("\n\nEnter value: ", ValidateInsertionValueInput);
    return int.Parse(validInput);
  }
  #endregion

  #region OptionInput
  private static void ValidateOptionInput(string? inputValue)
  {
    try
    {
      string choice = inputValue ?? throw new InvalidInputException();
      int selectedOption = int.Parse(choice.Trim());
      if (!_availableOptions.Contains(selectedOption))
        throw new InvalidInputException();
    }
    catch (Exception)
    {
      throw new InvalidInputException();
    }
  }

  private static int GetSelectedOption()
  {
    string validInput = GetUserInput("Enter your choice: ", ValidateOptionInput);
    return int.Parse(validInput);
  }
  #endregion

  private static void Main(string[] args)
  {
    Console.WriteLine("\nWelcome!");
    while (true)
    {
      Console.WriteLine("\n\nAvailable options: \n    1 - Insert an element\n    2 - Display nodes inorder\n    3 - Exit");
      int userChoice = GetSelectedOption();
      if (userChoice == 1)
      {
        _redBlackTree.Insert(GetInsertionValue());
        Console.WriteLine("\n\nValue successfully inserted!");
      }
      else if (userChoice == 2)
      {
        Console.WriteLine($"\n\n{_redBlackTree}");
      }
      if (userChoice == 3)
      {
        Console.WriteLine("\n\nThank you for your time! Goodbye.\n");
        break;
      }
    }
  }
}