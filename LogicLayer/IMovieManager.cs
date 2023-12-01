using DataObjects;
using System.Collections.Generic;

namespace LogicLayer
{
    internal interface IMovieManager
    {
        MovieVM GetMovieByTitleID(int titleID);
        List<Movie> GetAllMovies();
        List<string> GetAllGenres();
        List<string> GetGenresByTitleID(int titleID);
        List<string> GetAllLanguages();
        List<string> GetLanguagesByMovieID(int movieID);
        List<string> GetAllFormats();
        List<string> GetFormatsByMovieID(int movieID);
        List<string> GetAllLocations();
        string GetImageURLByMovieID(int movieID);
        List<MovieVM> GetAllMovieVMs();
        List<string> GetAllRatings();
        int AddMovieGetNewID(string title, int year, string rating, int runtime, bool isCriterion, string notes);
        int AddLanguage(int movieID, string language);
        int AddGenre(int movieID, string genre);
        int AddMovieImage(int movieID, string imageURL);
        int AddMovieFormat(int movieID, string format);
        int RemoveMovieGenre(int movieID);
        int RemoveMovieLanguage(int movieID);
        int UpdateMovieImage(int movieID, string newURL, string oldURL);

        bool AddMovie(string title, int year, string rating, int runtime, bool isCriterion, string notes, string language, string genre, string fileName, string format);
        bool UpdateMovie(MovieVM movie, string newTitle, int newYear, string newRating, int newRuntime, bool newCriterion, string newNotes,
            List<string> newLanguages, List<string> newGenres, string newURL);
        bool UpdateMovieIsActive(int id, bool isActive);
        List<Movie> GetAllInactiveMovies();
    }
}
