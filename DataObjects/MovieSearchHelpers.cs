using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public static class MovieSearchHelpers
    {
        

       

        public static List<MovieVM> searchByTitle(List<MovieVM> movies, string term) 
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.title.ToLower().Contains(term.ToLower())).ToList();
            return results;
        }

        public static List<MovieVM> searchByGenre(List<MovieVM> movies, string genre)
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.genres.Contains(genre)).ToList();
            return results;
        }

        public static List<MovieVM> searchByYear(List<MovieVM> movies, int year)
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.year == year).ToList();
            return results;
        }
        
        public static List<MovieVM> searchByLessThanRuntime(List<MovieVM> movies, int runtime)
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.runtime <= runtime).ToList();
            return results;
        }

        public static List<MovieVM> searchByGreaterThanRuntime(List<MovieVM> movies, int runtime)
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.runtime >= runtime).ToList();
            return results;
        }

        public static List<MovieVM> searchByRating(List<MovieVM> movies, string rating)
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.rating.ToLower() ==rating.ToLower()).ToList();
            return results;
        }

        public static List<MovieVM> searchByFormat(List<MovieVM> movies, string format)
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.formats.Contains(format)).ToList();
            return results;
        }

        public static List<MovieVM> searchByLanguage(List<MovieVM> movies, string language)
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.languages.Contains(language)).ToList();
            return results;
        }

        public static List<MovieVM> searchByCriterion(List<MovieVM> movies, bool isCriterion)
        {
            List<MovieVM> results = new List<MovieVM>();
            results = movies.Where(m => m.isCriterion == isCriterion).ToList();
            return results;
        }

        public static List<int> getMovieYears(List<MovieVM> movies) 
        {
            List<int> years = new List<int>();
            foreach(var movie in  movies) 
            { 
                if(!years.Contains(movie.year))
                {
                    years.Add(movie.year);
                }
            }
            years.Sort();
            years.Reverse();
            return years;
        }

        public static List<string> getCurrentMovieGenres(List<MovieVM> movies)
        {
            List<string> genres = new List<string>();
            foreach (var movie in movies)
            {
                foreach (string genre in movie.genres)
                {
                    if (!genres.Contains(genre))
                    {
                        genres.Add(genre);
                    }
                }
            }
            genres.Sort(); 
            return genres;
        }

        public static List<string> getCurrentMovieLanguages(List<MovieVM> movies)
        {
            List<string> languages = new List<string>();
            foreach (var movie in movies)
            {
                foreach (string language in movie.languages)
                {
                    if (!languages.Contains(language))
                    {
                        languages.Add(language);
                    }
                }
            }
            languages.Sort();
            return languages;
        }


    }
}
