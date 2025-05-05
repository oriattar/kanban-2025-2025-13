using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using NUnit.Framework;

namespace UnitTests
{
    internal class UserFacadeTests
    {

        private AuthenticationFacade aFacade;
        private UserFacade ufacade;
        private Dictionary<string, UserBL> dic;

        [SetUp]
        public void SetUp() 
        {
            aFacade = new AuthenticationFacade();
            dic = new Dictionary<string, UserBL>();
            ufacade = new UserFacade(aFacade);
        }

        [Test]
        public void RegisterSameEmail_Test()
        {
            string email = "avdf@gmail.com";
            string password = "fasdA123";

            ufacade.Register(email, password);

            string email1 = "avdf@gmail.com";
            string password1 = "fasdA123";
            try
            {
                ufacade.Register(email1, password1);
                Assert.Fail("should have throw excaption - email already in system");
            }
            catch (ArgumentException ex)
            {
                Assert.Pass(ex.Message);
            }
        }

        [Test]
        public void Registerincorrectuser_Test()
        {
            
            string email1 = "addddvdf@gmail.com";
            string password1 = "fa123";
            try
            {
                ufacade.Register(email1, password1);
                Assert.Fail("should have throw excaption - not correct user");
            }
            catch (ArgumentException ex)
            {
                Assert.Pass(ex.Message);
            }
        }


        [Test]
        public void Register_Test()
        {
            string email = "avdf@gmail.com";
            string password = "fasdA123";


            try
            {
                ufacade.Register(email, password);
                Assert.Pass();
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("should not have throw excaption ");

            }
        }



        [Test]
        public void LoginEmailnotexist_Test()
        {

            string email = "avdf@gmail.com";
            string password = "fasdA123";

            ufacade.Register(email, password);

            try
            {
                ufacade.Login("avfffdf@gmail.com", password);
                Assert.Fail("should have throw excaption - email not in system");
            }
            catch (ArgumentException ex)
            {
                Assert.Pass(ex.Message);
            }
        }

        public void LoginWrongPassword_Test()
        {

            string email = "avdf@gmail.com";
            string password = "fasdA123";

            ufacade.Register(email, password);

            try
            {
                ufacade.Login(email, "password");
                Assert.Fail("should have throw excaption - incorrect password");
            }
            catch (ArgumentException ex)
            {
                Assert.Pass(ex.Message);
            }
        }

        public void LoginCorrect_Test()
        {

            string email = "avdf@gmail.com";
            string password = "fasdA123";

            ufacade.Register(email, password);

            try
            {
                ufacade.Login(email, password);
                Assert.Pass();
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("should not have throw excaption ");

            }
        }


    }
}
