using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    internal interface IUserManager
    {
        string HashSha256(string source);
        UserVM SelectUserByEmail(string email);
        bool VerifyUser(string email, string passwordHash);
        bool UpdateUser(UserVM userVM, string newFName, string newLName, string newEmail, string newImageURL);
        List<User> GetInactiveUsers();
        List<User> GetActiveUsers();
        bool UpdateUserIsActive(int userID, bool isActive);
    }
}
