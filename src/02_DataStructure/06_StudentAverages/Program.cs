/*
 * 6.Jagged Array Analyzer(Intermediate)

    Analyze a jagged array of test scores.

    Concepts: Jagged arrays, nested loops.

    Task: For each student(row), calculate their average test score.
*/
using StudentScore.Application;
using StudentScore.Contracts;
using StudentScore.Contracts.Models;
using StudentScore.Logic;
using StudentScore.UserInterface;
using System.Globalization;
namespace StudentScore.Contracts
{
    public interface IOutputProvider
    {
        public void DisplayHeader();
        public void DisplayFooter();
        public void DisplayError(string message);
        public void DisplayStudentById(int id);
        public void DisplayALlStudents();
    }
    public interface IInputProvider
    {
        public List<float> GetOneStudent(out int id);
        public int GetChoice();
        public int GetPositiveInteger(string prompt, int min, int max);
        public bool ShouldContinue(string message);

    }
    public interface IDataHandler
    {
        public void AddStudent(Student student);
        public void UpdateStudent(Student student);
        public bool CheckIDExist(int id);
        public void DeleteById(int id);

    }
    public interface IInterpretEnum
    {
        public Grades GetGrade(float average);
        public string GetGrade(Grades grade);
        public Choices GetChoices(int choice);
    }
    public interface IStudentLogic
    {
        public float CalculateAverage(List<float> studentsScores);

    }
    public interface IApplication
    {
        public void Run();
    }
}
namespace StudentScore.Contracts.Models
{
    public class Student
    {
        public int studentID {  get; set; }
        public List<float> scores {  get; set; }
        public Grades grade {  get; set; }
        public float average {  get; set; }
        public Student(int studentID, List<float> scores, Grades grade, float average)
        {
            this.studentID = studentID;
            this.scores = scores;
            this.grade = grade;
            this.average = average;
        }
    }
    public enum Grades
    {
        GradeA , GradeB, GradeC, GradeD, GradeE, GradeF
    }
    public enum Choices
    {
        none = 0,
        ViewAll = 1,
        AddMore = 2, 
        Update = 3,
        GetByID = 4,
        DeleteByID = 5,
        Exit = 6
    }
    public class Constants
    {
        public const double A_GRADE_MIN = 90.0;
        public const double B_GRADE_MIN = 80.0;
        public const double C_GRADE_MIN = 70.0;
        public const double D_GRADE_MIN = 60.0;
        public const double E_GRADE_MIN = 50.0;
        public const string filePath = @"Students.txt";
        public const int minBound = 0;
        public const int studentMaxBound = 10000;
        public const float scoreLimit = 100.0f;
    }
}
namespace StudentScore.Logic
{
    public class StudentLogic: IStudentLogic
    {


        /// <summary>
        /// Validate the given array
        /// </summary>
        /// <param name="array">given array</param>
        private void ValidateArray(List<float> array)
        {
            if (array == null || array.Count == 0)
                throw new ArgumentException("Empty Array.", nameof(array));
        }
        /// <summary>
        /// Sums the scores of each student
        /// </summary>
        /// <param name="array">One student scores</param>
        /// <returns>Sum of scores</returns>
        private float SumArray(List<float> scores)
        {
            ValidateArray(scores);
            float sum = 0;
            int count = scores.Count;
            for (int i = 0; i < count; i++)
            {
                sum += scores[i];
            }
            return sum;
        }
        /// <summary>
        /// Calculates the average of all students
        /// </summary>
        /// <param name="studentsScores">Array of Averages of all students scores</param>
        /// <returns>Array of averages of all students</returns>
        public float CalculateAverage(List<float> studentsScores)
        {
            int studentCount = studentsScores.Count;
            float sum = SumArray(studentsScores);
            float averageScore = sum / studentCount;
            return averageScore;
        }
    }
    public class InterpretEnum: IInterpretEnum
    {
        /// <summary>
        /// Interpret Average to a Grade enum
        /// </summary>
        public Grades GetGrade(float average)
        {
            if (float.IsNaN(average) || float.IsInfinity(average))
                throw new ArgumentException("Invalid average value", nameof(average));
            if (average >= Constants.A_GRADE_MIN) return Grades.GradeA;
            else if (average >= Constants.B_GRADE_MIN) return Grades.GradeB;
            else if (average >= Constants.C_GRADE_MIN) return Grades.GradeC;
            else if (average >= Constants.D_GRADE_MIN) return Grades.GradeD;
            else if (average >= Constants.E_GRADE_MIN) return Grades.GradeE;
            else return Grades.GradeF;
        }
        /// <summary>
        /// Interprets Grade enum Into a String
        /// </summary>
        public string GetGrade(Grades grade)
        {
            return grade switch
            {
                Grades.GradeA => "Grade A: Excellent!",
                Grades.GradeB => "Grade B: Very Good!",
                Grades.GradeC => "Grade C: Good+.",
                Grades.GradeD => "Grade D: Good-.",
                Grades.GradeE => "Grade E: Acceptable.",
                Grades.GradeF => "Grade F: Failed.",
                _ => throw new ArgumentException("Not a valid Grade.")
            };
        }
        /// <summary>
        /// Interprets int Choice into an enum
        /// </summary>
        public Choices GetChoices(int choice)
        {
            return choice switch
            {
                1 => Choices.ViewAll,
                2 => Choices.AddMore,
                3 => Choices.Update,
                4 => Choices.GetByID,
                5 => Choices.DeleteByID,
                6 => Choices.Exit,
                _ => throw new ArgumentException("Not a Valid Choice")
            };
        }
    }
    public class DataHandler: IDataHandler
    {
        private readonly IInterpretEnum _interpretEnum;
        public DataHandler(IInterpretEnum interpretEnum)
        {
            _interpretEnum = interpretEnum;
        }
        /// <summary>
        /// Delete By ID
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="id"></param>
        public void DeleteById(int id)
        {
            if (!File.Exists(Constants.filePath))
                throw new ArgumentException($"File does not exist");

            string[] lines = File.ReadAllLines(Constants.filePath);
            if(lines.Length == 0) throw new ArgumentException($"No Students Added Yet.");
            List<string> updatedLines = new List<string>();
            bool found = false;
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (int.Parse(parts[0]) != id)
                {
                    updatedLines.Add(line);
                }
                else { found  = true; }
            }
            if (!found) { throw new ArgumentException("Student Doesn't Exist."); }
            File.WriteAllLines(Constants.filePath, updatedLines);
            Console.WriteLine("Student Deleted Successfully!");
        }

        /// <summary>
        /// Adds a new student data.
        /// </summary>
        public void AddStudent(Student student)
        {
            if (!File.Exists(Constants.filePath))
            {
                throw new ArgumentException("File Doesn't Exist.");
            }

            string[] lines = File.ReadAllLines(Constants.filePath);
            Array.Resize(ref lines, lines.Length + 1);
            lines[lines.Length - 1] = $"{student.studentID}|{_interpretEnum.GetGrade(student.grade)}|{string.Join(",", student.scores)}";
            File.WriteAllLines(Constants.filePath, lines);
            Console.WriteLine("Student Added Successfully!");
        }

        /// <summary>
        /// Update an Existing Student
        /// </summary>
        public void UpdateStudent(Student student)
        {
            if (!File.Exists(Constants.filePath))
            {
                throw new ArgumentException("File Doesn't Exist.");
            }

            string[] lines = File.ReadAllLines(Constants.filePath);
            bool found = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                if (int.Parse(parts[0]) == student.studentID)
                {
                    // Replace line
                    lines[i] = $"{student.studentID}|{_interpretEnum.GetGrade(student.grade)}|{string.Join(",", student.scores)}";
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new ArgumentException("Student Doesn't Exist.");
            }
            Console.WriteLine("Student Updated Successfully!");
        }
        /// <summary>
        /// Ensure if ID already Exists.
        /// </summary>
        public bool CheckIDExist(int id)
        {
            if (!File.Exists(Constants.filePath))
                throw new ArgumentException("File Doesn't Exist.");

            string[] lines = File.ReadAllLines(Constants.filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (int.Parse(parts[0]) == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
namespace StudentScore.UserInterface
{
    public class OutputProvider : IOutputProvider
    {
        private const string separator = "============================================================================";
        /// <summary>
        /// Displays header of program
        /// </summary>
        public void DisplayHeader()
        {
            Console.WriteLine(separator);
            Console.WriteLine("======================== Student Management System ========================");
            Console.WriteLine(separator);
        }
        /// <summary>
        /// Displays footer of program
        /// </summary>
        public void DisplayFooter()
        {
            Console.WriteLine(separator);
            Console.WriteLine("========================= Thanks For Using Our App =========================");
            Console.WriteLine(separator);
        }
        public void DisplayError(string message) 
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {message}");
            Console.ResetColor();
        }
        /// <summary>
        /// Display All Student id, grade and scores
        /// </summary>
        public void DisplayALlStudents()
        {
            if (!File.Exists(Constants.filePath))
                throw new ArgumentException("File Doesn't Exist.");

            string[] lines = File.ReadAllLines(Constants.filePath);
            if(lines.Length == 0)
            {
                throw new ArgumentException("No students found");
            }
            Console.WriteLine("|     Student ID     |     Grade     |          Scores          |");
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                Console.Write($"|     {parts[0], -10}     |     {parts[1], -12}     | [ ");
                string[] studentScores = parts[2].Split(',');
                int scoreCount = studentScores.Length;
                for (int i = 0; i < scoreCount; i++)
                {
                    Console.Write((i < (scoreCount - 1) ? $"{studentScores[i]}, " : $"{studentScores[i]}"));
                }
                Console.WriteLine(" ] |");
            }
        }
        /// <summary>
        /// Display One Student by ID
        /// </summary>
        public void DisplayStudentById(int id)
        {
            if (!File.Exists(Constants.filePath))
                throw new ArgumentException("File Doesn't Exist.");

            string[] lines = File.ReadAllLines(Constants.filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (int.Parse(parts[0]) == id)
                {
                    Console.Write($"Student ID = {id}, Grade is: {parts[1]}\n\tScores: [ ");
                    string[] studentScores = parts[2].Split(',');
                    int scoreCount = studentScores.Length;
                    for (int i = 0; i < scoreCount; i++)
                    {
                        Console.Write((i < (scoreCount - 1)? $"{studentScores[i]}, ": $"{studentScores[i]}"));
                    }
                    Console.WriteLine(" ]");
                }
            }
        }
    }
    public class InputProvider : IInputProvider
    {
        /// <summary>
        /// Gets a proper integer positive number
        /// </summary>
        /// <param name="prompt">User-Friendly prompt for needed number</param>
        /// <param name="min">minimum value of length of the students</param>
        /// <param name="max">maximum value of length of the students</param>
        /// <returns>valid positive integer</returns>
        public int GetPositiveInteger(string prompt, int min, int max)
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
        /// Reads the Scores of each student
        /// </summary>
        /// <param name="count">The number of scores expected</param>
        /// <param name="i">index of the student</param>
        /// <returns>Array of student index i scores</returns>
        public List<float> GetOneStudent(out int id)
        {
            while (true)
            {
                id = GetPositiveInteger("Enter Student ID: ", Constants.minBound, Constants.studentMaxBound);
                Console.Write($"Enter Scores {id}'s scores (space-separated): ");
                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }

                var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var scores = new List<float>();
                bool success = true;

                for (int j = 0; j < parts.Length; j++)
                {
                    if (!float.TryParse(parts[j], NumberStyles.Any, CultureInfo.InvariantCulture, out float score))
                    {
                        Console.WriteLine($"Invalid number: '{parts[j]}' at position {j + 1}.");
                        success = false;
                        break;
                    }
                    scores.Add(score);
                    if (score < Constants.minBound || score > Constants.scoreLimit)
                    {
                        Console.WriteLine($"Score {score} is out of range (0-100).");
                        success = false;
                        break;
                    }
                }
                if (success) return scores;
            }
        }
        public int GetChoice()
        {
            int choice = GetPositiveInteger("What do you Want to Do?\n(1: Get All Students | 2: Add a Student | 3: Update a Student By ID | 4: View A Student By ID | 5: Delete a Student By ID | 6: Exit):\n>", (int) Choices.ViewAll, (int) Choices.Exit);
            return choice;
        }
        public bool ShouldContinue(string message)
        {
            Console.Write(message);
            string decision = Console.ReadLine().Trim().ToLower();
            return decision == "y" || decision == "yes";
        }
    }
}
namespace StudentScore.Application
{
    public class StudentScoreApp : IApplication
    {
        IOutputProvider outputProvider;
        IInputProvider inputProvider;
        IStudentLogic studentLogic;
        IDataHandler dataHandler;
        IInterpretEnum interpretEnum;
        public StudentScoreApp(IOutputProvider outputProvider, IInputProvider inputProvider, IStudentLogic studentLogic, IDataHandler dataHandler, IInterpretEnum interpretEnum)
        {
            this.outputProvider = outputProvider;
            this.inputProvider = inputProvider;
            this.studentLogic = studentLogic;
            this.dataHandler = dataHandler;
            this.interpretEnum = interpretEnum;
        }

        public void Run()
        {
            var originalBack = Console.BackgroundColor;
            var originalFore = Console.ForegroundColor;
            while(true)
            {

                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = ConsoleColor.White;
                    outputProvider.DisplayHeader();
                    bool exit = false;
                    var choice = interpretEnum.GetChoices(inputProvider.GetChoice());
                    switch (choice)
                    {
                        case Choices.ViewAll:
                            outputProvider.DisplayALlStudents();
                            break;
                        case Choices.GetByID:
                            int idDisplay = inputProvider.GetPositiveInteger("Enter Student ID: ", Constants.minBound, Constants.studentMaxBound);
                            if (!dataHandler.CheckIDExist(idDisplay))
                            {
                                outputProvider.DisplayError("Student ID doesn't exist.");
                                break;
                            }
                            outputProvider.DisplayStudentById(idDisplay);
                            break;
                        case Choices.AddMore:
                            int idAdd;
                            var scoresAdd = inputProvider.GetOneStudent(out idAdd);
                            if (dataHandler.CheckIDExist(idAdd)){
                                outputProvider.DisplayError("This Student ID Already Exists.");
                                break;
                            }
                            float averageAdd = studentLogic.CalculateAverage(scoresAdd);
                            Student studentAdd = new Student(idAdd, scoresAdd, interpretEnum.GetGrade(averageAdd), averageAdd);
                            dataHandler.AddStudent(studentAdd);
                            break;
                        case Choices.Update:
                            int idUpdate;
                            var scoresUpdate = inputProvider.GetOneStudent(out idUpdate);
                            if (!dataHandler.CheckIDExist(idUpdate))
                            {
                                outputProvider.DisplayError("No ID Match Was Found.");
                                break;
                            }
                            if (!inputProvider.ShouldContinue("Are You Sure You Want To Update This Student Data? (y/n): ")) break;
                            float averageUpdate = studentLogic.CalculateAverage(scoresUpdate);
                            Student studentUpdate = new Student(idUpdate, scoresUpdate, interpretEnum.GetGrade(averageUpdate), averageUpdate);
                            dataHandler.UpdateStudent(studentUpdate);
                            break;
                        case Choices.DeleteByID:
                            int idDelete = inputProvider.GetPositiveInteger("Enter Student ID: ", Constants.minBound, Constants.studentMaxBound);
                            if (!dataHandler.CheckIDExist(idDelete))
                            {
                                outputProvider.DisplayError("Student ID doesn't exist.");
                                break;
                            }
                            if (!inputProvider.ShouldContinue("Are You Sure You Want To Delete This Student? (y/n): ")) break;
                            dataHandler.DeleteById(idDelete);
                            break;
                        case Choices.Exit:
                            exit = true;
                            break;
                        default:
                            outputProvider.DisplayError("Not a valid Choice");
                            break;
                    }
                    if(exit) { outputProvider.DisplayFooter(); break; }
                }
                catch(Exception e)
                {
                    outputProvider.DisplayError(e.Message);
                }
                finally { Console.ForegroundColor = originalFore; Console.BackgroundColor = originalBack; }
            }
        }
    }
}
namespace StudentScore
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Tests.Tests.TestAll();
            InputProvider inputProvider = new InputProvider();
            OutputProvider outputProvider = new OutputProvider();
            StudentLogic studentLogic = new StudentLogic();
            InterpretEnum interpretEnum = new InterpretEnum();
            DataHandler dataHandler = new DataHandler(interpretEnum);

            var app = new StudentScoreApp(outputProvider, inputProvider, studentLogic, dataHandler, interpretEnum);

            app.Run();
        }
    }
}
namespace StudentScore.Tests
{
    public class Tests
    {
        private static int Passed = 0;
        private static int Failed = 0;
        public static void TestAll()
        {
            Console.WriteLine("==================== Running Tests =====================");
            Testlogic.TestCalculateAverage();
            Testlogic.TestEmptyList();
            TestDataHandler.TestNonExistingID();
            TestDataHandler.TestExistingID();
            TestInterpretEnum.TestGetGradeGrades();
            TestInterpretEnum.TestGetGradeString();
            TestInterpretEnum.TestGetChoice();
            Console.WriteLine($"============ Passed : {Passed} | Failed : {Failed} ============");
        }
        public static void Assert(bool condition, string testName)
        {
            if (condition)
            {
                Console.WriteLine(testName + " : Passed!");
                Passed++;
            }
            else
            {
                Console.WriteLine(testName + " : Failed.");
                Failed++;
            }
        }
    }
    public class Testlogic
    {
        public static void TestCalculateAverage()
        {
            var studentLogic = new StudentLogic();
            var scoreList = new List<float> { 65.6f, 75.6f, 88.1f };
            float average = studentLogic.CalculateAverage(scoreList);
            Tests.Assert(average == 76.43333f, "Avergae should equal 76.4.");
        }
        public static void TestEmptyList()
        {
            var studentLogic = new StudentLogic();
            var scoreList = new List<float>();
            try
            {
                float average = studentLogic.CalculateAverage(scoreList);
                Tests.Assert(false, "Empty list should throw error.");
            }
            catch (ArgumentException)
            {
                Tests.Assert(true, "Empty list should throw error.");
            }
        }
    }
    public class TestDataHandler
    {
        public static void TestNonExistingID ()
        {
            InterpretEnum interpretEnum = new InterpretEnum();
            DataHandler dataHandler = new DataHandler(interpretEnum);
            bool found = dataHandler.CheckIDExist(999);
            Tests.Assert(found == false, "ID shouldn't exist.");
        }
        public static void TestExistingID()
        {
            InterpretEnum interpretEnum = new InterpretEnum();
            DataHandler dataHandler = new DataHandler(interpretEnum);
            var scoreList = new List<float> { 65.6f, 75.6f, 88.1f };
            Student student = new Student(-1, scoreList, Grades.GradeC, 76.4333f);
            dataHandler.AddStudent(student);
            bool found = dataHandler.CheckIDExist(student.studentID);
            Tests.Assert(found == true, "ID should exist.");
            dataHandler.DeleteById(student.studentID);
        }
    }
    public class TestInterpretEnum
    {
        public static void TestGetGradeGrades()
        {
            var interpretEnum = new InterpretEnum();
            Grades grade = interpretEnum.GetGrade(76.4f);
            Tests.Assert(grade == Grades.GradeC, "76.4 should be Grade C.");
        }
        public static void TestGetGradeString()
        {
            var interpretEnum = new InterpretEnum();
            string grade = interpretEnum.GetGrade(Grades.GradeB);
            Tests.Assert(grade.Contains("Very Good"), "Grade B should be Very Good.");
        }
        public static void TestGetChoice()
        {
            var interpretEnum = new InterpretEnum();
            Choices choice = interpretEnum.GetChoices(3);
            Tests.Assert(choice == Choices.Update, "3 should mean Update by ID.");
        }
    }
}