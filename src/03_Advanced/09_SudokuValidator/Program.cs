/*
10. Sudoku Row Validator (Advanced)

Check if a Sudoku row (or column) has all digits 1-9.

Concepts: 1D/2D arrays, conditionals, loops.

Task: Verify no duplicates in a 9-element array (ignore zeros for incomplete rows).
*/
//Contract Namespace
using System.Globalization;
using SudokuValidator.Application;
using SudokuValidator.Contracts;
using SudokuValidator.Contracts.Models;
using SudokuValidator.Logic;
using SudokuValidator.UserInterface;
using SudokuValidator.Tests;
namespace SudokuValidator.Contracts
{
    public interface IInputHandler
    {
        public bool ShouldContinue();
        public int[,] ReadBoard();
    }
    public interface IOutputHandle
    {
        public void DisplayHeader();
        public void DisplayFooter();
        public void DisplayError(string message);
        public void DisplayResult(string message);
        public void DisplayMistakes(List<string> mistakes);
    }
    public interface IGameDataHandler
    {
        public static abstract string GetState(States state);
        public static abstract States GetState(bool valid);
        public static abstract string DocumentError((int position, int num) rowNum, int RowColumnBoxCase);

    }
    public interface IValidateGame
    {
        public bool GetState(Game game);
    }
}
namespace SudokuValidator.Contracts.Models
{
    public class Game
    {
        public int[,]? gameBoard;
        public States state;
        public List<string>? Mistakes;
    }
    public enum States
    {
        none = 0,
        Valid = 1,
        NotValid = 2
    }
    public class Constants
    {
        public const int boxSize = 3;
        public const int BoardSize = 9;
        public const int upperBound = 9;
        public const int lowerBound = 1;
    }
}
namespace SudokuValidator.Logic
{
    public class SudokuValdate : IValidateGame
    {
        /// <summary>
        /// Game Validation and Error Documentation Orchistrator 
        /// </summary>
        /// <returns> Is game valid or not</returns>
        public bool GetState(Game game)
        {
            ValidateBoard(game.gameBoard);
            (bool valid, List<(int row, int num)> mistakes) rows = ValidateRows(game.gameBoard);
            int rowMistakeLen = rows.mistakes.Count;
            if (!rows.valid && rows.mistakes != null)
            {
                for (int i = 0; i < rowMistakeLen; i++)
                {
                    if (game.Mistakes == null) game.Mistakes = new List<string>();
                    game.Mistakes.Add(GameDataHandler.DocumentError(rows.mistakes[i], 1));
                }
            }
            (bool valid, List<(int, int)> mistakes) columns = ValidateColumns(game.gameBoard);
            if (!columns.valid && columns.mistakes != null)
            {
                int columnMistakeLen = columns.mistakes.Count;
                for (int i = 0; i < columnMistakeLen; i++)
                {
                    if (game.Mistakes == null) game.Mistakes = new List<string>();
                    game.Mistakes.Add(GameDataHandler.DocumentError(columns.mistakes[i], 2));
                }
            }
            bool boxesValid = true;
            for (int i = 0; i < Constants.BoardSize; i++)
            {
                (bool validBox, List<int> redundantNums) = ValidateBox(game.gameBoard, i);
                if (!validBox)
                {
                    if (game.Mistakes == null) game.Mistakes = new List<string>();
                    for (int j = 0; j < redundantNums.Count; j++)
                        game.Mistakes.Add(GameDataHandler.DocumentError((i, redundantNums[j]), 3));
                    boxesValid = false;
                }
            }
            return (rows.valid && columns.valid && boxesValid);
        }
        /// <summary>
        /// validate rows of game
        /// </summary>
        /// <returns>Tuble of state of all rows and a list of the mistakes if any exist; another tuple of the row index and the redundant number</returns>
        private (bool, List<(int, int)>) ValidateRows(int[,] gameBoard)
        {
            int size = gameBoard.GetLength(0);
            bool isValid = true;
            ushort[] freq = new ushort[size + 1];
            List<(int, int)> mistakeRowNum = new List<(int, int)>();
            for (int i = 0; i < size; i++)
            {
                Array.Clear(freq, 0, freq.Length);
                for (int j = 0; j < size; j++)
                {
                    int value = gameBoard[i, j];
                    if (value != 0)
                    {
                        if (value < 1 || value > 9)
                        {
                            throw new ArgumentException("Not a valid number.");
                        }
                        freq[value]++;
                    }
                }
                for (int j = 1; j <= size; ++j)
                {
                    if (freq[j] > 1)
                    {
                        isValid = false;
                        mistakeRowNum.Add((i, j));
                    }
                }
            }
            return (isValid, mistakeRowNum);
        }
        /// <summary>
        /// validate columns of game
        /// </summary>
        /// <returns>Tuple of the state of all columns and a list mistakes if any exist; another tuple of the column index and the redundant number</returns>
        private (bool, List<(int, int)>) ValidateColumns(int[,] gameBoard)
        {
            int size = Constants.BoardSize;
            bool isValid = true;
            ushort[] freq = new ushort[size + 1];
            List<(int, int)> mistakeColumnNum = new List<(int, int)>();
            for (int i = 0; i < size; i++)
            {
                Array.Clear(freq, 0, freq.Length);
                for (int j = 0; j < size; j++)
                {
                    int value = gameBoard[j, i];
                    if (value != 0)
                    {
                        if (value < 1 || value > 9)
                        {
                            throw new ArgumentException("Not a valid number.");
                        }
                        freq[value]++;
                    }
                }
                for (int j = 1; j <= size; ++j)
                {
                    if (freq[j] > 1)
                    {
                        isValid = false;
                        mistakeColumnNum.Add((i, j));
                    }
                }
            }
            return (isValid, mistakeColumnNum);
        }
        /// <summary>
        /// Validate one sub-box
        /// </summary>
        /// <param name="gameBoard"> Game Board</param>
        /// <param name="boxIndex"> index of the sub-box</param>
        /// <returns>Tuple of the state of the sub-box and the number that was redundant</returns>
        private (bool validBox, List<int> redundantNums) ValidateBox(int[,] gameBoard, int boxIndex)
        {
            bool isValid = true;
            ushort[] freq = new ushort[Constants.BoardSize + 1];
            List<int> redundantNums = new List<int>();
            int startRow = (boxIndex / Constants.boxSize) * Constants.boxSize;
            int StartColumn = (boxIndex % Constants.boxSize) * Constants.boxSize;
            int index = 0;
            for (int row = startRow; row < startRow + 3; row++)
            {
                for (int col = StartColumn; col < StartColumn + 3; col++)
                {
                    int value = gameBoard[row, col];
                    if (value != 0)
                    {
                        if (value < 1 || value > 9)
                        {
                            throw new ArgumentException("Not a valid number.");
                        }
                        freq[value]++;
                    }
                }
            }
            for (int i = 1; i <= Constants.BoardSize; ++i)
            {
                if (freq[i] > 1)
                {
                    isValid = false;
                    redundantNums.Add(i);
                }
            }
            return (isValid, redundantNums);
        }
        /// <summary>
        /// Validate Board
        /// </summary>
        private void ValidateBoard(int[,] gameBoard)
        {
            if (gameBoard == null)
            {
                throw new ArgumentNullException("Board is Empty");
            }
            if (gameBoard.GetLength(0) != gameBoard.GetLength(1))
            {
                throw new ArgumentException("Board not Squared");
            }
            if ((gameBoard.GetLength(0) != 9) || (gameBoard.GetLength(1) != 9))
            {
                throw new ArgumentException("Board not in valid size.", nameof(gameBoard));
            }
        }
    }
    public class GameDataHandler : IGameDataHandler
    {
        /// <summary>
        /// Error Documentation handler
        /// </summary>
        /// <param name="rowNum">tuple of the index and the redundant number</param>
        /// <param name="RowColumnBoxCase">row or column of box</param>
        /// <returns> String to be added to mistakes of game</returns>
        public static string DocumentError((int position, int num) rowNum, int RowColumnBoxCase)
        {
            return RowColumnBoxCase switch
            {
                1 => "The number " + rowNum.num + " was redundant in row number: " + (rowNum.position + 1),
                2 => "The number " + rowNum.num + " was redundant in column number: " + (rowNum.position + 1),
                3 => "The number " + rowNum.num + " was redundant in box number: " + (rowNum.position + 1),
                _ => "Invalid Input."
            };
        }
        public static States GetState(bool valid)
        {
            return valid switch
            {
                true => States.Valid,
                false => States.NotValid
            };
        }
        public static string GetState(States state)
        {
            return state switch
            {
                States.Valid => "Good Job! It's Valid.",
                States.NotValid => "You lose! Practice makes perfect.",
                _ => "Not valid State."
            };
        }
    }
}
namespace SudokuValidator.UserInterface
{
    public class OutputHandler : IOutputHandle
    {
        private const string separator = "=======================================================";
        public void DisplayHeader()
        {
            Console.WriteLine(separator);
            Console.WriteLine("=============== Sudoku Game Validator =================");
            Console.WriteLine(separator);
        }
        public void DisplayFooter()
        {
            Console.WriteLine(separator);
            Console.WriteLine("========== Thank you for using our validator ==========");
            Console.WriteLine(separator);
        }
        public void DisplayResult(string message) { Console.WriteLine(message + '\n' + separator); }
        public void DisplayError(string message) { Console.WriteLine(message); }
        public void DisplayMistakes(List<string> mistakes)
        {
            int mistakeCount = mistakes.Count;
            for (int i = 0; i < mistakeCount; i++)
            {
                Console.WriteLine(mistakes[i]);
            }
        }
    }
    public class InputHandler : IInputHandler
    {
        /// <summary>
        /// Reads desire to continue for more checks
        /// </summary>
        public bool ShouldContinue()
        {
            Console.Write("Would you want to try check more games? (y/n): ");
            string decision = Console.ReadLine().Trim().ToLower();
            return decision == "y" || decision == "yes";
        }
        /// <summary>
        /// Read board
        /// </summary>
        public int[,] ReadBoard()
        {
            Console.WriteLine("Enter game board line by line, 9 space-separated integers.");
            int[,] board = new int[Constants.BoardSize, Constants.BoardSize];
            bool idone = false, jdone = false;
            for (int row = 0; row < 9; row++)
            {
                while (true)
                {
                    Console.Write("Enter Row number #" + (row + 1) + " : ");
                    string[] numbers = Console.ReadLine().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (numbers.Length != 9)
                    {
                        Console.WriteLine($"Need 9 numbers, got {numbers.Length}. Try again.");
                        continue;
                    }

                    bool validRow = true;
                    for (int col = 0; col < 9; col++)
                    {
                        if (!int.TryParse(numbers[col], out int num) || num < 1 || num > 9)
                        {
                            Console.WriteLine($"Invalid number '{numbers[col]}' at position {col + 1}");
                            validRow = false;
                            break;
                        }
                        board[row, col] = num;
                    }

                    if (validRow) break;
                }
            }
            return board;
        }
    }
}
namespace SudokuValidator.Application
{
    public class SudokuValidatorApplication
    {
        IInputHandler inputHandler;
        IOutputHandle outputHandler;
        IGameDataHandler gameDataHandler;
        IValidateGame validateGame;
        public SudokuValidatorApplication(IInputHandler inputHandler, IOutputHandle outputHandler, IGameDataHandler gameDataHandler, IValidateGame validateGame)
        {
            this.inputHandler = inputHandler;
            this.outputHandler = outputHandler;
            this.gameDataHandler = gameDataHandler;
            this.validateGame = validateGame;
        }
        public void Run()
        {
            var originalFor = Console.ForegroundColor;
            var originalBack = Console.BackgroundColor;
            try
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Green;
                while (true)
                {
                    Game game = new Game();
                    outputHandler.DisplayHeader();
                    game.gameBoard = inputHandler.ReadBoard();
                    bool isValid = validateGame.GetState(game);
                    outputHandler.DisplayResult(GameDataHandler.GetState(GameDataHandler.GetState(isValid)));
                    outputHandler.DisplayMistakes(game.Mistakes);
                    if (!inputHandler.ShouldContinue())
                    {
                        outputHandler.DisplayFooter();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                outputHandler.DisplayError(ex.Message);
            }
            finally { Console.ForegroundColor = originalFor; Console.BackgroundColor = originalBack; }
        }
    }
}
namespace SudokuValidator
{
    public class SudokuValidator
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Run tests? (y/n): ");
            //if (Console.ReadLine()?.ToLower() == "y")
            //{
            //    Tests.Tests.TestAll();
            //    Console.WriteLine("Press any key to exit...");
            //    Console.ReadKey();
            //    return;
            //}

            InputHandler inputHandler = new InputHandler();
            OutputHandler outputHandler = new OutputHandler();
            GameDataHandler gameDataHandler = new GameDataHandler();
            SudokuValdate sudokuValdate = new SudokuValdate();

            SudokuValidatorApplication app = new SudokuValidatorApplication(inputHandler, outputHandler, gameDataHandler, sudokuValdate);
            app.Run();
        }
    }
}
namespace SudokuValidator.Tests
{
    public class Tests
    {
        private static int passed = 0;
        private static int failed = 0;
        public static void TestAll()
        {
            Console.WriteLine("================ Running Tests ==============");
            RowsTests();
            ColumnsTests();
            BoxesTests();
            ValidBoardTest();
            Console.WriteLine("==============================================");
            Console.WriteLine($"==== Results: {passed} passed, {failed} failed ====");
        }
        private static void Assert(bool condition, string testName)
        {
            if (condition)
            {
                Console.WriteLine($"! Pass !: {testName}");
                passed++;
            }
            else
            {
                Console.WriteLine($"!! Failed !! {testName}");
                failed++;
            }
        }
        private static void RowsTests()
        {
            var game = new Game
            {
                gameBoard = new int[,]
                {
                    { 1, 2, 3, 4, 5, 6, 7, 8, 1 },  // '1' appears twice in row 0
                    { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
                    { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
                    { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
                    { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
                    { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
                    { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
                    { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
                    { 9, 1, 2, 3, 4, 5, 6, 7, 8 }
                }
            };
            var validator = new SudokuValdate();
            bool isValid = validator.GetState(game);
            Assert(isValid == false, "Row validation detects duplicate");
            Assert(game.Mistakes != null && game.Mistakes.Count > 1, "Mistakes list contains error.");
        }
        private static void ColumnsTests()
        {
            var game = new Game
            {
                gameBoard = new int[,]
                {
                    { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
                    { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
                    { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
                    { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
                    { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
                    { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
                    { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
                    { 1, 2, 3, 4, 5, 6, 7, 8, 9 }   // '1' appears twice in column 0 (row 0 and row 8)
                }
            };
            var validator = new SudokuValdate();
            bool isValid = validator.GetState(game);
            Assert(isValid == false, "Column validation detects duplicates.");
            Assert(game.Mistakes != null && game.Mistakes.Count > 1, "Mistakes list contains error.");
        }
        private static void BoxesTests()
        {
            var game = new Game
            {
                gameBoard = new int[,]
                {
                    { 1, 1, 3, 4, 5, 6, 7, 8, 9 },  // Two 1's in top-left box (positions 0,0 and 0,1)
                    { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
                    { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
                    { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
                    { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
                    { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
                    { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
                    { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
                    { 9, 1, 2, 3, 4, 5, 6, 7, 8 }
                }
            };
            var validator = new SudokuValdate();
            bool isValid = validator.GetState(game);
            Assert(isValid == false, "Box validator detects error");
            Assert(game.Mistakes != null && game.Mistakes.Count > 1, "Mistakes list contains error.");
        }
        private static void ValidBoardTest()
        {
            var game = new Game
            {
                gameBoard = new int[,]
                {
                    { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
                    { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
                    { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
                    { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
                    { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
                    { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
                    { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
                    { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
                    { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
                }
            };
            var validator = new SudokuValdate();
            bool isValid = validator.GetState(game);
            Assert(isValid == true, "board is valid.");
        }
    }
}