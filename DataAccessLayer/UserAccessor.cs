using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public UserVM SelectUserByEmail(string email)
        {
            UserVM userVM = null;

            var conn = DBConnectionProvider.GetConnection();

            var cmdText = "sp_view_user_by_email";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 250);

            cmd.Parameters["@email"].Value = email;

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.HasRows)
                {
                    reader.Read();

                    userVM = new UserVM()
                    {
                        userID = reader.GetInt32(0),
                        fName = reader.GetString(1),
                        lName = reader.GetString(2),
                        email = reader.GetString(3),
                        passwordHash = reader.GetString(4),
                        isActive = reader.GetBoolean(5),
                        imgURL = reader.GetString(6)
                    };
                }
                else
                {
                    throw new ArgumentException("Email address not found");
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException("Email not found", ex);
            }

            return userVM;
        }

        public int VerifyUserWithEmailAndPasswordHash(string email, string passwordHash)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();

            var cmdText = "sp_verify_user";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@passwordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters["@email"].Value = email;
            cmd.Parameters["@passwordHash"].Value = passwordHash;
            
            try
            {
                conn.Open();
                rows = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                conn.Close();
            }

            return rows;

        }

        public string SelectRoleByUserID(int userID)
        {
            string role = "";
            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_get_role_by_userID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int);
            cmd.Parameters["@userID"].Value = userID;

            try
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                
                if(result == null)
                {
                    throw new ApplicationException("No roles found");
                }
                else
                {
                    role = Convert.ToString(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            { 
                conn.Close(); 
            }
            return role;
        }

        public int UpdateUser(int userID, string newFName, string newLName, string newEmail, string newImgURL, string oldFName, string oldLName, string oldEmail)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();

            var cmdText = "sp_update_user";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@userID", SqlDbType.Int);
            cmd.Parameters.Add("@newFName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@newLName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@newEmail", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@newImgURL", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@oldFName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@oldLName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@oldEmail", SqlDbType.NVarChar, 250);

            cmd.Parameters["@userID"].Value = userID;          
            cmd.Parameters["@newFName"].Value = newFName;
            cmd.Parameters["@newLName"].Value = newLName;
            cmd.Parameters["@newEmail"].Value = newEmail;
            cmd.Parameters["@newImgURL"].Value = newImgURL;
            cmd.Parameters["@oldFName"].Value = oldFName;
            cmd.Parameters["@oldLName"].Value = oldLName;
            cmd.Parameters["@oldEmail"].Value = oldEmail;


            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("User update failed");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public List<User> GetInactiveUsers()
        {
            List<User> users = new List<User>();


            var conn = DBConnectionProvider.GetConnection();
            var cmd = new SqlCommand("sp_get_inactive_users", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    reader.Read();
                    while (reader.Read())
                    {
                        users.Add(new User()
                        {
                            userID = reader.GetInt32(0),
                            fName = reader.GetString(1),
                            lName = reader.GetString(2),
                            email = reader.GetString(3),
                            imgURL = reader.GetString(4)
                        }); 
                    }

                }
                else
                {
                    throw new ArgumentException("No inactive users found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return users;
        }
    }
}
