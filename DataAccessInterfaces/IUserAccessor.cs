using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
