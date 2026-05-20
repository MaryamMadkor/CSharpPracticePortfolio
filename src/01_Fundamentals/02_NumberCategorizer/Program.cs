/*
 * 2.Number Categorizer(Beginner)

    Classify numbers as positive/negative and even/odd.

    Concepts: Conditional statements, modulo operator (%).

    Task: Read a number.Print whether it’s positive/negative and even/odd.
*/
using NumberCategorization.Contracts.Models;

namespace NumberCategorization.Contracts
{

}
namespace NumberCategorization.Contracts.Models
{
    public class Input
    {
        public char input {  get; set; }
        public CharCategory type {  get; set; }
        public NumberCategory? numbertype {  get; set; }
        public Input(char input, CharCategory type, NumberCategory? numbertype = null)
        {
            this.input = input;
            this.type = type;
            this.numbertype = numbertype;
        }
    }
    public enum CharCategory
    {
        none = 0,
        Char = 1,
        integer = 2,
        floating = 3,
        String = 4,
        whiteSpace = 5
    }
    public enum NumberCategory
    {
        PositiveEven = 2,
        PositiveOdd = 1,
        Zero = 0,
        NegativeOdd = -1,
        NegativeEven = -2
    }
}
namespace NumberCategorization.Logic
{
    public class NumberCateorizerLogic
    {
        public CharCategory IdentifyChar(string input)
        {
            if(int.TryParse(input, out int inputNum))
            {
                return CharCategory.integer;
            }
            else if(float.TryParse(input, out float inputFloat) || double.TryParse(input,out double inputDouble))
            {
                return CharCategory.floating;
            }
            else
            {
                if(input.Length == 1 && input != " ") return CharCategory.Char;
                else if(string.IsNullOrWhiteSpace(input)) return CharCategory.whiteSpace;
                else return CharCategory.String;
            }
        }
        public NumberCategory IdentifyNumProperties(int input)
        {
            bool even = false; bool positive = false; bool zero = false;
            if (input == 0) return NumberCategory.Zero;
            if (input > 0) positive = true;
            if (input % 2 == 0) even = true;
            if (even && positive) return NumberCategory.PositiveEven;
            else if (even && !positive) return NumberCategory.NegativeEven;
            else if (!even && positive) return NumberCategory.PositiveOdd;
            else return NumberCategory.NegativeOdd;
        }
        public NumberCategory IdentifyNumProperties(float input)
        {
            bool even = false; bool positive = false;
            if (input == 0) return NumberCategory.Zero;
            if (input > 0) positive = true;
            if (input % 2 == 0) even = true;
            if (even && positive) return NumberCategory.PositiveEven;
            else if (even && !positive) return NumberCategory.NegativeEven;
            else if (!even && positive) return NumberCategory.PositiveOdd;
            else return NumberCategory.NegativeOdd;
        }
    }
}
namespace NumberCategorization.UserInterface
{

}
namespace NumberCategorization.Application
{

}
namespace NumberCategorization.Tests
{

}
namespace NumberCategorization
{
    /// <summary>
    /// Represents the category of a number
    /// </summary>

    /// <summary>
    /// Provides methods for categorizing numbers
    /// </summary>
    public static class NumberCategorizer
    {
        /// <summary>
        /// Categorizes a whole number into its sign and parity
        /// </summary>
        /// <param name="num">The number to categorize</param>
        /// <returns>The category of the number</returns>
        public static NumberCategory Categorize(int num)
        {
            if (num == 0) { return NumberCategory.Zero; }
            bool isEven = num % 2 == 0;
            bool isPositive = num > 0;
            if (isEven)
            {
                return isPositive ? NumberCategory.PositiveEven : NumberCategory.NegativeEven;
            }
            else
            {
                return isPositive ? NumberCategory.PositiveOdd : NumberCategory.NegativeOdd;
            }
        }
        /// <summary>
        /// Gets a user-friendly description of a number category
        /// </summary>
        public static string GetDescribtion(this NumberCategory category)
        {
            return category switch
            {
                NumberCategory.NegativeEven => "Negative and Even.",
                NumberCategory.PositiveEven => "Positive and Even.",
                NumberCategory.PositiveOdd => "Positive and Odd.",
                NumberCategory.NegativeOdd => "Negative and Odd.",
                NumberCategory.Zero => "Zero is not ever, odd, positive or negative.",
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };
        }
        /// <summary>
        /// Validates that a number is a whole number (not floating point)
        /// </summary>
        public static bool isWholeNumber(double number, out int wholeNumber)
        {
            wholeNumber = (int)number;
            return Math.Abs(number - wholeNumber) < double.Epsilon;
        }
    }

    public class Program
    {
        /// <summary>
        /// This function is the main function (starting point of the program)
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("--------------- Program Categorizer App ----------------");
            Console.WriteLine("--------------------------------------------------------");
            while (true)
            {
                try
                {
                    int num = takeValueFromUser();
                    var category = NumberCategorizer.Categorize(num);
                    Console.WriteLine($"The number {num} is: {category.GetDescribtion()}.");
                    if (!shouldContinue()) { break; }
                }
                catch (Exception ex) when (ex is ArgumentException or ArgumentOutOfRangeException)
                {
                    Console.WriteLine($"\nError: {ex.Message}\n");
                }
            }
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("--------------- Thanks for using our app ---------------");
            Console.WriteLine("--------------------------------------------------------");
        }
        /// <summary>
        /// this function takes the value from user and validates it before sending it to main
        /// </summary>
        /// <returns>returns the validated value of the number to be categorized </returns>
        public static int takeValueFromUser()
        {
            Console.Write("Enter the number you need categorized: ");
            if (!double.TryParse(Console.ReadLine(), out double temp))
            {
                Console.WriteLine("Not A valid number.");
            }
            if (!NumberCategorizer.isWholeNumber(temp, out int wholeNumber))
                throw new ArgumentException("Please enter a whole number (no decimals).");
            if (wholeNumber < int.MinValue || wholeNumber > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(wholeNumber), "Number is out of range.");
            return wholeNumber;
        }
        /// <summary>
        /// determines if the program should continue with the while loop (if the user wants to categorize more numbers)
        /// </summary>
        public static bool shouldContinue()
        {
            Console.Write("Would you to Catigorize more number? (y/n):");
            string decision = Console.ReadLine().Trim().ToLower();
            return decision == "y" || decision == "yes";
        }
    }
}
