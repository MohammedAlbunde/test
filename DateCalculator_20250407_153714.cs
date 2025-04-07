using System;
using System.Globalization;

namespace MCP.Demo
{
    /// <summary>
    /// Date Calculator - A C# utility for date calculations
    /// Created via MCP integration on April 7, 2025
    /// </summary>
    public class DateCalculator
    {
        /// <summary>
        /// Calculates the difference between two dates in days, months, and years
        /// </summary>
        public static (int Years, int Months, int Days) CalculateDateDifference(DateTime startDate, DateTime endDate)
        {
            // Ensure the start date is before the end date
            if (startDate > endDate)
            {
                (startDate, endDate) = (endDate, startDate);
            }

            // Calculate years difference
            int years = endDate.Year - startDate.Year;
            
            // Adjust years if needed (if we haven't reached the month/day in the end year)
            if (endDate.Month < startDate.Month || (endDate.Month == startDate.Month && endDate.Day < startDate.Day))
            {
                years--;
            }
            
            // Calculate the months difference
            int months = endDate.Month - startDate.Month;
            if (months < 0)
            {
                months += 12;
            }
            
            // Adjust months if needed (if we haven't reached the day in the end month)
            if (endDate.Day < startDate.Day)
            {
                months--;
                if (months < 0)
                {
                    months += 12;
                }
            }
            
            // Calculate days difference
            int days = endDate.Day - startDate.Day;
            if (days < 0)
            {
                // Get the number of days in the previous month of the end date
                int daysInPreviousMonth = DateTime.DaysInMonth(
                    endDate.Month == 1 ? endDate.Year - 1 : endDate.Year,
                    endDate.Month == 1 ? 12 : endDate.Month - 1);
                
                days += daysInPreviousMonth;
            }
            
            return (years, months, days);
        }
        
        /// <summary>
        /// Adds a specified duration to a date
        /// </summary>
        public static DateTime AddDuration(DateTime startDate, int years, int months, int days)
        {
            // Add each component
            DateTime result = startDate.AddDays(days);
            result = result.AddMonths(months);
            result = result.AddYears(years);
            
            return result;
        }
        
        /// <summary>
        /// Gets the day of week name for a given date
        /// </summary>
        public static string GetDayOfWeek(DateTime date, bool abbreviate = false)
        {
            if (abbreviate)
            {
                return date.ToString("ddd", CultureInfo.InvariantCulture);
            }
            return date.ToString("dddd", CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Calculates age in years, months, and days given a birth date
        /// </summary>
        public static (int Years, int Months, int Days) CalculateAge(DateTime birthDate)
        {
            return CalculateDateDifference(birthDate, DateTime.Today);
        }
        
        /// <summary>
        /// Determines if a year is a leap year
        /// </summary>
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }
    }
    
    /// <summary>
    /// Sample program to demonstrate DateCalculator features
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MCP C# Date Calculator Demo");
            Console.WriteLine("============================");
            
            // Example 1: Calculate age
            DateTime birthDate = new DateTime(1986, 4, 15);
            var age = DateCalculator.CalculateAge(birthDate);
            Console.WriteLine($"If born on April 15, 1986, you are {age.Years} years, {age.Months} months, and {age.Days} days old today.");
            
            // Example 2: Calculate date difference
            DateTime startDate = new DateTime(2020, 3, 15); // COVID-19 pandemic declaration
            DateTime endDate = DateTime.Today;
            var diff = DateCalculator.CalculateDateDifference(startDate, endDate);
            Console.WriteLine($"\nTime since March 15, 2020 (COVID-19 pandemic declaration):");
            Console.WriteLine($"{diff.Years} years, {diff.Months} months, and {diff.Days} days");
            
            // Example 3: Add duration
            DateTime today = DateTime.Today;
            DateTime future = DateCalculator.AddDuration(today, 1, 6, 15);
            Console.WriteLine($"\nToday is {today.ToShortDateString()}");
            Console.WriteLine($"In 1 year, 6 months, and 15 days it will be {future.ToShortDateString()}");
            
            // Example 4: Day of week
            Console.WriteLine($"\nToday is a {DateCalculator.GetDayOfWeek(DateTime.Today)}");
            
            // Example 5: Check leap years
            int currentYear = DateTime.Today.Year;
            Console.WriteLine($"\nIs {currentYear} a leap year? {DateCalculator.IsLeapYear(currentYear)}");
            Console.WriteLine($"Is 2024 a leap year? {DateCalculator.IsLeapYear(2024)}");
            
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
