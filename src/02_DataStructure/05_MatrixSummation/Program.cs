
/*
         * 5.Matrix Summation(Intermediate)

                Add matrices.

                Concepts: 2D arrays, nested loops.

                Task: Add two, three or four fixed-size matrices and print the result.
*/
using System.Data;

namespace MatrixSum
{
    /// <summary>
    /// Sums Matrices and Validates them.
    /// </summary>
    public static class SumMatrices
    {
        /// <summary>
        /// Sums multiple matrices of equal dimentions.
        /// </summary>
        /// <param name = "matrices" > Matrices to be summed up</param>
        /// <returns>Result matrix</returns>
        public static double[,] Sum(params double[][,] matrices)
        {
            ValidateMatrices(matrices);
            int rows = matrices[0].GetLength(0);
            int columns = matrices[0].GetLength(1);
            int matrixCount = matrices.Length;
            double[,] result = new double[rows, columns];

            foreach (var matrix in matrices)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        result[i, j] += matrix[i, j];
                    }
                }
            }

            return result;
        }
        private static void ValidateMatrices(double[][,] matrices)
        {
            if (matrices == null)
                throw new ArgumentNullException(nameof(matrices));

            if (matrices.Length < 2)
                throw new ArgumentException("At least two matrices are required for summation", nameof(matrices));

            if (matrices.Any(m => m == null))
                throw new ArgumentException("Null matrices are not allowed", nameof(matrices));

            int rows = matrices[0].GetLength(0);
            int columns = matrices[0].GetLength(1);

            for (int i = 1; i < matrices.Length; i++)
            {
                if (matrices[i].GetLength(0) != rows || matrices[i].GetLength(1) != columns)
                {
                    throw new ArgumentException(
                        $"Matrix {i} has dimensions [{matrices[i].GetLength(0)}x{matrices[i].GetLength(1)}] " +
                        $"but expected [{rows}x{columns}]");
                }
            }
        }
    }
    /// <summary>
    /// User Interface
    /// </summary>
    public static class InputOutputHandler
    {
        private const int maxMatrixCount = 10;
        private const int minMatrixCount = 2;
        private const int maxDimentionCount = 100;
        private const int minDimentionCount = 1;
        /// <summary>
        /// Displayes header of program
        /// </summary>
        public static void DisplayOpening()
        {
            Console.WriteLine("====================================================================");
            Console.WriteLine("========================= Matrices Sum App =========================");
            Console.WriteLine("====================================================================");
        }
        / <summary>
        / Displays footer of program
        / </summary>
        public static void DisplayEnding()
        {
            Console.WriteLine("====================================================================");
            Console.WriteLine("===================== Thanks for using our app =====================");
            Console.WriteLine("====================================================================");
        }
        /// <summary>
        /// Gets dimentions of matrices 
        /// </summary>
        /// <returns>Dimentions of matrices</returns>
        public static (int rows, int columns) GetSize()
        {
            int rows = GetPositiveInteger("Enter number of rows of matrices (1-100): ", minDimentionCount, maxDimentionCount)
              , columns = GetPositiveInteger("Enter number of columns of matrices (1-100): ", minDimentionCount, maxDimentionCount);
            return new(rows, columns);
        }
        /// <summary>
        /// Validates entered numbers of indecies
        /// </summary>
        /// <param name = "prompt" > String showing user the needed parameter name</param>
        /// <returns>Valid positive integer</returns>
        private static int GetPositiveInteger(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                    return value;

                Console.WriteLine($"Please enter a positive whole number between {min} and {max}.");
            }
        }
        /// <summary>
        /// Accepts and validates the number of 
        /// </summary>
        /// <returns>Valid number of matrices to be summed up</returns>
        public static int GetMatrixCount()
        {
            int count = GetPositiveInteger("\nHow many matrices to sum? (1-10)", minMatrixCount, maxMatrixCount);
            return count;
        }
        /// <summary>
        /// Reads matrix and validates dimentions according to GetSize()
        /// </summary>
        /// <param name = "rows" > Length of matrix</param>
        /// <param name = "columns" > Width of matrix</param>
        /// <param name = "matrixName" > Name of this needed matrix to be identified by user</param>
        /// <returns>Matrix to be summed up with others</returns>

        public static double[,] GetMatrix(int rows, int columns, string matrixName)
        {
            Console.WriteLine($"Enter Matrix {matrixName} ({rows} x {columns}):");
            double[,] matrix = new double[rows, columns];
            for (int j = 0; j < rows; j++)
            {
                while (true)
                {
                    Console.Write($"Enter row {j + 1} elements separated by a space each: ");
                    var strArr = Console.ReadLine().Split(' ');
                    if (strArr.Length == columns)
                    {
                        bool valid = true;
                        for (int i = 0; i < strArr.Length; i++)
                        {
                            if (!double.TryParse(strArr[i], out double item))
                            {
                                Console.WriteLine($"Invalid number at position {i + 1}. Try again.");
                                valid = false;
                                j--;
                                break;
                            }
                            matrix[j, i] = item;
                        }
                        if (valid) break;
                    }
                    else
                    {
                        Console.WriteLine($"Expected {columns} numbers, got {strArr.Length}. Try again.");
                        continue;
                    }
                }

            }
            return matrix;
        }
        /// <summary>
        /// Displays Result matrix of summation
        /// </summary>
        /// <param name = "result" > Result sum matrix</param>
        public static void DisplayResult(double[,] result)
        {
            Console.WriteLine("Result matrix is: ");
            int rows = result.GetLength(0), columns = result.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(j < (columns - 1) ? $"{result[i, j]} " : result[i, j]);
                }
                Console.Write(" |\n");
            }
            Console.WriteLine("====================================================================");
        }
        /// <summary>
        /// Prompts and reads user's will to continue
        /// </summary>
        /// <returns>Boolean of yes or no</returns>
        public static bool ShouldContinue()
        {
            Console.Write("Would you to sum up more matrices? (y/n):");
            string decision = Console.ReadLine().Trim().ToLower();
            return decision == "y" || decision == "yes";
        }
    }
    /// <summary>
    /// Main program
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            InputOutputHandler.DisplayOpening();
            while (true)
            {
                int matricesCount = InputOutputHandler.GetMatrixCount();
                (int rows, int columns) = InputOutputHandler.GetSize();
                var matrices = new double[matricesCount][,];
                for (int i = 0; i < matricesCount; i++)
                {
                    matrices[i] = InputOutputHandler.GetMatrix(rows, columns, $"{(char)(65 + i)}");
                }
                try
                {
                    double[,] result = SumMatrices.Sum(matrices);
                    InputOutputHandler.DisplayResult(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }
                if (!InputOutputHandler.ShouldContinue())
                {
                    break;
                }
            }
            InputOutputHandler.DisplayEnding();
        }
    }
}