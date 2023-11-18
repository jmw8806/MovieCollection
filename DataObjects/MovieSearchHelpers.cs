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
            results = movies.Where(m => m.title.Contains(term)).ToList();
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
    }
}
