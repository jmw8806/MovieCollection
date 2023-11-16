using DataAccessInterfaces;
using DataObjects;
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

        public string GetImageNameByMovieID(int movieID)
        {
            string imgName = "";
            var conn = DBConnectionProvider.GetConnection();
            var cmdText = "sp_get_fileName_by_titleID";
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
                    imgName = Convert.ToString(result);
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
            return imgName;

        }
    }

}
