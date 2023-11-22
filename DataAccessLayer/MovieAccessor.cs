using DataAccessInterfaces;
using DataObjects;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MovieAccessor : IMovieAccessor
    {
        public MovieVM SelectMovieByID(int movieID)
        {
            MovieVM movieVM = null;
            

            var conn = DBConnectionProvider.GetConnection();
            var cmd = new SqlCommand("sp_select_title_by_titleID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters["@titleID"].Value = movieID;
            try 
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.HasRows)
                {
                    
                        reader.Read();
                        movieVM = new MovieVM()
                        {
                            titleID = reader.GetInt32(0),
                            title = reader.GetString(1),
                            year = reader.GetInt32(2),
                            rating = reader.GetString(3),
                            runtime = reader.GetInt32(4),
                            isCriterion = reader.GetBoolean(5),
                            notes = reader.GetString(6),
                            isActive = reader.GetBoolean(7)
                        };
                    
                }
                else
                {
                    throw new ArgumentException("Movie not found by that ID");
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

            return movieVM;
        }

        public int count_all_titles()
        {
            int count = 0;
            var conn = DBConnectionProvider.GetConnection();
            var cmd = new SqlCommand("sp_count_titles", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public List<Movie> GetAllMovies()
        {
            List<Movie> movies = new List<Movie>();
            

            var conn = DBConnectionProvider.GetConnection();
            var cmd = new SqlCommand("sp_select_all_movies", conn);
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
                        movies.Add(new Movie()
                        {
                            titleID = reader.GetInt32(0),
                            title = reader.GetString(1),
                            year = reader.GetInt32(2),
                            rating = reader.GetString(3),
                            runtime = reader.GetInt32(4),
                            isCriterion = reader.GetBoolean(5),
                            notes = reader.GetString(6),
                            isActive = reader.GetBoolean(7)
                        });
                    }

                }
                else
                {
                    throw new ArgumentException("No movies found");
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return movies;
        }

        public List<string> GetAllGenres()
        {
            List<string> genres = new List<string>();


            var conn = DBConnectionProvider.GetConnection();
            var cmd = new SqlCommand("sp_select_all_genres", conn);
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
                        genres.Add(reader.GetString(0));
                    }

                }
                else
                {
                    throw new ArgumentException("No genres found");
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
            return genres;
        }

        public List<string> GetMovieGenresByMovieID(int movieID)
        {
            List<string> genres = new List<string>(); ;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_select_genres_by_titleID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters["@titleID"].Value = movieID;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        genres.Add(reader.GetString(0));
                    }
                }
                else
                {
                    throw new ApplicationException("Genres not found");
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

            return genres;

        }

        public List<string> GetAllLanguages()
        {
            List<string> languages = new List<string>();
            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_get_all_languages";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        languages.Add(reader.GetString(0));
                    }
                }
                else
                {
                    throw new ApplicationException("Languages not found");
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
            return languages;
        }

        public List<string> GetLanguagesByMovieID(int movieID)
        {
            List<string> languages = new List<string>(); ;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_select_languages_by_titleID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters["@titleID"].Value = movieID;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        languages.Add(reader.GetString(0));
                    }
                }
                else
                {
                    throw new ApplicationException("Languages not found");
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

            return languages;
        }

        public List<string> GetAllFormats()
        {
            List<string> formats = new List<string>();
            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_get_all_formats";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        formats.Add(reader.GetString(0));
                    }
                }
                else
                {
                    throw new ApplicationException("Languages not found");
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
            return formats;
        }

        public List<string> GetAllFormatsByMovieID(int movieID)
        {
            List<string> formats = new List<string>(); ;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_select_formats_by_titleID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters["@titleID"].Value = movieID;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        formats.Add(reader.GetString(0));
                    }
                }
                else
                {
                    throw new ApplicationException("Formats not found");
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

            return formats;
        }

        public List<string> GetAllLocations()
        {
            List<string> locations = new List<string>();
            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_get_all_locations";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        locations.Add(reader.GetString(0));
                    }
                }
                else
                {
                    throw new ApplicationException("Locations not found");
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
            return locations;
        }

        public string GetImageURLByMovieID(int movieID)
        {
            string url = "";
            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_get_url_by_titleID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters["@titleID"].Value = movieID;

            try
            {
                conn.Open();
                var result = cmd.ExecuteScalar();

                if (result == null)
                {
                    throw new ApplicationException("No file name found");
                }
                else
                {
                    url = Convert.ToString(result);
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
            return url;

        }

        public List<string> GetAllRatings()
        {
            List<string> ratings = new List<string>();
            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_get_all_ratings";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ratings.Add(reader.GetString(0));
                    }
                }
                else
                {
                    throw new ApplicationException("Languages not found");
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
            return ratings;
        }

        public int AddMovieReturnNewID(string title, int year, string rating, int runtime, bool criterion, string notes)
        {
            int newID = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_insert_title_return_ID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@title", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@release_year", SqlDbType.Int);
            cmd.Parameters.Add("@rating", SqlDbType.NVarChar, 5);
            cmd.Parameters.Add("@runtime", SqlDbType.Int);
            cmd.Parameters.Add("@criterion", SqlDbType.Bit);           
            cmd.Parameters.Add("@notes", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;

            cmd.Parameters["@title"].Value = title;
            cmd.Parameters["@release_year"].Value = year;
            cmd.Parameters["@rating"].Value = rating;
            cmd.Parameters["@runtime"].Value = runtime;
            cmd.Parameters["@criterion"].Value = criterion;
            cmd.Parameters["@notes"].Value = notes;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                newID = (int)cmd.Parameters["@new_id"].Value;
                if(newID == 0)
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

        public int AddMovieLanguage(int id, string language)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_insert_titleLanguage";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@media_language", SqlDbType.NVarChar, 100);
           

            cmd.Parameters["@titleID"].Value = id;
            cmd.Parameters["@media_language"].Value = language;
           
            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
               if(rows == 0)
                {
                    throw new ApplicationException("Language " + language + " addition failed");
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

        public int AddMovieGenre(int id, string genre)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_insert_titleGenre";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@genre", SqlDbType.NVarChar, 100);


            cmd.Parameters["@titleID"].Value = id;
            cmd.Parameters["@genre"].Value = genre;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Genre " + genre + " addition failed");
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

        public int AddMovieImage(int id, string url)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_insert_titleImage";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@url", SqlDbType.NVarChar, 100);


            cmd.Parameters["@titleID"].Value = id;
            cmd.Parameters["@url"].Value = url;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Image addition failed");
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

        public int AddMovieFormat(int id, string format)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_insert_titleFormat";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@media_format", SqlDbType.NVarChar, 100);


            cmd.Parameters["@titleID"].Value = id;
            cmd.Parameters["@media_format"].Value = format;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Format, " + format + ", addition failed");
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

        public int UpdateTitleByMovieID(int id, string newTitle, int newYear, string newRating, int newRuntime, bool newCriterion, string newNotes, string oldTitle, int oldYear, string oldRating, int oldRuntime, bool oldCriterion, string oldNotes)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_update_title_by_titleID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@newTitle", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@newRelease_year", SqlDbType.Int);
            cmd.Parameters.Add("@newRating", SqlDbType.NVarChar, 5);
            cmd.Parameters.Add("@newRuntime", SqlDbType.Int);
            cmd.Parameters.Add("@newCriterion", SqlDbType.Bit);
            cmd.Parameters.Add("@newNotes", SqlDbType.NVarChar, 250);

            cmd.Parameters.Add("@oldTitle", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@oldRelease_year", SqlDbType.Int);
            cmd.Parameters.Add("@oldRating", SqlDbType.NVarChar, 5);
            cmd.Parameters.Add("@oldRuntime", SqlDbType.Int);
            cmd.Parameters.Add("@oldCriterion", SqlDbType.Bit);
            cmd.Parameters.Add("@oldNotes", SqlDbType.NVarChar, 250);

            cmd.Parameters["@titleID"].Value = id;
            cmd.Parameters["@newTitle"].Value = newTitle;
            cmd.Parameters["@newRelease_year"].Value = newYear;
            cmd.Parameters["@newRating"].Value = newRating;
            cmd.Parameters["@newRuntime"].Value = newRuntime;
            cmd.Parameters["@newCriterion"].Value = newCriterion;
            cmd.Parameters["@newNotes"].Value = newNotes;

            cmd.Parameters["@oldTitle"].Value = oldTitle;
            cmd.Parameters["@oldRelease_year"].Value = oldYear;
            cmd.Parameters["@oldRating"].Value = oldRating;
            cmd.Parameters["@oldRuntime"].Value = oldRuntime;
            cmd.Parameters["@oldCriterion"].Value = oldCriterion;
            cmd.Parameters["@oldNotes"].Value = oldNotes;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Title update failed");
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

        public int RemoveMovieGenre(int id)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_remove_title_genres";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);

            cmd.Parameters["@titleID"].Value = id;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Title genre removal failed");
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

        public int RemoveMovieLanguage(int id)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_remove_title_language";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);

            cmd.Parameters["@titleID"].Value = id;
            

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Title language removal failed");
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

        public int UpdateMovieImageURL(int id, string newURL, string oldURL)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_update_TitleImage";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@newURL", SqlDbType.NVarChar, 255);
            cmd.Parameters.Add("@oldURL", SqlDbType.NVarChar, 255);
            
            cmd.Parameters["@titleID"].Value = id;
            cmd.Parameters["@newURL"].Value = newURL;
            cmd.Parameters["@oldURL"].Value = oldURL;


            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Title image url update failed");
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

        public int UpdateMovieIsActive(int id, bool isActive)
        {
            int rows = 0;

            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_update_title_isActive";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@titleID", SqlDbType.Int);
            cmd.Parameters.Add("@isActive", SqlDbType.Bit);
            
            cmd.Parameters["@titleID"].Value = id;
            cmd.Parameters["@isActive"].Value = isActive;
            
            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Title isActive update failed");
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
