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
        string GetImageURLByMovieID(int movieID);
        List<string> GetAllRatings();
        int AddMovieReturnNewID(string title, int year, string rating, int runtime, bool criterion, string notes);
        int AddMovieLanguage(int id, string language);
        int AddMovieGenre(int id, string genre);
        int AddMovieImage(int id, string url);
        int AddMovieFormat(int id, string format);
    }
}
