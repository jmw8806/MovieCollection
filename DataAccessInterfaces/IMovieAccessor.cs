using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IMovieAccessor
    {
        MovieVM SelectMovieByID(int movieID);
        int count_all_titles();
        List<Movie> GetAllMovies();
        List<string> GetAllGenres();
        List<string> GetMovieGenresByMovieID(int movieID);
        List<string> GetAllLanguages();
        List<string> GetLanguagesByMovieID(int movieID);
        List<string> GetAllFormats();
        List<string> GetAllFormatsByMovieID(int movieID);
        List<string> GetAllLocations();
        string GetImageNameByMovieID(int movieID);
    }
}
