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
    }   
}
