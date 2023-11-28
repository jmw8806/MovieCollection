using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class UserAccessorFake : IUserAccessor
    {
        private List<UserVM> fakeUsers = new List<UserVM>();
        private List<string> passwordHashes = new List<string>();
        private List<User> fakeUser = new List<User>();

        public UserAccessorFake() {

            // UserVM data
            fakeUsers.Add(new UserVM()
            {
                userID = 10003,
                fName = "Jane",
                lName = "Smith",
                email = "jane@gmail.com",
                isActive = true,
                roles = "Administrator"

            });
            fakeUsers.Add(new UserVM()
            {
                userID = 10001,
                fName = "Joe",
                lName = "Dude",
                email = "joeDude@mmail.com",
                isActive = true,
                roles = "User"

            });


            //User data
            fakeUser.Add(new User()
            {
                userID = 10003,
                fName = "Jane",
                lName = "Smith",
                email = "jane@gmail.com",
                isActive = true
                

            });
            fakeUser.Add(new User()
            {
                userID = 10001,
                fName = "Joe",
                lName = "Dude",
                email = "joeDude@mmail.com",
                isActive = false
               
            });

            passwordHashes.Add("5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8");
            passwordHashes.Add("bad hash");

            

        }

        public UserVM SelectUserByEmail(string email)
        {
            UserVM userVM = null;
            foreach(var user in fakeUsers)
            {
                if(user.email == email)
                {
                    userVM = user;
                    break;
                }
            }
            if(userVM == null)
            {
                throw new ArgumentException("Email address not found");
            }
            return userVM;
        }

        public int VerifyUserWithEmailAndPasswordHash(string email, string passwordHash)
        {
            int rows = 0;

            for (int i = 0; i < fakeUsers.Count; i++)
            {
                if (fakeUsers[i].email == email)
                {
                    if (passwordHashes[i] == passwordHash && fakeUsers[i].isActive)
                    {
                        rows += 1;
                        continue;
                    }
                }
            }

            return rows;
        }

        public string SelectRoleByUserID(int userId) 
        {
            string role = "";
            foreach(var user in fakeUsers)
            {
                if (user.userID == userId) 
                {
                    role = user.roles;
                }
                else 
                { 
                    throw new ArgumentException("User not found"); 
                }
            }
            return role;
           
        }

        public int UpdateUser(int userID, string newFName, string newLName, string newEmail, string newImgURL, string oldFName, string oldLName, string oldEmail)
        {
            throw new NotImplementedException();
        }

        public List<User> GetInactiveUsers()
        {
            List<User> users = new List<User>();

            foreach (var user in fakeUser) 
            { 
                if(user.isActive == false)
                {
                    users.Add(user);
                }
            }
            if(users == null)
            {
                throw new ArgumentException("No inactive users found");
            }
            return users;

        }
    }
}
