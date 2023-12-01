using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LogicLayer
{
    public class UserManager : IUserManager
    {
        IUserAccessor _userAccessor = null;

        public UserManager()
        {
            _userAccessor = new UserAccessor();
        }

        public UserManager(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public UserVM LoginUser(string email, string password)
        {
            UserVM userVM = null;
            try
            {
                string passwordHash = HashSha256(password);
                if (VerifyUser(email, passwordHash))
                {
                    userVM = _userAccessor.SelectUserByEmail(email);
                    userVM.roles = _userAccessor.SelectRoleByUserID(userVM.userID);
                }
                else
                {
                    throw new ApplicationException("Login Failed");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Authentication failure", ex);
            }

            return userVM;
        }

        public string HashSha256(string source)
        {
            string hash = null;

            // we need a byte array
            byte[] data;
            // we need a hash provider
            using (SHA256 sha256hasher = SHA256.Create())
            {
                // hashes the input as a byte array
                data = sha256hasher.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            // create a stringbuilder
            var s = new StringBuilder();

            // loop through the hashed data byte list making characters for a string.
            for (int i = 0; i < data.Length; i++)
            {
                // grab every byte of byte array and output as hex
                s.Append(data[i].ToString("x2"));
            }
            // convert stringbuilder to string
            hash = s.ToString();
            return hash;
        }


        public UserVM SelectUserByEmail(string email)
        {
            UserVM userVM = null;

            try
            {
                userVM = _userAccessor.SelectUserByEmail(email);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("User Not Found", ex);
            }

            return userVM;
        }

        public string SelectRoleByUserID(int userId)
        {
            string role = "Guest";
            try
            {
                role = _userAccessor.SelectRoleByUserID(userId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Couldn't verify user role", ex);
            }

            return role;
        }

        public bool VerifyUser(string email, string passwordHash)
        {
            bool result = false;

            try
            {
                result = 1 == _userAccessor.VerifyUserWithEmailAndPasswordHash(email, passwordHash);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Verification Failed", ex);
            }

            return result;
        }

        public bool UpdateUser(UserVM userVM, string newFName, string newLName, string newEmail, string newImageURL)
        {
            bool result = false;
            int rows = 0;
            try
            {
                rows = _userAccessor.UpdateUser(userVM.userID, newFName, newLName, newEmail, newImageURL, userVM.fName, userVM.lName, userVM.email);
                if (rows == 0)
                {
                    throw new ArgumentException("Incorrect input, user update failed");
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("User update failed", ex);
            }

            return result;

        }

        public List<User> GetInactiveUsers()
        {
            List<User> users = null;
            try
            {
                users = _userAccessor.GetInactiveUsers();
                if (users == null)
                {
                    throw new ArgumentException("No inactive users found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving users", ex);
            }
            return users;
        }

        public List<User> GetActiveUsers()
        {
            List<User> users = null;
            try
            {
                users = _userAccessor.GetActiveUsers();
                if (users == null)
                {
                    throw new ArgumentException("No active users found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving users", ex);
            }
            return users;
        }

        public bool UpdateUserIsActive(int userID, bool isActive)
        {
            int rows = 0;
            bool result = false;
            try
            {
                rows = _userAccessor.UpdateUserIsActive(userID, isActive);
                if (rows != 0)
                {
                    result = true;
                }
                else
                {
                    throw new ArgumentException("Error updating user");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating user status", ex);
            }
            return result;
        }

        public bool ResetPassword(string email, string oldPassword, string newPassword)
        {
            bool result = false;

            oldPassword = HashSha256(oldPassword);
            newPassword = HashSha256(newPassword);

            try
            {
                result = (1 == _userAccessor.UpdatePasswordHash(email, oldPassword, newPassword));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Password update failed", ex);
            }


            return result;
        }

        public bool ResetPasswordAdmin(string email)
        {
            bool result = false;
            try
            {
                result = (1 == _userAccessor.ResetPasswordAdmin(email));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Password reset failed", ex);
            }
            return result;
        }
    }
}
