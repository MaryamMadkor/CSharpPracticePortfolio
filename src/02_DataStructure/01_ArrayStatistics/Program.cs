/*
 * 4.Array Statistics Calculator(Intermediate)

        Compute statistics for a 1D array or a 2D array.

        Concepts: 1D arrays, loops, arithmetic.

        Task: Find min, max, sum, and average of an array of numbers.
*/
namespace ArrayStatistics
{
    /// <summary>
    /// Provides statistical calculations for various array types
    /// </summary>
    public static class StatisticsCalculator
    {
        //----------------------------------- 1D arrays
        public static int FindMaxIndex(double[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array cannot be null or empty", nameof(array));

            int maxIndex = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > array[maxIndex])
                    maxIndex = i;
            }
            return maxIndex;
        }

        public static int FindMinIndex(double[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array cannot be null or empty", nameof(array));

            int minIndex = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[minIndex])
                    minIndex = i;
            }
            return minIndex;
        }

        public static double CalculateSum(double[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            double sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum;
        }

        public static double CalculateAverage(double[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array cannot be null or empty", nameof(array));

            return CalculateSum(array) / array.Length;
        }

        //-------------------------- 2D arrays
        public static (int Row, int Column) FindMaxIndex(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            int maxRow = 0, maxCol = 0;
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (array[i, j] > array[maxRow, maxCol])
                    {
                        maxRow = i;
                        maxCol = j;
                    }
                }
            }
            return (maxRow, maxCol);
        }

        public static (int Row, int Column) FindMinIndex(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            int minRow = 0, minCol = 0;
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (array[i, j] < array[minRow, minCol])
                    {
                        minRow = i;
                        minCol = j;
                    }
                }
            }
            return (minRow, minCol);
        }

        public static double CalculateSum(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            double sum = 0;
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sum += array[i, j];
                }
            }
            return sum;
        }

        public static double CalculateAverage(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            return CalculateSum(array) / (array.GetLength(0) * array.GetLength(1));
        }

        //----------------------------------------------- Jagged array
        public static (int Row, int Column) FindMaxIndex(double[][] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array cannot be null or empty", nameof(array));

            int maxRow = 0, maxCol = 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null || array[i].Length == 0)
                    continue;

                for (int j = 0; j < array[i].Length; j++)
                {
                    if (array[i][j] > array[maxRow][maxCol])
                    {
                        maxRow = i;
                        maxCol = j;
                    }
                }
            }
            return (maxRow, maxCol);
        }

        public static (int Row, int Column) FindMinIndex(double[][] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array cannot be null or empty", nameof(array));

            int minRow = 0, minCol = 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null || array[i].Length == 0)
                    continue;

                for (int j = 0; j < array[i].Length; j++)
                {
                    if (array[i][j] < array[minRow][minCol])
                    {
                        minRow = i;
                        minCol = j;
                    }
                }
            }
            return (minRow, minCol);
        }

        public static double CalculateSum(double[][] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            double sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                    continue;

                for (int j = 0; j < array[i].Length; j++)
                {
                    sum += array[i][j];
                }
            }
            return sum;
        }

        public static double CalculateAverage(double[][] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            int totalElements = array.Sum(row => row?.Length ?? 0);
            if (totalElements == 0)
                throw new InvalidOperationException("Array contains no elements");

            return CalculateSum(array) / totalElements;
        }
    }

    /// <summary>
    /// Handles user input and output operations
    /// </summary>
    public static class ArrayInputHandler
    {

        public static double[] Get1DArray()
        {
            while (true)
            {
                Console.Write("Enter the array elements separated by spaces: ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                    continue;
                }

                string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                double[] result = new double[parts.Length];
                bool success = true;

                for (int i = 0; i < parts.Length; i++)
                {
                    if (!double.TryParse(parts[i], out result[i]))
                    {
                        Console.WriteLine($"Invalid number: '{parts[i]}'. Please try again.");
                        success = false;
                        break;
                    }
                }

                if (success)
                    return result;
            }
        }

        public static double[,] Get2DArray()
        {
            int rows = GetPositiveInteger("Enter the number of rows: ");
            int columns = GetPositiveInteger("Enter the number of columns: ");

            var array = new double[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                while (true)
                {
                    Console.Write($"Enter row {row + 1} elements (exactly {columns} numbers separated by spaces): ");
                    string input = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Input cannot be empty.");
                        continue;
                    }

                    string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != columns)
                    {
                        Console.WriteLine($"Expected {columns} numbers, but got {parts.Length}. Please try again.");
                        continue;
                    }

                    bool rowSuccess = true;
                    for (int col = 0; col < columns; col++)
                    {
                        if (!double.TryParse(parts[col], out array[row, col]))
                        {
                            Console.WriteLine($"Invalid number: '{parts[col]}'. Please try again.");
                            rowSuccess = false;
                            break;
                        }
                    }

                    if (rowSuccess)
                        break;
                }
            }

            return array;
        }

        public static double[][] GetJaggedArray()
        {
            int rows = GetPositiveInteger("Enter the number of rows: ");
            var array = new double[rows][];

            for (int row = 0; row < rows; row++)
            {
                while (true)
                {
                    Console.Write($"Enter row {row + 1} elements (any count, separated by spaces): ");
                    string input = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Input cannot be empty.");
                        continue;
                    }

                    string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    array[row] = new double[parts.Length];

                    bool rowSuccess = true;
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (!double.TryParse(parts[i], out array[row][i]))
                        {
                            Console.WriteLine($"Invalid number: '{parts[i]}'. Please try again.");
                            rowSuccess = false;
                            break;
                        }
                    }

                    if (rowSuccess)
                        break;
                }
            }

            return array;
        }

        private static int GetPositiveInteger(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                    return value;

                Console.WriteLine("Please enter a positive integer.");
            }
        }

        public static bool ShouldContinue()
        {
            while (true)
            {
                Console.Write("\nWould you like to analyze more arrays? (y/n): ");
                string decision = Console.ReadLine()?.Trim().ToLowerInvariant();

                if (decision == "y" || decision == "yes")
                    return true;
                if (decision == "n" || decision == "no")
                    return false;

                Console.WriteLine("Please enter 'y' or 'n'.");
            }
        }
    }

    /// <summary>
    /// Main program entry point
    /// </summary>
    public static class Program2
    {
        public static void Main()
        {
            DisplayHeader();

            while (true)
            {
                int choice = GetArrayTypeChoice();

                try
                {
                    switch (choice)
                    {
                        case 1:
                            Process1DArray();
                            break;
                        case 2:
                            Process2DArray();
                            break;
                        case 3:
                            ProcessJaggedArray();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                if (!ArrayInputHandler.ShouldContinue())
                {
                    DisplayFooter();
                    break;
                }
            }
        }

        private static void DisplayHeader()
        {
            Console.WriteLine(new string('=', 69));
            Console.WriteLine("==================== Array Statistics Calculator ====================");
            Console.WriteLine(new string('=', 69));
        }

        private static void DisplayFooter()
        {
            Console.WriteLine(new string('=', 69));
            Console.WriteLine("======================= Thanks for using our app ====================");
            Console.WriteLine(new string('=', 69));
        }

        private static int GetArrayTypeChoice()
        {
            Console.WriteLine("\nSelect array type:");
            Console.WriteLine("1. One-dimensional array");
            Console.WriteLine("2. Two-dimensional array");
            Console.WriteLine("3. Jagged array (rows with different lengths)");
            Console.Write("Enter your choice (1-3): ");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
            {
                Console.Write("Invalid input. Please enter 1, 2, or 3: ");
            }

            return choice;
        }

        private static void Process1DArray()
        {
            double[] array = ArrayInputHandler.Get1DArray();

            int minIndex = StatisticsCalculator.FindMinIndex(array);
            int maxIndex = StatisticsCalculator.FindMaxIndex(array);
            double sum = StatisticsCalculator.CalculateSum(array);
            double average = StatisticsCalculator.CalculateAverage(array);

            DisplayResults(
                minValue: array[minIndex],
                maxValue: array[maxIndex],
                sum: sum,
                average: average
            );
        }

        private static void Process2DArray()
        {
            double[,] array = ArrayInputHandler.Get2DArray();

            var (minRow, minCol) = StatisticsCalculator.FindMinIndex(array);
            var (maxRow, maxCol) = StatisticsCalculator.FindMaxIndex(array);
            double sum = StatisticsCalculator.CalculateSum(array);
            double average = StatisticsCalculator.CalculateAverage(array);

            DisplayResults(
                minValue: array[minRow, minCol],
                maxValue: array[maxRow, maxCol],
                sum: sum,
                average: average
            );
        }

        private static void ProcessJaggedArray()
        {
            double[][] array = ArrayInputHandler.GetJaggedArray();

            var (minRow, minCol) = StatisticsCalculator.FindMinIndex(array);
            var (maxRow, maxCol) = StatisticsCalculator.FindMaxIndex(array);
            double sum = StatisticsCalculator.CalculateSum(array);
            double average = StatisticsCalculator.CalculateAverage(array);

            DisplayResults(
                minValue: array[minRow][minCol],
                maxValue: array[maxRow][maxCol],
                sum: sum,
                average: average
            );
        }

        private static void DisplayResults(double minValue, double maxValue, double sum, double average)
        {
            Console.WriteLine("\nArray Statistics:");
            Console.WriteLine($"Minimum element: {minValue}");
            Console.WriteLine($"Maximum element: {maxValue}");
            Console.WriteLine($"Sum: {sum}");
            Console.WriteLine($"Average: {average:F2}");
        }
    }
}