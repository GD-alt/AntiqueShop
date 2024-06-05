using System;
using System.Text.RegularExpressions;

namespace AntiqueShop.Utils
{
    public static class Validators {
        /// <summary>
        /// Validates a Russian phone number in either +792190008080 or 89219008080 format.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns>True if the phone number is valid, false otherwise.</returns>
        public static bool ValidatePhone(string phoneNumber)
        {
            // Remove all non-digit characters from the phone number
            phoneNumber = Regex.Replace(phoneNumber, @"[^0-9\+]", "");

            if (Regex.IsMatch(phoneNumber, @"(\+7|8)\d{10}$"))
                return true;

            return false;
        }

        /// <summary>
        /// Validates an email address.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email address is valid, false otherwise.</returns>
        public static bool ValidateEmail(string email)
        {
            // Use a regular expression to validate the email address
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));
        }
    }
}