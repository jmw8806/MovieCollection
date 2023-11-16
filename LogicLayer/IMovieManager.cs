using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        string GetImageNameByMovieID(int movieID);
        List<MovieVM> GetAllMovieVMs();
    }
}
