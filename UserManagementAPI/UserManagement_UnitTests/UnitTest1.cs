using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using UserManagementAPI;

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
    }
}
