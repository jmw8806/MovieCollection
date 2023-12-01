using DataAccessFakes;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LogicLayerTests
{
    [TestClass]
    public class CollectionManagerTests
    {
        CollectionManager _collectionManager = null;
        [TestInitialize]
        public void TestSetup()
        {
            _collectionManager = new CollectionManager(new CollectionAccessorFake());
        }

        [TestMethod]
        public void GetAllCollectionsByUserIDCountPasses()
        {
            int expected = 2;
            int actual = 0;
            int id = 1;
            actual = _collectionManager.GetCollectionsByUserID(id).Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAllCollectionsByUserIDCountFails()
        {
            int expected = 1;
            int actual = 0;
            int id = 1;
            actual = _collectionManager.GetCollectionsByUserID(id).Count;

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void AddMovieIDtoCollectionPasses()
        {
            bool actual = false;
            int collectionID = 1;
            int movieID = 7;
            actual = _collectionManager.AddMovieToCollection(movieID, collectionID);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void AddMovieIDtoCollectionFails()
        {
            bool actual = false;
            int collectionID = 5;
            int movieID = 7;
            actual = _collectionManager.AddMovieToCollection(movieID, collectionID);

        }

        [TestMethod]
        public void AddNewCollectionByUserIDPasses()
        {
            bool actual = false;
            int userID = 2;
            actual = _collectionManager.AddUserCollection(userID, "new");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void AddNewCollectionByUserIDFailsCollectionNameExists()
        {
            bool actual = false;
            int userID = 2;
            actual = _collectionManager.AddUserCollection(userID, "Favorites");

        }

        [TestMethod]
        public void RemoveCollectionByUserIDAndCollectionIDPasses()
        {
            bool result = false;
            int userID = 1;
            int collectionID = 1;
            result = _collectionManager.RemoveUserCollection(userID, collectionID);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveCollectionByUserIDAndCollectionIDFails()
        {
            bool result = false;
            int userID = 7;
            int collectionID = 1;
            result = _collectionManager.RemoveUserCollection(userID, collectionID);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveMovieFromCollectionPasses()

        {
            bool result = false;
            int movieID = 1;
            int collectionID = 2;
            result = _collectionManager.RemoveMovieFromCollection(movieID, collectionID);
            Assert.IsTrue(result);
        }

    }
}
