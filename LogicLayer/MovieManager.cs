using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class MovieManager : IMovieManager
    {
        IMovieAccessor _movieAccessor = null;

        public MovieManager()
        {
            _movieAccessor = new MovieAccessor();
        }

        public MovieManager(IMovieAccessor movieAccessor)
        {
            _movieAccessor = movieAccessor;
        }

        public MovieVM GetMovieByTitleID(int movieID)
        {
            MovieVM movieVM = null;
            try
            {
                movieVM = _movieAccessor.SelectMovieByID(movieID);
                movieVM.genres = _movieAccessor.GetMovieGenresByMovieID(movieID);
                movieVM.languages = _movieAccessor.GetLanguagesByMovieID(movieID);
                movieVM.formats = _movieAccessor.GetAllFormatsByMovieID(movieID);
                movieVM.imgName = _movieAccessor.GetImageURLByMovieID(movieID);
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("Movie not found", ex);
            }
         
            return movieVM;
        }

        public int count_all_titles()
        {
            int count = 0;
            count = _movieAccessor.count_all_titles();
            return count;
        }

        public List<Movie> GetAllMovies()
        {
            List<Movie> movies = null;
            try
            {
                movies = _movieAccessor.GetAllMovies();
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("No movies found", ex);
            }
            return movies;
        }

        public List<string> GetAllGenres()
        {
            List<string> genres = null;
            try
            {
                genres = _movieAccessor.GetAllGenres();
            }
            catch(Exception ex)
            {
                throw new ApplicationException("No genres found", ex);
            }
            return genres;
        }

        public List<string> GetGenresByTitleID(int titleID)
        {
            List<string> genres = null;
            try
            {
                genres = _movieAccessor.GetMovieGenresByMovieID(titleID);
                if(genres == null)
                {
                    throw new ApplicationException("No genres found by that id");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error accessing genres by that id", ex);
            }
            
            return genres;
        }

        public List<string> GetAllLanguages()
        {
            List<string> languages = null;
            try
            {
                languages = _movieAccessor.GetAllLanguages();
                if (languages == null)
                {
                    throw new ApplicationException("No languages found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error accessing languages", ex);
            }
            return languages;
        }

        public List<string> GetLanguagesByMovieID(int movieID)
        {
            List<string> languages = null;
            try
            {
                languages = _movieAccessor.GetLanguagesByMovieID(movieID);
                if(languages == null)
                {
                    throw new ApplicationException("No languages found");
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error accessing languages by that movieID");
            }
            return languages;
        }

        public List<string> GetAllFormats()
        {
            List<string> formats = null;
            try
            {
                formats= _movieAccessor.GetAllFormats();
                if (formats == null)
                {
                    throw new ApplicationException("No formats found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error accessing formats", ex);
            }
            return formats;
        }

        public List<string> GetFormatsByMovieID(int movieID)
        {
            List<string> formats = null;

            try
            {
                formats = _movieAccessor.GetAllFormatsByMovieID(movieID);
                if(formats == null)
                {
                    throw new ApplicationException("No formats found by that movieID");
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error finding formats", ex);
            }

            return formats;
        }

        public List<string> GetAllLocations()
        {
            List<string> locations = null;
            try
            {
                locations = _movieAccessor.GetAllLocations();
                if(locations == null)
                {
                    throw new ApplicationException("No locations found");
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error finding locations", ex);
            }

            return locations;
        }

        public string GetImageURLByMovieID(int movieID)
        {
            string url = "";
            try
            {
                url = _movieAccessor.GetImageURLByMovieID(movieID);
                if(url == null || url == "")
                {
                    url = "https://thumbs.dreamstime.com/b/movie-film-poster-design-template-background-vintage-reel-sunray-element-can-be-used-backdrop-banner-brochure-leaflet-197521645.jpg";
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error locating image", ex);
            }

            return url;
        }

        public List<MovieVM> GetAllMovieVMs()
        {
            List<Movie> movies = _movieAccessor.GetAllMovies();
            List<MovieVM> movieVMs = new List<MovieVM>();

            foreach(Movie movie in movies)
            {
                movieVMs.Add(new MovieVM
                {
                    titleID = movie.titleID,
                    title = movie.title,
                    year = movie.year,
                    rating = movie.rating,
                    runtime = movie.runtime,
                    isCriterion = movie.isCriterion,
                    notes = movie.notes,
                    genres = _movieAccessor.GetMovieGenresByMovieID(movie.titleID),
                    formats = _movieAccessor.GetAllFormatsByMovieID(movie.titleID),
                    imgName = _movieAccessor.GetImageURLByMovieID(movie.titleID),
                    languages = _movieAccessor.GetLanguagesByMovieID(movie.titleID)
                }) ;
            }

            return movieVMs;
        }

        public List<string> GetAllRatings()
        {
            List<string> ratings = null;
            try
            {
                ratings = _movieAccessor.GetAllRatings();
                if (ratings == null)
                {
                    throw new ApplicationException("No locations found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error finding locations", ex);
            }

            return ratings;

        }

        public bool AddMovie(string title, int year, string rating, int runtime,
            bool isCriterion, string notes, string language, string genre, string fileName, string format)
        {
            bool success = false;
            int addMovieGetID = 0;
            // sp_insert_title
            try
            {
                addMovieGetID = _movieAccessor.AddMovieReturnNewID(title, year, rating, runtime, isCriterion, notes);
                if (addMovieGetID == 0)
                {
                    throw new ApplicationException("Add movie failed");
                }

                // stored procedures
                int addMovieLanguage = _movieAccessor.AddMovieLanguage(addMovieGetID, language);
                int addMovieGenre = _movieAccessor.AddMovieGenre(addMovieGetID, genre);
                int addMovieImage = _movieAccessor.AddMovieImage(addMovieGetID, fileName);
                int addMovieFormat = _movieAccessor.AddMovieFormat(addMovieGetID, format);
                
                // Check that all stored procedures executed successfully
                if (addMovieLanguage != 1 && addMovieGenre != 1 && addMovieImage != 1 && addMovieFormat != 1)
                {
                    throw new ApplicationException("Failed to add movie");
                }
                else
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }


    }
}
