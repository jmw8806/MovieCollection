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
            int expectedResult = 1;
            int actualResult = 0;  

            actualResult = _movieManager.GetFormatsByMovieID(movieID).Count;
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

        [TestMethod]
        public void TestAddNewMovieReturnNewMovieIDPasses()
        {
            int expected = 4;
            int actual = 0;
            actual = _movieManager.AddMovieGetNewID("test", 1999, "G", 195, true, "");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddNewMovieReturnNewMovieIDFails()
        {
            int expected = 3;
            int actual = 0;
            actual = _movieManager.AddMovieGetNewID("test2", 2002, "R", 165, false, "");
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddNewMovieLanguagePasses()
        {
            int expected = 1;
            int actual = 0;
            actual = _movieManager.AddLanguage(1, "Spanish");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddNewMovieLanguageFailsThrowsException()
        {          
            int actual = 0;
            actual = _movieManager.AddLanguage(7, "Spanish"); 
        }


        [TestMethod]
        public void TestAddNewMovieGenrePasses()
        {
            int expected = 1;
            int actual = 0;
            actual = _movieManager.AddGenre(1, "Horror");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddNewMovieGenreFailsThrowsException()
        {
            int actual = 0;
            actual = _movieManager.AddGenre(7, "Christmas");
        }

        [TestMethod]
        public void TestAddNewMovieImagePasses()
        {
            int expected = 1;
            int actual = 0;
            actual = _movieManager.AddMovieImage(1, "www.picture.com");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddNewMovieImageFailsThrowsException()
        {
            int actual = 0;
            actual = _movieManager.AddMovieImage(7, "www.picture.com");
        }

        [TestMethod]
        public void TestAddNewMovieFormatPasses()
        {
            int expected = 1;
            int actual = 0;
            actual = _movieManager.AddMovieFormat(1, "Digital");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddNewMovieFormatFailsThrowsException()
        {
            int actual = 0;
            actual = _movieManager.AddMovieImage(7, "4K");
        }

        [TestMethod]
        public void TestRemoveMovieGenrePasses()
        {
            int expected = 0;
            int actual = 0;
           
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRemoveMovieGenreFailsThrowsException()
        {
            int actual = 0;
            actual = _movieManager.RemoveMovieGenre(7);
        }

        [TestMethod]
        public void TestRemoveMovieLanguagePasses()
        {
            int expected = 0;
            int actual = 0;
           
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRemoveMovieLanguageFailsThrowsException()
        {
            int actual = 0;
            actual = _movieManager.RemoveMovieGenre(7);
        }

        [TestMethod]
        public void TestUpdateMovieImagePasses()
        {
            int expected = 1;
            int actual = 0;
            int id = 1;
            string oldURL = "one.png";
            string newURL = "oneOne.png";
            actual = _movieManager.UpdateMovieImage(id, newURL, oldURL);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUpdateMovieImageFailsThrowsException()
        {
            int expected = 1;
            int actual = 0;
            int id = 1;
            string oldURL = "one1111.png";
            string newURL = "oneOne.png";
            actual = _movieManager.UpdateMovieImage(id, newURL, oldURL);
            
        }

        [TestMethod]
        public void TestUpdateMovieIsActivePasses()
        {
           
            bool actual = false;
            int id = 1;
            bool isActive = false;
            actual = _movieManager.UpdateMovieIsActive(id, isActive);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUpdateMovieIsActiveFailsThrowsException()
        {
            bool actual = false;
            int id = 5;
            bool isActive = false;
            actual = _movieManager.UpdateMovieIsActive(id, isActive);
            
        }

    }

}
