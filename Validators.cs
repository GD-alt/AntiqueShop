using System;
using System.Text.RegularExpressions;

namespace Validators
{
    public abstract class Validator
    {
        public abstract bool IsValid(string input);
    }

    public class Email : Validator
    {
        private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");

        public override bool IsValid(string input)
        {
            return EmailRegex.IsMatch(input);
        }
    }

    public class Phone : Validator
    {
        private static readonly Regex PhoneRegex = new Regex(@"^(\+7|8)?9\d{9}$");

        public override bool IsValid(string input)
        {
            return PhoneRegex.IsMatch(input);
        }
    }
}