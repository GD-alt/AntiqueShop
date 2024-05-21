using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AntiqueShop
{
    public class Encoder
    {
        // Method to hash a string using SHA256 algorithm
        public static string HashString(string input)
        {
            // Create a SHA256 hash object
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);

                // Compute the hash of the input bytes
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                // Return the hexadecimal hash string
                return hash;
            }
        }

        // Method to validate a string against a given hash
        public static bool ValidateString(string input, string hash)
        {
            // Hash the input string using SHA256
            string calculatedHash = HashString(input);

            // Compare the calculated hash with the given hash
            return calculatedHash == hash;
        }
    }
}
