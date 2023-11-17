using DataAccessFakes;
using DataObjects;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class MovieManagerTests
    {

        MovieManager _movieManager = null;
        [TestInitialize]
        public void TestSetup()
        {
            _movieManager = new MovieManager(new MovieAccessorFake());
        }

        [TestMethod]
        public void TestSelectMovieByIDSuccess()
        {
            int movieID = 1;
            string expectedTitle = "First";
            string actualTitle = _movieManager.GetMovieByTitleID(movieID).title;

            Assert.AreEqual(expectedTitle, actualTitle);

        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestSelectMovieByIDFail()
        {
            int movieID = 2;
            string expectedTitle = "First";
            string actualTitle = _movieManager.GetMovieByTitleID(movieID).title;
            
        }
        [TestMethod]
        public void TestCountMoviesPass()
        {
            int count = 0;
            int expectedCount = 3;
            int actualCount = _movieManager.count_all_titles();
            Assert.AreEqual(expectedCount, actualCount);
        }
        [TestMethod]
        public void TestCountMoviesFail()
        {
            int count = 0;
            int wrongCount = 5;
            int actualCount = _movieManager.count_all_titles();
            Assert.AreNotEqual(wrongCount, actualCount);
        }
        [TestMethod]
        public void TestGetAllMoviesPass()
        {
            int actualCount = 0;
            int expectedCount = 3;
            actualCount = _movieManager.GetAllMovies().Count;
            Assert.AreEqual(expectedCount, actualCount);
        }
        [TestMethod]
        public void TestGetAllMoviesFail()
        {
            int actualCount = 0;
            int expectedCount = 5;
            actualCount = _movieManager.GetAllMovies().Count;
            Assert.AreNotEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void TestGetAllGenresPass()
        {
            int actualCount = 0;
            int expectedCount = 3;
            actualCount = _movieManager.GetAllGenres().Count;
            Assert.AreEqual(actualCount, expectedCount);
        }
        [TestMethod]
        public void TestGetAllGenresFail()
        {
            int actualCount = 0;
            int expectedCount = 4;
            actualCount = _movieManager.GetAllGenres().Count;
            Assert.AreNotEqual(actualCount, expectedCount);
        }

        [TestMethod]
        public void TestGetGenresByMovieIDPass()
        {
            int movieID = 1;
            string expectedResult = "HorrorDrama";

            string actualResult = "";
            List<string> genres = _movieManager.GetGenresByTitleID(movieID);
            foreach (string genre in genres)
            {
                actualResult += genre;
            }

            Assert.AreEqual(expectedResult, actualResult);

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestGetGenresByMovieIDNoMovieIDFoundFails()
        {
            int movieID = 12;
            List<string> result = _movieManager.GetGenresByTitleID(movieID);
           
        }

        [TestMethod]
        
        public void TestGetGenresByMovieIDIncorrectResultsFails()
        {
            int movieID = 1;
            string expectedResult = "Horror";

            string actualResult = "";
            List<string> genres = _movieManager.GetGenresByTitleID(movieID);
            foreach (string genre in genres)
            {
                actualResult += genre;
            }

            Assert.AreNotEqual(expectedResult, actualResult);

        }

        [TestMethod]
        public void TestGetAllLanguagesPasses()
        {
            string expectedResult = "EnglishSpanishFrench";
            List<string> languages = _movieManager.GetAllLanguages();
            string actualResult = "";
            foreach(string language in languages)
            {
                actualResult += language;
            }
            Assert.AreEqual(expectedResult, actualResult); 
        }

        [TestMethod]
        public void TestGetLanguagesByMovieIDPasses()
        {
            int movieID = 1;
            string expectedResult = "English";
            string actualResult = "";
            foreach(string language in _movieManager.GetLanguagesByMovieID(movieID))
            {
                actualResult += language;
            }

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestGetLanguagesByMovieIDFailsThrowsException()
        {
            int movieID = 12;
            List<string> result = _movieManager.GetLanguagesByMovieID(movieID);
        }

        [TestMethod]
        public void TestGetAllFormatsCountIsTrue()
        {
            int count = _movieManager.GetAllFormats().Count;
            Assert.IsTrue(count == 4);
        }

        [TestMethod]
        
        public void TestGetAllFormatsFailsCountIsFalse()
        {
            int count = _movieManager.GetAllFormats().Count;
            Assert.IsFalse(count == 2);
        }

        [TestMethod]
        public void TestGetFormatsByMovieIDCountPasses()
        {
            int movieID = 2;
            string expectedResult = "4k";
            string actualResult = "";

            List<string> results = _movieManager.GetFormatsByMovieID(movieID);
            foreach(string format in results)
            {
                actualResult += format;
            }
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestGetAllRatingsCountPasses()
        {
            int actualCount = _movieManager.GetAllRatings().Count;
            Assert.IsTrue (actualCount == 3);
        }

        [TestMethod]
        public void TestGetAllRatingsCountFails()
        {
            int actualCount = _movieManager.GetAllRatings().Count;
            Assert.IsFalse(actualCount == 4);
        }

    }

}
