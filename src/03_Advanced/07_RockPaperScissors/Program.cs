/*

7. Rock-Paper-Scissors Game (Intermediate)

Play against the computer.

Concepts: Random numbers, conditionals, Boolean logic.

Task: Generate a computer choice (0=Rock, 1=Paper, 2=Scissors) and determine the winner.
*/
using RockPaperScissors.Application;
using RockPaperScissors.Contracts;
using RockPaperScissors.Logic;
using RockPaperScissors.UI;


///*
///*
// Interfaces namespace
namespace RockPaperScissors.Contracts
{
    public interface IChoiceGenerator
    {
        ChoiceOptions GetPCChoice();
        int GenerateRandom();
    }
    public interface IGameRules
    {
        GameStates DetermineGameState(ChoiceOptions user, ChoiceOptions pc);
        string GetChoiceName(ChoiceOptions choice);
        string GetStateName(GameStates state);
        ChoiceOptions InterpretUserChoice(int userChoice);
    }
    public interface IOutputProvider
    {
        public void DisplayHeader();
        public void DisplayFooter();
        public void DisplayResult(string pc, string user, string gameState);
        public void DisplayError(string message);
    }
    public interface IInputProvider
    {
        public int GetUserChoice();
        public int GetPositiveInteger(string message, int min, int max);
        public bool ShouldContinue();
    }
    // game statistics wins, loses, draws, games played
    public interface IRockPaperScissorsApplication
    {
        public void Run();
    }
}
// Logic nampespace
namespace RockPaperScissors.Logic
{

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
        Draw = 4
    }

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

    public class Game : IGameRules
    {
        /// <summary>
        /// Interprets User integer choice into an enum choice for calculation
        /// </summary>
        /// <param name="userChoice">user int choice</param>
        /// <returns>Enum choice of user</returns>
        public ChoiceOptions InterpretUserChoice(int userChoice)
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
        public string GetChoiceName(ChoiceOptions choice)
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
        /// <summary>
        /// Give user-friendly state of game
        /// </summary>
        /// <param name="state">enum of state of game</param>
        /// <returns>user-understandable string of game state</returns>
        public string GetStateName(GameStates state)
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
}
// UI namespace
namespace RockPaperScissors.UI
{
    public class InputProvider : IInputProvider
    {
        private const int minChoice = 1, maxChoice = 3;
        /// <summary>
        /// Read positive integer from user
        /// </summary>
        /// <param name="prompt">usesr-friendly message about needed parameter</param>
        /// <param name="min">low edge value of the choice</param>
        /// <param name="max">high edge value of the choice</param>
        /// <returns>validated integer</returns>
        public int GetPositiveInteger(string prompt, int min, int max)
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
        /// Reads user choise
        /// </summary>
        /// <returns>user choice as integer</returns>
        public int GetUserChoice()
        {
            int choice = GetPositiveInteger("Enter Your choice as:\n1. Rock.\n2. Paper.\n3. Scissors.\n", minChoice, maxChoice);
            return choice;
        }
        /// <summary>
        /// Reads user's will to continue
        /// </summary>
        /// <returns>boolean value meaning yes or no</returns>
        public bool ShouldContinue()
        {
            Console.Write("Would you to play more? (y/n): ");
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
        private readonly IGameRules game;
        private readonly IChoiceGenerator choiceGenerator;
        public RockPaperScissorsApplication(IOutputProvider outputProvider, IInputProvider inputProvider, IGameRules game, IChoiceGenerator choiceGenerator)
        {
            this.outputProvider = outputProvider ?? throw new ArgumentNullException();
            this.inputProvider = inputProvider ?? throw new ArgumentNullException();
            this.game = game ?? throw new ArgumentNullException();
            this.choiceGenerator = choiceGenerator;
        }
        public void Run()
        {
            outputProvider.DisplayHeader();
            while (true)
            {
                try
                {
                    int user = inputProvider.GetUserChoice();
                    var userChoice = game.InterpretUserChoice(user);
                    var pc = choiceGenerator.GetPCChoice();
                    var gameState = game.DetermineGameState(userChoice, pc);
                    outputProvider.DisplayResult(game.GetChoiceName(pc), game.GetChoiceName(userChoice), game.GetStateName(gameState));
                    if (!inputProvider.ShouldContinue()) break;
                }
                catch (Exception ex)
                {
                    outputProvider.DisplayError(ex.Message);
                }

            }
            outputProvider.DisplayFooter();
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
            var game = new Game();
            var output = new OutputProvider();
            var input = new InputProvider();
            var pcChoice = new ChoiceGenerator();


            var app = new RockPaperScissorsApplication(output, input, game, pcChoice);
            app.Run();
        }
    }
}