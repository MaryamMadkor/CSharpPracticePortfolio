/*

7. Rock-Paper-Scissors Game (Intermediate)

Play against the computer.

Concepts: Random numbers, conditionals, Boolean logic.

Task: Generate a computer choice (0=Rock, 1=Paper, 2=Scissors) and determine the winner.
*/
using RockPaperScissors.Application;
using RockPaperScissors.Contracts;
using RockPaperScissors.Contracts.Models;
using RockPaperScissors.Logic;
using RockPaperScissors.UI;
using System.Runtime.CompilerServices;



// Interfaces namespace
namespace RockPaperScissors.Contracts
{
    public interface IChoiceGenerator
    {
        public ChoiceOptions GetPCChoice();
    }
    public interface IGameRules
    {
        public GameStates DetermineGameState(ChoiceOptions user, ChoiceOptions pc);

    }
    public interface IOutputProvider
    {
        public void DisplayHeader();
        public void DisplayFooter();
        public void DisplayResult(string pc, string user, string gameState);
        public void DisplayCount();
        public void DisplayHistory();
        public void DisplayError(string message);
    }
    public interface IInputProvider
    {
        public int GetUserChoice();
        public bool ShouldContinue();
        public bool GetHistoryChoice();
    }
    // game statistics wins, loses, draws, games played
    public interface IRockPaperScissorsApplication
    {
        public void Run();
    }
}
namespace RockPaperScissors.Contracts.Models
{
    public class Game
    {
        public ChoiceOptions PlayerChoice;
        public ChoiceOptions PcChoice;
        public GameStates State;
        public Game(ChoiceOptions playerChoice, ChoiceOptions pcChoice, GameStates state = GameStates.None)
        {
            PlayerChoice = playerChoice;
            PcChoice = pcChoice;
            State = state;
        }
    }
    public class GameSession
    {
        public static int GameCount = 0;
        public static int PlayerWinCount = 0;
        public static int PlayerLoseCount = 0;
        public static int DrawCount = 0;
        public static List<(ChoiceOptions pc, ChoiceOptions player, GameStates state)> PlayerPCHistoryChoices = new List<(ChoiceOptions, ChoiceOptions, GameStates)>();
        public static void CalculateStatistics(Game game)
        {
            GameCount++;
            PlayerPCHistoryChoices.Add((game.PcChoice, game.PlayerChoice, game.State));

            switch (game.State)
            {
                case GameStates.UserWin:
                    PlayerWinCount++;
                    break;
                case GameStates.PCWin:
                    PlayerLoseCount++;
                    break;
                case GameStates.Draw:
                    DrawCount++;
                    break;
                default:
                    throw new ArgumentException("Not a valid state.");
            }
        }
        public static void ClearStatistics()
        {
            PlayerPCHistoryChoices?.Clear();
            PlayerWinCount = 0;
            PlayerLoseCount = 0;
            DrawCount = 0;
            GameCount = 0;
        }
    }
    public class Constants
    {
        public const int minChoiceInt = 1;
        public const int maxChoiceInt = 3;
        public const int historyChoice = 1;
        public const int continueChoice = 2;
        public const int endGameChoice = 3;
    }
    public enum ChoiceOptions : Int16
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2
    }
    public enum GameStates : Int16
    {
        UserWin = 0,
        PCWin = 2,
        Draw = 4,
        None = -1
    }
}
// Logic nampespace
namespace RockPaperScissors.Logic
{

    public class ChoiceGenerator : IChoiceGenerator
    {
        private static readonly Random _random = new Random();
        public int GenerateRandom()
        {
            return _random.Next(0, 3);
        }
        /// <summary>
        /// Generates PC Choice
        /// </summary>
        /// <returns>Enum of pc choice</returns>
        public ChoiceOptions GetPCChoice()
        {
            int choice = GenerateRandom();
            return choice switch
            {
                0 => ChoiceOptions.Rock,
                1 => ChoiceOptions.Paper,
                2 => ChoiceOptions.Scissors,
                _ => throw new InvalidOperationException()
            };
        }
    }
    public static class DataHandler
    {
        /// <summary>
        /// Interprets User integer choice into an enum choice for calculation
        /// </summary>
        /// <param name="userChoice">user int choice</param>
        /// <returns>Enum choice of user</returns>
        public static ChoiceOptions InterpretUserChoice(int userChoice)
        {
            return userChoice switch
            {
                1 => ChoiceOptions.Rock,
                2 => ChoiceOptions.Paper,
                3 => ChoiceOptions.Scissors,
                _ => throw new InvalidOperationException()
            };
        }
        /// <summary>
        /// Give user understandable choice
        /// </summary>
        /// <param name="choice">enum choice pc or user</param>
        /// <returns>user-friendly string for display</returns>
        public static string GetChoiceName(ChoiceOptions choice)
        {
            return choice switch
            {
                ChoiceOptions.Rock => "Rock",
                ChoiceOptions.Paper => "Paper",
                ChoiceOptions.Scissors => "Scissors",
                _ => throw new ArgumentException("Not a valid Choice.", nameof(choice))
            };
        }
        /// <summary>
        /// Give user-friendly state of game
        /// </summary>
        /// <param name="state">enum of state of game</param>
        /// <returns>user-understandable string of game state</returns>
        public static string GetStateName(GameStates state)
        {
            return state switch
            {
                GameStates.UserWin => "You Win! Congrats!",
                GameStates.PCWin => "You Lose, Better luck next time,",
                GameStates.Draw => "DRAW! Play again?",
                _ => throw new ArgumentException("Not a valid Choice.", nameof(state))
            };
        }
    }
    public class GameRules : IGameRules
    {
        /// <summary>
        /// Calculates game state - core game logic
        /// </summary>
        /// <param name="user">user Enum choice</param>
        /// <param name="pc">pc enum choice</param>
        /// <returns>enum of game state</returns>
        public GameStates DetermineGameState(ChoiceOptions user, ChoiceOptions pc)
        {
            if (user == pc) return GameStates.Draw;

            return (user, pc) switch
            {
                (ChoiceOptions.Rock, ChoiceOptions.Scissors) => GameStates.UserWin,
                (ChoiceOptions.Paper, ChoiceOptions.Rock) => GameStates.UserWin,
                (ChoiceOptions.Scissors, ChoiceOptions.Paper) => GameStates.UserWin,
                _ => GameStates.PCWin
            };
        }

    }
}
// UI namespace
namespace RockPaperScissors.UI
{
    public class InputProvider : IInputProvider
    {
        /// <summary>
        /// Read positive integer from user
        /// </summary>
        /// <param name="prompt">usesr-friendly message about needed parameter</param>
        /// <param name="min">low edge value of the choice</param>
        /// <param name="max">high edge value of the choice</param>
        /// <returns>validated integer</returns>
        private int GetPositiveInteger(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Please enter a positive whole number between {min} and {max}.");
            }
        }
        /// <summary>
        /// Reads user choise
        /// </summary>
        /// <returns>user choice as integer</returns>
        public int GetUserChoice()
        {
            int choice = GetPositiveInteger("Enter Your choice as:\n1. Rock.\n2. Paper.\n3. Scissors.\n", Constants.minChoiceInt, Constants.maxChoiceInt);
            return choice;
        }
        /// <summary>
        /// Reads user's will to continue
        /// </summary>
        /// <returns>boolean value meaning yes or no</returns>
        public bool ShouldContinue()
        {
            Console.Write("Would you to play more? (y/n):");
            string decision = Console.ReadLine().Trim().ToLower();
            return decision == "y" || decision == "yes";
        }
        public bool GetHistoryChoice()
        {
            Console.Write("Would you like to see the history of all games? (y/n):");
            string decision = Console.ReadLine().Trim().ToLower();
            return decision == "y" || decision == "yes";
        }
    }
    public class OutputProvider : IOutputProvider
    {
        private const string separator = "============================================================================";
        public void DisplayHeader()
        {
            Console.WriteLine(separator);
            Console.WriteLine("========================  Rock Paper Scissors Game  ========================");
            Console.WriteLine(separator);
        }
        public void DisplayFooter()
        {
            Console.WriteLine(separator);
            Console.WriteLine("========================= Thanks For Using Our App =========================");
            Console.WriteLine(separator);
        }
        public void DisplayResult(string pc, string user, string gameState)
        {
            Console.WriteLine(separator);
            Console.WriteLine($"Pc is: {pc}, and your choice is: {user}");
            Console.WriteLine($"GAME RESULT IS: ===  {gameState}  ===");
            Console.WriteLine(separator);
        }
        public void DisplayCount()
        {
            Console.WriteLine(separator);
            Console.WriteLine($"Games Played: {GameSession.GameCount}.\nWin Count = {GameSession.PlayerWinCount} | Lose Count = {GameSession.PlayerLoseCount} | Draw Count = {GameSession.DrawCount}.");
            Console.WriteLine(separator);
        }
        public void DisplayHistory()
        {
            int gamecount = GameSession.PlayerPCHistoryChoices.Count;
            Console.WriteLine("|      Game No.     |      Player     |       PC      |       Winner      |");
            for (int i = 0; i < gamecount; i++)
            {
                string state = GameSession.PlayerPCHistoryChoices[i].state == GameStates.UserWin ? "YOU" : GameSession.PlayerPCHistoryChoices[i].state == GameStates.PCWin ? "PC" : "Draw";
                Console.WriteLine($"|    {i + 1,-10}    |    {GameSession.PlayerPCHistoryChoices[i].player,-10}    |    {GameSession.PlayerPCHistoryChoices[i].pc,-8}    |    {state,-10}    |");
            }
            Console.WriteLine(separator);
        }
        public void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {message}");
            Console.ResetColor();
        }
    }
}
// Application namespace
namespace RockPaperScissors.Application
{
    public class RockPaperScissorsApplication : IRockPaperScissorsApplication
    {
        private readonly IOutputProvider outputProvider;
        private readonly IInputProvider inputProvider;
        private readonly IGameRules gameRule;
        private readonly IChoiceGenerator choiceGenerator;
        public RockPaperScissorsApplication(IOutputProvider outputProvider, IInputProvider inputProvider, IGameRules game, IChoiceGenerator choiceGenerator)
        {
            this.outputProvider = outputProvider ?? throw new ArgumentNullException();
            this.inputProvider = inputProvider ?? throw new ArgumentNullException();
            this.gameRule = game ?? throw new ArgumentNullException();
            this.choiceGenerator = choiceGenerator;
        }
        public void Run()
        {

            var originalFore = Console.ForegroundColor;
            var originalBack = Console.BackgroundColor;
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.White;
                    outputProvider.DisplayHeader();
                    int user = inputProvider.GetUserChoice();
                    var userChoice = DataHandler.InterpretUserChoice(user);
                    var pc = choiceGenerator.GetPCChoice();
                    var gameState = gameRule.DetermineGameState(userChoice, pc);
                    var game = new Game(userChoice, pc, gameState);
                    GameSession.CalculateStatistics(game);
                    outputProvider.DisplayResult(DataHandler.GetChoiceName(pc), DataHandler.GetChoiceName(userChoice), DataHandler.GetStateName(gameState));
                    outputProvider.DisplayCount();
                    bool endGame = false;
                    if (inputProvider.GetHistoryChoice())
                    {
                        outputProvider.DisplayHistory();
                    }
                    if (!inputProvider.ShouldContinue())
                    {
                        outputProvider.DisplayFooter();
                        break;
                    }

                }
                catch (Exception ex)
                {
                    outputProvider.DisplayError(ex.Message);
                }
                finally
                {
                    Console.ForegroundColor = originalFore;
                    Console.BackgroundColor = originalBack;
                }
            }

        }
    }
}
// Start Point Final Game namespace
namespace RockPaperScissors
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Tests.Tests.TestAll();
            var game = new GameRules();
            var output = new OutputProvider();
            var input = new InputProvider();
            var pcChoice = new ChoiceGenerator();


            var app = new RockPaperScissorsApplication(output, input, game, pcChoice);
            app.Run();
        }
    }
}
namespace RockPaperScissors.Tests
{
    public class Tests
    {
        public static int passedTests = 0;
        public static int failedTests = 0;
        public static void TestAll()
        {
            Console.WriteLine("============== Running All Tests ===============");

            TestPlayerWin();
            TestPCWin();
            TestDraw();

            Console.WriteLine($"=================== Passed = {passedTests} | Failed = {failedTests} =======================");
        }
        private static void Assert(bool condition, string TestName)
        {
            if (condition)
            {
                Console.WriteLine(TestName);
                passedTests++;
            }
            else
            {
                failedTests++;
            }
        }
        private static void TestPlayerWin()
        {
            GameSession.ClearStatistics();
            var game = new Game(ChoiceOptions.Scissors, ChoiceOptions.Paper);
            var gameRules = new GameRules();
            game.State = gameRules.DetermineGameState(game.PlayerChoice, game.PcChoice);
            GameSession.CalculateStatistics(game);
            Assert(game.State == GameStates.UserWin && GameSession.PlayerWinCount > 0 && GameSession.PlayerPCHistoryChoices.Count > 0, "Player Win test Is Valid.");
            GameSession.ClearStatistics();
        }
        private static void TestPCWin()
        {
            GameSession.ClearStatistics();
            var game = new Game(ChoiceOptions.Scissors, ChoiceOptions.Rock);
            var gameRules = new GameRules();
            game.State = gameRules.DetermineGameState(game.PlayerChoice, game.PcChoice);
            GameSession.CalculateStatistics(game);
            Assert(game.State == GameStates.PCWin && GameSession.PlayerLoseCount > 0 && GameSession.PlayerPCHistoryChoices.Count > 0, "PC Win test Is Valid.");
            GameSession.ClearStatistics();
        }
        private static void TestDraw()
        {
            GameSession.ClearStatistics();
            var game = new Game(ChoiceOptions.Paper, ChoiceOptions.Paper);
            var gameRules = new GameRules();
            game.State = gameRules.DetermineGameState(game.PlayerChoice, game.PcChoice);
            GameSession.CalculateStatistics(game);
            Assert(game.State == GameStates.Draw && GameSession.DrawCount > 0 && GameSession.PlayerPCHistoryChoices.Count > 0, "Player Win test Is Valid.");
            GameSession.ClearStatistics();
        }
    }
}
