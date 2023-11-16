using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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


    }
}
