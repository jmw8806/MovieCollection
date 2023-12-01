using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class CollectionAccessor : ICollectionAccessor
    {
        public int AddMovieToCollection(int movieID, int collectionID)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_insert_movieID_into_UserCollectionLineItems";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@collectionID", SqlDbType.Int);


            cmd.Parameters["@titleID"].Value = movieID;
            cmd.Parameters["@collectionID"].Value = collectionID;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Collecition addition failed");
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

        public int AddNewCollection(int userID, string collectionName)
        {
            int newID = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_insert_new_collection";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int);
            cmd.Parameters.Add("@collectionName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;

            cmd.Parameters["@userID"].Value = userID;
            cmd.Parameters["@collectionName"].Value = collectionName;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                newID = (int)cmd.Parameters["@new_id"].Value;
                if (newID == 0)
                {
                    throw new ApplicationException("ID retrieval failed");
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
            return newID;
        }

        public List<CollectionVM> GetCollectionsByUserID(int userID)
        {
            List<CollectionVM> collections = new List<CollectionVM>();


            var conn = DBConnectionProvider.GetConnection();
            var cmd = new SqlCommand("sp_get_collections_by_userID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int);
            cmd.Parameters["@userID"].Value = userID;
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        collections.Add(new CollectionVM()
                        {
                            collectionID = reader.GetInt32(0),
                            collectionName = reader.GetString(1),
                            userID = reader.GetInt32(2)
                        });
                    }

                }
                else
                {
                    throw new ArgumentException("No collections found");
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
            return collections;
        }

        public List<int> GetMovieIDsByCollectionID(int collectionID)
        {
            List<int> movieIDs = new List<int>();


            var conn = DBConnectionProvider.GetConnection();
            var cmd = new SqlCommand("sp_get_titleIDs_by_collectionID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@collectionID", SqlDbType.Int);
            cmd.Parameters["@collectionID"].Value = collectionID;
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        movieIDs.Add(reader.GetInt32(0));
                    }

                }
                else
                {
                    throw new ArgumentException("No collections found");
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
            return movieIDs;
        }

        public int RemoveCollection(int userID, int collectionID)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_remove_collection_by_userID_and_collection";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int);
            cmd.Parameters.Add("@collectionID", SqlDbType.Int);


            cmd.Parameters["@userID"].Value = userID;
            cmd.Parameters["@collectionID"].Value = collectionID;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Collecition removal failed");
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

        public int RemoveMovieFromCollection(int movieID, int collectionID)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_remove_title_from_UserCollectionLineItems";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@collectionID", SqlDbType.Int);


            cmd.Parameters["@titleID"].Value = movieID;
            cmd.Parameters["@collectionID"].Value = collectionID;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Removing movie from collection failed");
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
    }
}
