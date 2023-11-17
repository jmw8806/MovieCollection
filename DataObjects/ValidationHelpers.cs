using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public static class ValidationHelpers
    {
        public static bool IsValidEmail(this string email)
        {
            bool result = false;
            if(email.Length > 14 && email.Length < 255)
            {
                result = true;
            }
            return result;
        }
        public static bool IsValidPassword(this string password)
        {
            bool result = false;
            if (password.Length >= 7)
            {
                result = true;
            }
            return result;
        }
        public static bool isValidYear(this int year)
        {
            bool result = false;
            int min = 1888;
            int max = DateTime.Now.Year;
            if (year >= min || year <= max)
            {
                result = true;
            }
            return result;
        }

        public static bool isValidRuntime(this int runtime)
        {
            bool result = false;
            int min = 1;
            
            if (runtime >= min)
            {
                result = true;
            }
            return result;
        }

        public static bool isValidNumber(this string input)
        {
            bool result = false;
            
            if(int.TryParse(input, out int value))
            {
                result = true;
            }
            return result;
        }
    }   
}
