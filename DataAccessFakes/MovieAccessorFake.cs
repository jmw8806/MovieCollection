using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class MovieAccessorFake : IMovieAccessor
    {
        private List<MovieVM> fakeMovies = new List<MovieVM>();
        private List<Movie> fakeMovie = new List<Movie>();
        private List<string> genres = new List<string>();
        private List<string> languages = new List<string>();
        private List<string> formats = new List<string>();
        private List<string> locations = new List<string>();
        private List<string> ratings = new List<string>();
        public MovieAccessorFake() {
            //MovieVM
            fakeMovies.Add(new MovieVM()
            {
                titleID = 1,
                title = "First",
                year = 2001,
                rating = "G",
                isCriterion = false,
                notes = "",
                isActive = true,
                genres = new List<string>(),
                formats = new List<string>(),
                imgName = "one.png",
                languages = new List<string>(),
                

            }) ;
            fakeMovies.Add(new MovieVM()
            {
                titleID = 2,
                title = "Second",
                year = 2002,
                rating = "PG",
                isCriterion = true,
                notes = "",
                isActive = true,
                genres = new List<string>(),
                formats = new List<string>(),
                imgName = "two.png",
                languages = new List<string>()
            });
            fakeMovies.Add(new MovieVM()
            {
                titleID = 3,
                title = "Third",
                year = 2003,
                rating = "PG-13",
                isCriterion = false,
                notes = "This is the third",
                isActive = true,
                genres = new List<string>(),
                formats = new List<string>(),
                imgName = "three.png",
                languages = new List<string>()
            });

            //Movie
            fakeMovie.Add(new Movie()
            {
                titleID = 1,
                title = "First",
                year = 2001,
                rating = "G",
                isCriterion = false,
                notes = "",
                isActive = true,

            });
            fakeMovie.Add(new Movie()
            {
                titleID = 2,
                title = "Second",
                year = 2002,
                rating = "PG",
                isCriterion = true,
                notes = "",
                isActive = true,

            });
            fakeMovie.Add(new Movie()
            {
                titleID = 3,
                title = "Third",
                year = 2003,
                rating = "PG-13",
                isCriterion = false,
                notes = "This is the third",
                isActive = true,
            });

          

            fakeMovies[0].genres.Add("Horror");
            fakeMovies[0].genres.Add("Drama");
            fakeMovies[1].genres.Add("Comedy");
            fakeMovies[2].genres.Add("Romance");

            fakeMovies[0].formats.Add("DVD");
            fakeMovies[0].formats.Add("Blu-Ray");
            fakeMovies[1].formats.Add("4K");
            fakeMovies[2].formats.Add("VHS");

            fakeMovies[0].languages.Add("English");
            fakeMovies[1].languages.Add("French");
            fakeMovies[2].languages.Add("Spanish");

            formats.Add("DVD");
            formats.Add("Blu-Ray");
            formats.Add("4K");
            formats.Add("VHS");



            ratings.Add("G");
            ratings.Add("PG-13");
            ratings.Add("R");

            genres.Add("Horror");
            genres.Add("Comedy");
            genres.Add("Romance");

            languages.Add("English");
            languages.Add("Spanish");
            languages.Add("French");
        }



        public MovieVM SelectMovieByID(int movieID)
        {
            MovieVM movieVM = null;

            foreach (var movie in fakeMovies) 
            { 
                if(movie.titleID == movieID)
                {
                    movieVM = movie;
                    break;
                }
            }
            if(movieVM == null)
            {
                throw new ArgumentException("Movie ID not found");
            }
            return movieVM;
        }

        public int count_all_titles()
        {
            return fakeMovies.Count;
        }

        public List<Movie> GetAllMovies() 
        {
            return fakeMovie;
        }

        public List<string> GetAllGenres()
        {
            return genres;
        }

        public List<string> GetMovieGenresByMovieID(int movieID)
        {
            List<string> genres = null;
            foreach(var movie in fakeMovies)
            {
                if(movie.titleID == movieID)
                {
                    genres = movie.genres;
                    break;
                }
                else
                {
                    throw new ArgumentException("Movie not found");
                }
            }
            if(genres == null)
            {
                throw new ArgumentException("No genres found");
            }
            return genres;
        
        }

        public List<string> GetAllLanguages()
        {
            return languages;
        }

        public List<string> GetLanguagesByMovieID(int movieID)
        {
            List<string> langauges = null;
            foreach (var movie in fakeMovies)
            {
                if (movie.titleID == movieID)
                {
                    languages = movie.languages;
                    break;
                }
                else
                {
                    throw new ArgumentException("Movie not found");
                }
            }
            if (languages == null)
            {
                throw new ArgumentException("No genres found");
            }
            return languages;
        }

        public List<string> GetAllFormats()
        {
            return formats;
        }

        public List<string> GetAllFormatsByMovieID(int movieID)
        {
            List<string> formats = null;
            foreach (var movie in fakeMovies)
            {
                if (movie.titleID == movieID)
                {
                    formats = movie.formats;
                    break;
                }
                else
                {
                    throw new ArgumentException("Movie not found");
                }
            }
            if (languages == null)
            {
                throw new ArgumentException("No formats found");
            }
            return formats;
        }

        public List<string> GetAllLocations()
        {
            throw new NotImplementedException();
        }

        public string GetImageURLByMovieID(int movieID)
        {
            string imageName = "";
            foreach(var movie in fakeMovies)
            {
                if(movie.titleID == movieID)
                {
                    imageName = movie.imgName;
                    break;
                }
                else
                {
                    throw new ApplicationException("No movie by that ID found");
                }
            }
            if(imageName == "")
            {
                throw new ApplicationException("No image found");
            }
            return imageName;
        }

        public List<string> GetAllRatings()
        {
            return ratings;
        }

        public int AddMovieReturnNewID(string title, int year, string rating, int runtime, bool criterion, string notes)
        {
            throw new NotImplementedException();
        }

        public int AddMovieLanguage(int id, string language)
        {
            throw new NotImplementedException();
        }

        public int AddMovieGenre(int id, string genre)
        {
            throw new NotImplementedException();
        }

        public int AddMovieImage(int id, string fileName)
        {
            throw new NotImplementedException();
        }

        public int AddMovieFormat(int id, string format)
        {
            throw new NotImplementedException();
        }
    }
}
