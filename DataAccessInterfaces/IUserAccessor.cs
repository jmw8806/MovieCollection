using DataObjects;
using System.Collections.Generic;

namespace DataAccessInterfaces
{
    public interface IUserAccessor
    {
        UserVM SelectUserByEmail(string email);
        string SelectRoleByUserID(int userID);
        int VerifyUserWithEmailAndPasswordHash(string email, string passwordHash);
        int UpdateUser(int userID, string newFName, string newLName, string newEmail, string newImgURL, string oldFName, string oldLName, string oldEmail);
        List<User> GetInactiveUsers();
        List<User> GetActiveUsers();
        int UpdateUserIsActive(int userID, bool isActive);
        int UpdatePasswordHash(string email, string oldPasswordHash, string newPasswordHash);
        int ResetPasswordAdmin(string email);
    }
}
