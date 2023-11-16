using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LogicLayer;
using DataAccessFakes;
using DataObjects;


namespace LogicLayerTests
{
    [TestClass]
    
    public class UserManagerTests
    {
        UserManager _userManager = null;


        [TestInitialize]
        public void TestSetup()
        {
            _userManager = new UserManager(new UserAccessorFake());

        }


        [TestMethod]
        public void TestHashSha256ReturnsAValidHash()
        {
         
            //arrange
            string testString = "password";
            string expectedResult = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";
            string actualResult = null;

            //Act
            actualResult = _userManager.HashSha256(testString);
            

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestGetUserByEmailReturnsCorrectUser() 
        {
            
            string email = "jane@gmail.com";
            int expectedID = 10003;

            int actualID = _userManager.SelectUserByEmail(email).userID;

            Assert.AreEqual(expectedID, actualID);  
        }

        

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestGetUserByEmailFailsForBadEmailWithApplicationException()
        {
            UserVM userVM = null;
            string email = "cane@gmail.com";
            int expectedID = 10003;

            int actualID = _userManager.SelectUserByEmail(email).userID;
        }

        [TestMethod]
        public void TestGetRoleByUserIDReturnsCorrectRole()
        {
            int testID = 10001;
            string expectedRole = "User";
            string actualRole = "";

            actualRole = _userManager.SelectRoleByUserID(testID);

            Assert.AreEqual(expectedRole, actualRole);
        }

        [TestMethod]
        public void TestVerifyUserCredentialsReturnsTrue()
        {
            string email = "jane@gmail.com";
            string password = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";
            bool expectedResult = true;
            bool actualResult = false;

            actualResult = _userManager.VerifyUser(email, password);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestVerifyUserCredentialsReturnsFalse()
        {
            string email = "lane@gmail.com";
            string password = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";
            bool expectedResult = false;
            bool actualResult = false;

            actualResult = _userManager.VerifyUser(email, password);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
