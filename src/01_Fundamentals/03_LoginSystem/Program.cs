/*
 * 3.Simple Login System(Beginner)

        Validate a username and password.

        Concepts: String comparison, logical operators (&&).

        Task: Check if username is "admin" and password is "1234". Grant access or deny.
*/
using System.Data;

///*
namespace LoginValidation
{
    public static class LoginValidator
    {
        private const string username = "admin";
        private const string password = "1234";

        /// <summary>
        /// validates username and password to one given by attempting user.
        /// </summary>
        /// <param name="_username"> given username by user</param>
        /// <param name="_password">given password by user</param>
        /// <returns>boolean value, true if the same</returns>

        public static bool Validate(string _username, string _password)
        {
            return string.Equals(_username, username, StringComparison.Ordinal) && string.Equals(_password, password, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// starting point main
        /// </summary>
        /// <param name="args"></param>

        private const int trials = 3;

        public static void Main(string[] args)
        {

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("----------------- Log In Validator App -----------------");
            Console.WriteLine("--------------------------------------------------------");
            int remainingTrials = trials;
            bool valid = false;
            while (remainingTrials > 0)
            {
                string username = GetUsernameFromUser();
                string password = GetPasswordFromUser();
                if (LoginValidator.Validate(username, password))
                {
                    Console.WriteLine("Welcome Back!");
                    valid = true;
                    break;
                }
                else
                {
                    remainingTrials--;
                    if (remainingTrials > 0)
                        Console.WriteLine("Password or Username are not correct, please try again.");
                }
            }
            if (!valid) { Console.WriteLine("Account is Locked, Too many failed attemps."); }
            Console.WriteLine("---------------------------------------------------------");
        }
        /// <summary>
        /// accepts and validates the username
        /// </summary>
        /// <returns>string of the given username</returns>
        private static string GetUsernameFromUser()
        {
            Console.Write("Please Enter Your Username: ");
            string username = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(username)) { Console.WriteLine("Username can not be empty."); return string.Empty; }
            return username;
        }

        /// <summary>
        /// accepts the given password
        /// </summary>
        /// <returns> password to test</returns>
        private static string GetPasswordFromUser()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Enter &&
                         key.Key != ConsoleKey.Backspace &&
                         !char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
    }
}