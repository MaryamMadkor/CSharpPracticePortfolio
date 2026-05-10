/*
9. Tic-Tac-Toe Win Checker (Advanced)

Check for a winner in a 3x3 grid.

Concepts: 2D arrays, conditional checks, logical operators.

Task: Validate rows, columns, and diagonals for 3 matching marks (X/O).
*/

// interface namespace: contracts:
using TicTacToeWinCheck.Application;
using TicTacToeWinCheck.Contracts;
using TicTacToeWinCheck.Contracts.Models;
using TicTacToeWinCheck.Logic;
using TicTacToeWinCheck.UserInterface;

namespace TicTacToeWinCheck.Contracts
{
    public interface IDetermineWinner
    {
        public void DetermineWinner(TicTacToe game);
    }
    public interface IInterpretEnums
    {
        public Sizes GetGameSizeFromInt(int givenSize);
        public string GetGameStateString(States state);
    }
    public interface IInputHandler
    {
        public char[,] GetTicTacToeBoard(int? index);
        public bool ShouldContinue();
        public int GetSizeChoice();

    }
    public interface IOutputHandler
    {
        public void DisplayHeader();
        public void DisplayFooter();
        public void DisplayResult(string gameState);
        public void DisplayBoard(char[,] board);
        public void DisplayError(string message);
    }
    public interface TicTacToeCheckApplication
    {
        public void Run();
    }
}
namespace TicTacToeWinCheck.Contracts.Models
{
    public enum Sizes
    {
        none = 0,
        normal = 3,
        ultimate = 9
    }
    public enum States
    {
        Incomplete = 0,
        Xwin = 1,
        Owin = 2,
        Draw = 3
    }
    public class GameConstants
    {
        public const char xPlayer = 'x';
        public const char oPlayer = 'o';
        public const char whiteSpaceSymbol = '-';
        public const int normalSizeInt = (int)Sizes.normal;
        public const int ultimateSizeInt = (int)Sizes.ultimate;
    }
    public class TicTacToe
    {
        public States State = States.Incomplete;
        public Sizes Size;
        public char[,] gameBoard;
    }
}
// logic namespace - implementation of contracts - Services
namespace TicTacToeWinCheck.Logic
{
    public class CheckWinner : IDetermineWinner, IInterpretEnums // could be broken into two classes, each doing only one thing, but it would break everything now. so no.
    {

        /// <summary>
        /// Final determine of game state given to the game class object directly.
        /// </summary>
        public void DetermineWinner(TicTacToe game)
        {
            ValidateBoard(game.gameBoard);
            char? winner = GetWinner(game.gameBoard);
            if (winner == GameConstants.xPlayer)
            {
                game.State = States.Xwin;
                return;
            }
            if (winner == GameConstants.oPlayer)
            {
                game.State = States.Owin;
                return;
            }

            bool hasEmptySpaces = HasEmptySpaces(game.gameBoard);
            game.State = hasEmptySpaces ? States.Incomplete : States.Draw;
        }

        /// <summary>
        /// Returns game winner as a character.
        /// </summary>
        private char? GetWinner(char[,] board)
        {
            var lines = new[]
            {
                // Rows
                ((0,0), (0,1), (0,2)),
                ((1,0), (1,1), (1,2)),
                ((2,0), (2,1), (2,2)),
                // Columns
                ((0,0), (1,0), (2,0)),
                ((0,1), (1,1), (2,1)),
                ((0,2), (1,2), (2,2)),
                // Diagonals
                ((0,0), (1,1), (2,2)),
                ((0,2), (1,1), (2,0))
            };

            foreach (var ((r1, c1), (r2, c2), (r3, c3)) in lines)
            {
                if (board[r1, c1] != GameConstants.whiteSpaceSymbol &&
                    board[r1, c1] == board[r2, c2] &&
                    board[r2, c2] == board[r3, c3])
                    return board[r1, c1];
            }

            return null;
        }
        /// <summary>
        /// Determines if there is any Empty cells
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private bool HasEmptySpaces(char[,] board)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == GameConstants.whiteSpaceSymbol)
                        return true;
            return false;
        }




        /// <summary>
        /// Interprets size given by user int an enum size
        /// </summary>

        public Sizes GetGameSizeFromInt(int givenSize)
        {
            return givenSize switch
            {
                GameConstants.normalSizeInt => Sizes.normal,
                GameConstants.ultimateSizeInt => Sizes.ultimate,
                _ => throw new ArgumentException("Not an Acceptable Size.", nameof(givenSize))
            };
        }


        /// <summary>
        /// Returns a user-friendly message for each game state.
        /// </summary>
        public string GetGameStateString(States state)
        {
            return state switch
            {
                States.Owin => "O is the winner! Congrats!!",
                States.Xwin => "X is the winner! Congrats!!",
                States.Incomplete => "Game is incomplete again, so maybe play again?",
                States.Draw => "It's a DRAW, best of luck next time.",
                _ => "Something Went Wrong, Please Try again."
            };
        }



        /// <summary>
        /// Validates board for null, square and size
        /// </summary>
        private void ValidateBoard(char[,] board)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            if (board.GetLength(0) != 3 || board.GetLength(1) != 3)
                throw new ArgumentException("Board must be 3x3", nameof(board));

            // Validate all cells contain valid characters
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    char c = board[i, j];
                    if (c != GameConstants.xPlayer &&
                        c != GameConstants.oPlayer &&
                        c != GameConstants.whiteSpaceSymbol)
                    {
                        throw new ArgumentException(
                            $"Invalid character '{c}' at position [{i},{j}]");
                    }
                }
            }
        }
    }
}
//UI
namespace TicTacToeWinCheck.UserInterface
{
    public class InputHandler : IInputHandler
    {
        /// <summary>
        /// Reads user choice for size
        /// </summary>
        public int GetSizeChoice()
        {
            int size = GetPositiveInteger("Please Enter Game board size (-> 3 for normal, -> 9 for ultimate): ");
            return size;
        }
        private int GetPositiveInteger(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && (value == GameConstants.normalSizeInt || value == GameConstants.ultimateSizeInt))
                    return value;

                Console.WriteLine($"Please enter a positive whole number: {GameConstants.normalSizeInt} for normal size board, {GameConstants.ultimateSizeInt} for ultimate size board.");
            }
        }
        /// <summary>
        /// Reads one 3x3 tic tac toe game board, in case of 9x9 it reads only an indexed 3x3 board.
        /// </summary>
        public char[,] GetTicTacToeBoard(int? boardIndex)
        {
            Console.WriteLine(boardIndex == null
                ? "\nEnter Tic-Tac-Toe board:"
                : $"\nEnter small board #{boardIndex + 1}:");

            Console.WriteLine("Enter 9 characters (row by row, use 'x', 'o', or '-'):");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }

                var chars = input.Replace(" ", "").ToCharArray();

                if (chars.Length != 9)
                {
                    Console.WriteLine($"Expected 9 characters, got {chars.Length}.");
                    continue;
                }

                var board = new char[3, 3];
                bool valid = true;

                for (int i = 0; i < 9; i++)
                {
                    int row = i / 3;
                    int col = i % 3;

                    if (chars[i] != GameConstants.xPlayer &&
                        chars[i] != GameConstants.oPlayer &&
                        chars[i] != GameConstants.whiteSpaceSymbol)
                    {
                        Console.WriteLine($"Invalid character '{chars[i]}' at position {i + 1}.");
                        Console.WriteLine("Use only: 'x', 'o', or '-'");
                        valid = false;
                        break;
                    }

                    board[row, col] = chars[i];
                }

                if (valid) return board;
            }
        }
        /// <summary>
        /// Reads desire to continue for more checks
        /// </summary>
        public bool ShouldContinue()
        {
            Console.Write("Would you want to try check more games? (y/n): ");
            string decision = Console.ReadLine().Trim().ToLower();
            return decision == "y" || decision == "yes";
        }
    }
    public class OutputHandler : IOutputHandler
    {
        public const string separator = "==========================================================================";
        public void DisplayHeader()
        {
            Console.WriteLine(separator);
            Console.WriteLine("====================== Tic Tac Toe Game Win Checker ======================");
            Console.WriteLine(separator);
        }
        public void DisplayFooter()
        {
            Console.WriteLine(separator);
            Console.WriteLine("=======================  Thanks For Using Our App  =======================");
            Console.WriteLine(separator);
        }
        public void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {message}");
            Console.ResetColor();
        }
        public void DisplayResult(string gameState)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Game state is: {gameState}.");
            Console.WriteLine(separator);
        }
        public void DisplayBoard(char[,] board)
        {
            Console.WriteLine("\nBoard:");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(" ");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i, j]);
                    if (j < 2) Console.Write(" | ");
                }
                Console.WriteLine();
                if (i < 2) Console.WriteLine("---+---+---");
            }
        }
    }
}
// namespace for application: Run
namespace TicTacToeWinCheck.Application
{
    public class TicTacToeApplication : TicTacToeCheckApplication
    {
        public readonly IDetermineWinner determineWinner;
        public readonly IInterpretEnums interpretEnums;
        public readonly IOutputHandler outputHandler;
        public readonly IInputHandler inputHandler;
        public TicTacToeApplication(IDetermineWinner determineWinner, IInterpretEnums interpretEnums, IOutputHandler outputHandler, IInputHandler inputHandler)
        {
            this.determineWinner = determineWinner;
            this.interpretEnums = interpretEnums;
            this.outputHandler = outputHandler;
            this.inputHandler = inputHandler;
        }

        public void Run()
        {
            var originalFore = Console.ForegroundColor;
            var originalBack = Console.BackgroundColor;
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = ConsoleColor.White;
                    outputHandler.DisplayHeader();
                    int size = inputHandler.GetSizeChoice();
                    TicTacToe game = new TicTacToe();
                    game.Size = interpretEnums.GetGameSizeFromInt(size);
                    if (size == GameConstants.normalSizeInt)
                    {
                        game.gameBoard = inputHandler.GetTicTacToeBoard(null);
                    }
                    else
                    {
                        game.gameBoard = BuildUltimateBoard();
                    }
                    determineWinner.DetermineWinner(game);
                    outputHandler.DisplayBoard(game.gameBoard);
                    outputHandler.DisplayResult(interpretEnums.GetGameStateString(game.State));
                    if (!inputHandler.ShouldContinue())
                    {
                        outputHandler.DisplayFooter(); break;
                    }
                }
                catch (Exception e)
                {
                    outputHandler.DisplayError(e.Message);
                }
                finally
                {
                    Console.ForegroundColor = originalFore;
                    Console.BackgroundColor = originalBack;
                }
            }
        }
        private char[,] BuildUltimateBoard()
        {
            var metaBoard = new char[3, 3];

            for (int smallBoard = 0; smallBoard < 9; smallBoard++)
            {
                int row = smallBoard / 3;
                int col = smallBoard % 3;

                var smallGame = new TicTacToe
                {
                    gameBoard = inputHandler.GetTicTacToeBoard(smallBoard)
                };

                determineWinner.DetermineWinner(smallGame);

                metaBoard[row, col] = smallGame.State switch
                {
                    States.Xwin => GameConstants.xPlayer,
                    States.Owin => GameConstants.oPlayer,
                    _ => GameConstants.whiteSpaceSymbol
                };
            }

            return metaBoard;
        }
    }
}
// main namespace
namespace TicTacToeWinCheck
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var checkWinner = new CheckWinner();
            var outputHandler = new OutputHandler();
            var inputHandler = new InputHandler();

            var app = new TicTacToeApplication(checkWinner, checkWinner, outputHandler, inputHandler);
            app.Run();
        }
    }
}
