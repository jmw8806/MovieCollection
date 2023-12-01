using DataObjects;
using System.Collections.Generic;

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
        bool ResetPassword(string email, string oldPassword, string newPassword);
        bool ResetPasswordAdmin(string email);
    }
}
