﻿using DataAccessFakes;
using DataObjects;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            int testID = 10003;
            string expectedRole = "Administrator";
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

        [TestMethod]
        public void TestGetInactiveUsersCountPasses()
        {
            List<User> users = null;
            int expectedResult = 1;
            int actualResult = 0;
            users = _userManager.GetInactiveUsers();
            actualResult = users.Count;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestGetInactiveUsersCountFails()
        {
            List<User> users = null;
            int expectedResult = 2;
            int actualResult = 0;
            users = _userManager.GetInactiveUsers();
            actualResult = users.Count;
            Assert.AreNotEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestGetActiveUsersCountPasses()
        {
            List<User> users = null;
            int expectedResult = 1;
            int actualResult = 0;
            users = _userManager.GetActiveUsers();
            actualResult = users.Count;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestGetActiveUsersCountFails()
        {
            List<User> users = null;
            int expectedResult = 2;
            int actualResult = 0;
            users = _userManager.GetActiveUsers();
            actualResult = users.Count;
            Assert.AreNotEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestUpdateUserIsActivePasses()
        {
            bool result = false;
            int userID = 10003;
            result = _userManager.UpdateUserIsActive(userID, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestResetPasswordWorksCorrectly()
        {

            string email = "jane@gmail.com";
            string password = "password";
            string newPassword = "password1";
            bool expectedResult = true;
            bool actualResult = false;

            actualResult = _userManager.ResetPassword(email, password, newPassword);


            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestResetPasswordFailsWithBadPassword()
        {

            string email = "jane@gmail.com";
            string password = "notapassword";
            string newPassword = "password";
            bool actualResult = false;

            actualResult = _userManager.ResetPassword(email, password, newPassword);

        }
    }
}
