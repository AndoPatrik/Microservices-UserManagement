using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI;
using UserManagementAPI.Models;

namespace UserManagement_UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        UserManagementAPI.Controllers.UsersController userController = new UserManagementAPI.Controllers.UsersController();
        UserManagementAPI.Models.Logger logger = new UserManagementAPI.Models.Logger();

        // LOGGER TESTS
        [TestMethod]
        public void correct_logfile_name_and_path()
        {
            // ARRANGE
            DateTime dateTime = DateTime.UtcNow.Date;
            string correct_fileName = dateTime.ToString("dd-MM-yyyy") + "_log.txt";
            string correct_filePath = Path.GetFullPath(correct_fileName);

            // ACT
            File.Delete(correct_filePath);
            logger.LogAction("Test");

            // ASSERT
            Assert.IsTrue(File.Exists(correct_filePath));
        }

        [TestMethod]
        public void correct_logfile_content()
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            string correct_fileName = dateTime.ToString("dd-MM-yyyy") + "_log.txt";
            string correct_filePath = Path.GetFullPath(correct_fileName);

            File.Delete(correct_filePath);
            logger.LogAction("Test text to log");
            string logfileContent = File.ReadAllText(correct_filePath);

            Assert.AreEqual(logfileContent, DateTime.Now.ToString("HH:mm:ss") + " - " + "Test text to log\r\n");
        }

        //Initialize Method for email and Post tests
        private readonly UserManagementContext _context = new UserManagementContext();
        List<Users> usersList;
        Users testUser;

        [TestInitialize]
        public void before()
        {
            //ARRANGE
            usersList = _context.Users.ToList<Users>();
            testUser = new Users();
        }

        //VALIDATE TEST
        [TestMethod]
        public void test_validate_invalid_id()
        {
            //ARRANGE
            int invalidId = 0;

            //ACT && ASSERT
            Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(() => userController.ValidateUser(invalidId));
        }

        [TestMethod]
        public void test_validate_valid_id()
        {
            //ARRANGE
            int validId = 1; //depends on the id that we test
            bool ?isVerified = false;

            //ACT
            userController.ValidateUser(validId);

            foreach (Users user in usersList)
            {
                if (user.Id == validId)
                {
                    isVerified = user.IsVerified; //should be true

                    //ASSERT
                    Assert.AreEqual(isVerified, true);
                }
            }
        }

        //POST TEST
        [TestMethod]
        public void test_post_test_user_list_count_not_changed()
        {
            //ACT
            userController.PostUsers(testUser);
            List<Users> afterList = _context.Users.ToList<Users>();
            //ASSERT
            Assert.AreEqual(usersList.Count, afterList.Count);  //number of users in the list did not chnaged
        }

        [TestMethod]
        public void test_post_test_user_list_count_changed()
        {
            //ARRANGE
            testUser.FirstName = "testName";
            testUser.LastName = "testLastName";
            testUser.Password = "testPassword";
            testUser.EmailAddress = "test@email";

            //ACT
            userController.PostUsers(testUser);
            List<Users> afterList = _context.Users.ToList<Users>();
            //ASSERT
            Assert.AreEqual(usersList.Count + 1, afterList.Count);  //number of users increased by 1
        }

        //EMAIL TESTS
        [TestMethod]
        public void email_not_sent_empty_object_return_exception()
        {
            //ACT & ASSERT
            Assert.ThrowsExceptionAsync<Exception>(() => userController.PostUsers(testUser)); //testing empty user object
        }

        [TestMethod]
        public void test_email_wrong_email_exception()
        {
            //ARRANGE
            testUser.EmailAddress = "wrongEmailAddress";

            //ACT & ASSERT
            Assert.ThrowsExceptionAsync<Exception>(() => userController.PostUsers(testUser));
        }

        [TestMethod]
        public async Task Test_user_update()
        {
            Users user = new Users("John", "Doe", false, "pass", "email@mail.com", "456789123", true, false);
            await userController.PostUsers(user);

            string oldPhoneNumber = user.PhoneNumber;
            user.PhoneNumber = "45788615";

            await userController.PutUsers(3, user);
            string newPhoneNumber = user.PhoneNumber;

            Assert.AreNotEqual(oldPhoneNumber, newPhoneNumber);
        }

        [TestMethod]
        public async Task Test_user_get_by_id()
        {
            int validId = 1;
            string expectedFirstName = "Aaron";

            await userController.GetUsers(validId);
            foreach (Users user in usersList)
            {
                if (user.Id == validId)
                {
                    string actualFirstName = user.FirstName;
                    Assert.AreEqual(expectedFirstName, actualFirstName);
                }
            }
        }

        [TestMethod]
        public void Test_user_delete_by_id()
        {
            int validId = 1;
            bool initialDeletedValue = false;

            userController.isDeletedUser(validId);
            foreach (Users user in usersList)
            {
                if (user.Id == validId)
                {
                    bool? finalDeletedValue = user.IsDeleted;
                    Assert.AreNotEqual(initialDeletedValue, finalDeletedValue);
                }
            }
        }

    }

}
