using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace UnitTests
{
    public class UserBLTests
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("noat.com")]
        [TestCase("no.dot@com")]
        [TestCase("in valid@email.com")]
        [TestCase("@gmail.com")]
        [TestCase("user@.com")]
        [TestCase("user@gmail")]
        public void ConstructorWrongEmail_Tests(string email)
        {
            string password = "Ax12345";

            try
            {
                UserBL user = new UserBL(email, password);
                Assert.Fail("Expected an exception for invalid email, but none was thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.Pass();
            }

        }


        [TestCase("")]
        [TestCase("short")]
        [TestCase("thisPasswordIsWayTooLong123")]
        [TestCase("nouppercase1")]
        [TestCase("NOLOWERCASE1")]
        [TestCase("NoDigits")]
        public void ConstructorWrongPassword_Tests(string password)
        {
            string email = "abcde@gmail.com";

            try
            {
                UserBL user = new UserBL(email, password);
                Assert.Fail("Expected an exception for invalid password, but none was thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.Pass();
            }

        }


        [Test]
        public void Constructor_ValidInputemail_CreatesUser()
        {
            UserBL user = new UserBL("test@example.com", "Valid123");
            Assert.AreEqual("test@example.com", user.Email);
        }

        [Test]
        public void Constructor_ValidInputPassword_CreatesUser()
        {
            UserBL user = new UserBL("test@example.com", "Valid123");
            Assert.IsTrue(user.Login("Valid123"));
        }

        [Test]
        public void Login_CreatesUser()
        {
            UserBL user = new UserBL("testss@example.com", "Valid123"); 

            bool result = user.Login("WrongPass123");
            Assert.IsFalse(result, "Login should fail with incorrect password.");
        }

    }
}
