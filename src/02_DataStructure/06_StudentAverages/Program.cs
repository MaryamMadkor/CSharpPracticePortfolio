/*
 * 6.Jagged Array Analyzer(Intermediate)

    Analyze a jagged array of test scores.

    Concepts: Jagged arrays, nested loops.

    Task: For each student(row), calculate their average test score.
*/
using System.Globalization;

///*
namespace StudentScore
{
    // Logic class (Business Layer)
    public static class CalcStudentAverage
    {

        private const double A_GRADE_MIN = 90.0;
        private const double B_GRADE_MIN = 80.0;
        private const double C_GRADE_MIN = 70.0;
        private const double D_GRADE_MIN = 60.0;
        private const double E_GRADE_MIN = 50.0;

        /// <summary>
        /// Validate the given array
        /// </summary>
        /// <param name="array">given array</param>
        private static void ValidateArray(double[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Empty Array.", nameof(array));
        }
        /// <summary>
        /// Sums the scores of each student
        /// </summary>
        /// <param name="array">One student scores</param>
        /// <returns>Sum of scores</returns>
        public static double SumArray(double[] array)
        {
            ValidateArray(array);
            double sum = 0;
            int count = array.Length;
            for (int i = 0; i < count; i++)
            {
                sum += array[i];
            }
            return sum;
        }
        /// <summary>
        /// Gets grade according to average.
        /// </summary>
        /// <param name="scoreAverage">Average of one student</param>
        /// <returns>Character grade</returns>
        public static char GetGrade(double average)
        {
            if (double.IsNaN(average) || double.IsInfinity(average))
                throw new ArgumentException("Invalid average value", nameof(average));

            if (average >= A_GRADE_MIN) return 'A';
            if (average >= B_GRADE_MIN) return 'B';
            if (average >= C_GRADE_MIN) return 'C';
            if (average >= D_GRADE_MIN) return 'D';
            if (average >= E_GRADE_MIN) return 'E';
            return 'F';
        }
        /// <summary>
        /// Calculates the average of all students
        /// </summary>
        /// <param name="studentsScores">Array of Averages of all students scores</param>
        /// <returns>Array of averages of all students</returns>
        public static double[] CalculateAverage(double[][] studentsScores)
        {
            int studentCount = studentsScores.Length;
            double[] averageScores = new double[studentCount];
            for (int i = 0; i < studentCount; i++)
            {
                int oneCount = studentsScores[i].Length;
                double sum = SumArray(studentsScores[i]);
                averageScores[i] = sum / oneCount;
            }
            return averageScores;
        }
    }
    // User interface layer UI/UX
    public static class UserInterface
    {
        private const string separator = "============================================================================";
        private const int minLength = 1;
        private const int maxLength = 100;
        /// <summary>
        /// Displays header of program
        /// </summary>
        public static void DisplayHeader()
        {
            Console.WriteLine(separator);
            Console.WriteLine("======================== Student Average Calculator ========================");
            Console.WriteLine(separator);
        }
        /// <summary>
        /// Displays footer of program
        /// </summary>
        public static void DisplayFooter()
        {
            Console.WriteLine(separator);
            Console.WriteLine("========================= Thanks For Using Our App =========================");
            Console.WriteLine(separator);
        }
        /// <summary>
        /// Displays the Averages of all students
        /// </summary>
        /// <param name="averages">Array of averages of all students</param>
        public static void DisplaySummary(double[] averages)
        {
            int Count = averages.Length;
            Console.WriteLine("The Average of students scores are:");
            for (int i = 0; i < Count; i++)
            {
                Console.WriteLine($"The average of student number {i + 1} is: \n\t{averages[i]}, The grade is: {GetGrade(averages[i])}");
                Console.WriteLine(separator);
            }
        }
        /// <summary>
        /// Gets char Grade and the states it represents
        /// </summary>
        /// <param name="averageScore">Average of one student</param>
        /// <returns>String meaning the status of the students</returns>
        public static string GetGrade(double averageScore)
        {
            char grade = CalcStudentAverage.GetGrade(averageScore);
            if (grade == 'A' || grade == 'B' || grade == 'C' || grade == 'D')
                return $"{grade}, You pass. Congrats!";
            else if (grade == 'E' || grade == 'F')
                return $"{grade}, You Fail. Best of luck next time!";
            else
                return string.Empty;
        }
        /// <summary>
        /// Gets a proper integer positive number
        /// </summary>
        /// <param name="prompt">User-Friendly prompt for needed number</param>
        /// <param name="min">minimum value of length of the students</param>
        /// <param name="max">maximum value of length of the students</param>
        /// <returns>valid positive integer</returns>
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
        /// Reads the Scores of each student
        /// </summary>
        /// <param name="count">The number of scores expected</param>
        /// <param name="i">index of the student</param>
        /// <returns>Array of student index i scores</returns>
        public static double[] GetOneStudent(int expectedCount, int studentIndex)
        {
            while (true)
            {
                Console.Write($"Enter Student {studentIndex + 1}'s {expectedCount} scores (space-separated): ");
                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }

                var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != expectedCount)
                {
                    Console.WriteLine($"Expected {expectedCount} numbers, got {parts.Length}.");
                    continue;
                }

                var scores = new double[expectedCount];
                bool success = true;

                for (int j = 0; j < parts.Length; j++)
                {
                    if (!double.TryParse(parts[j], NumberStyles.Any, CultureInfo.InvariantCulture, out scores[j]))
                    {
                        Console.WriteLine($"Invalid number: '{parts[j]}' at position {j + 1}.");
                        success = false;
                        break;
                    }
                    if (scores[j] < 0 || scores[j] > 100)
                    {
                        Console.WriteLine($"Score {scores[j]} is out of range (0-100).");
                        success = false;
                        break;
                    }
                }

                if (success) return scores;
            }
        }
        /// <summary>
        /// Reads a jagged of all students scores
        /// </summary>
        /// <returns>jagged array of scores of all students</returns>
        public static double[][] GetStudentScores()
        {
            int studentCount = GetPositiveInteger("Enter Number of students: ", minLength, maxLength);
            double[][] StudentsScores = new double[studentCount][];
            for (int i = 0; i < studentCount; i++)
            {
                int StudentScoreCount = GetPositiveInteger($"Enter number of scores for student {i + 1}: ", minLength, maxLength);
                StudentsScores[i] = GetOneStudent(StudentScoreCount, i);
            }
            return StudentsScores;
        }
        /// <summary>
        /// Reads user's choice to continue or stop
        /// </summary>
        /// <returns>Boolean value of yes or no</returns>
        public static bool ShouldContinue()
        {
            Console.Write("Would you to get more averages? (y/n):");
            string decision = Console.ReadLine().Trim().ToLower();
            return decision == "y" || decision == "yes";
        }
    }
    // Orchistra layer: Main
    public static class Program
    {
        public static void Main(string[] args)
        {
            UserInterface.DisplayHeader();
            while (true)
            {
                try
                {
                    double[][] scores = UserInterface.GetStudentScores();
                    UserInterface.DisplaySummary(CalcStudentAverage.CalculateAverage(scores));
                    if (!UserInterface.ShouldContinue())
                    {
                        break;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                
            }
            UserInterface.DisplayFooter();
        }
    }
}