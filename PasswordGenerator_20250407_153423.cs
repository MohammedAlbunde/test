using System;
using System.Text;
using System.Security.Cryptography;

namespace MCP.Demo
{
    /// <summary>
    /// A secure password generator created via MCP integration
    /// Created on April 7, 2025
    /// </summary>
    public class PasswordGenerator
    {
        private readonly RandomNumberGenerator _rng;
        
        // Character sets for password generation
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NumericChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";
        
        public PasswordGenerator()
        {
            _rng = RandomNumberGenerator.Create();
        }
        
        /// <summary>
        /// Generates a cryptographically secure random password
        /// </summary>
        /// <param name="length">Length of the password</param>
        /// <param name="includeLowercase">Include lowercase letters</param>
        /// <param name="includeUppercase">Include uppercase letters</param>
        /// <param name="includeNumbers">Include numbers</param>
        /// <param name="includeSpecial">Include special characters</param>
        /// <returns>A secure random password</returns>
        public string GeneratePassword(
            int length = 12,
            bool includeLowercase = true,
            bool includeUppercase = true,
            bool includeNumbers = true,
            bool includeSpecial = true)
        {
            // Validate parameters
            if (length <= 0)
                throw new ArgumentException("Password length must be positive", nameof(length));
                
            if (!(includeLowercase || includeUppercase || includeNumbers || includeSpecial))
                throw new ArgumentException("At least one character set must be included");
            
            // Build character set
            var charSet = new StringBuilder();
            if (includeLowercase) charSet.Append(LowercaseChars);
            if (includeUppercase) charSet.Append(UppercaseChars);
            if (includeNumbers) charSet.Append(NumericChars);
            if (includeSpecial) charSet.Append(SpecialChars);
            
            string availableChars = charSet.ToString();
            char[] password = new char[length];
            
            // Generate password
            for (int i = 0; i < length; i++)
            {
                password[i] = availableChars[GetRandomInt(0, availableChars.Length)];
            }
            
            return new string(password);
        }
        
        /// <summary>
        /// Generates a cryptographically secure random integer within a specified range
        /// </summary>
        private int GetRandomInt(int min, int max)
        {
            if (min >= max)
                throw new ArgumentException("Max must be greater than min");
                
            byte[] randomNumber = new byte[4];
            _rng.GetBytes(randomNumber);
            
            // Convert to an integer and scale to desired range
            int value = BitConverter.ToInt32(randomNumber, 0);
            
            // We use Math.Abs and modulo to ensure the value is within our range
            return min + (Math.Abs(value) % (max - min));
        }
    }
    
    /// <summary>
    /// Program demonstrates the use of PasswordGenerator
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MCP C# Password Generator Demo");
            Console.WriteLine("===============================");
            
            var generator = new PasswordGenerator();
            
            // Generate a default password
            string defaultPassword = generator.GeneratePassword();
            Console.WriteLine($"Default Password (12 chars, all types): {defaultPassword}");
            
            // Generate a longer password
            string longPassword = generator.GeneratePassword(20);
            Console.WriteLine($"Long Password (20 chars): {longPassword}");
            
            // Generate a numeric PIN
            string pin = generator.GeneratePassword(6, false, false, true, false);
            Console.WriteLine($"Numeric PIN (6 digits): {pin}");
            
            // Generate an alphanumeric password (no special chars)
            string alphanumeric = generator.GeneratePassword(10, true, true, true, false);
            Console.WriteLine($"Alphanumeric Password (10 chars): {alphanumeric}");
            
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
