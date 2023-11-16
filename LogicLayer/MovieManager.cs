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
                movieVM.imgName = _movieAccessor.GetImageNameByMovieID(movieID);
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

        public string GetImageNameByMovieID(int movieID)
        {
            string imageName = "";
            try
            {
                imageName = _movieAccessor.GetImageNameByMovieID(movieID);
                if(imageName == null)
                {
                    throw new ApplicationException("No image found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error locating image", ex);
            }

            return imageName;
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
                    imgName = _movieAccessor.GetImageNameByMovieID(movie.titleID),
                    languages = _movieAccessor.GetLanguagesByMovieID(movie.titleID)
                }) ;
            }

            return movieVMs;
        }
    }
}
