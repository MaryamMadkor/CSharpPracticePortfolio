/*
 * 1.Temperature Analyzer(Beginner)

        Check if water would freeze or boil at a given temperature.

        Concepts: Data types, conditional statements.

        Task: Read a temperature value. Print "Freezing" if ≤0°C, "Boiling" if ≥100°C, otherwise "Normal".
*/
using System.Runtime.CompilerServices;
namespace TemperatureAnalyzing
{
    public class TemperatureAnalyzer
    {
        private const int BoilingPointCel = 100;
        private const int FreezingPointCel = 0;
        public static string AnalyzeTemperature(float temperature)
        {
            if (temperature >= BoilingPointCel) return "Boiling";
            if (temperature <= FreezingPointCel) return "Freezing";
            return "Normal";
        }
        public static void ValidateTemperatre(float Temperatrue)
        {
            if (float.IsNaN(Temperatrue)) throw new ArgumentException("Temperature must be a number.", nameof(Temperatrue));
            if (float.IsInfinity(Temperatrue)) throw new ArgumentException("Temeprature can not be infinity.");

        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("===========================================================");
            Console.WriteLine("================= Water Analyzing Program =================");
            Console.WriteLine("===========================================================");
            while (true)
            {
                try
                {
                    float temperature = ReadFromUserTemperature();
                    string waterState = TemperatureAnalyzer.AnalyzeTemperature(temperature);
                    Console.WriteLine($"At Temperature {temperature}, Water is: {waterState}.");
                    if (!ShouldContiue())
                    {
                        break;
                    }
                }
                catch (Exception ex) when (ex is ArgumentException or ArgumentOutOfRangeException)
                {
                    Console.WriteLine($"\nError: {ex.Message}\n");
                }
            }
            Console.WriteLine("===========================================================");
            Console.WriteLine("=============== Thank you for using our App ===============");
            Console.WriteLine("===========================================================");
        }
        private static float ReadFromUserTemperature()
        {
            Console.Write("Enter Celsius Temeprature: ");
            if (!float.TryParse(Console.ReadLine(), out float temp))
            {
                throw new ArgumentException("Please Enter a Valid Number.");
            }
            return temp;
        }
        private static bool ShouldContiue()
        {
            Console.Write("Would you like to analyze another temperature? (y/n): ");
            string response = Console.ReadLine()?.Trim().ToLower();

            return response == "y" || response == "yes";
        }
    }
}