

using System.Reflection;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net.Config;
using log4net;

namespace UnitTests
{
    public class AuthenticationFacadeTests
    {
        private AuthenticationFacade _af;
        [SetUp]
        public void Setup()
        {
            this._af = new AuthenticationFacade();
            _af.Register("oriatta@bgu.ac.il");

            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config")); //inits the logger
        }

        [Test]
        public void TestLogin_success()
        {
            _af.Login("oriatta@bgu.ac.il");


            Assert.AreEqual(_af.IsLoggedIn("oriatta@bgu.ac.il"), true);

            _af.Register("o@a.com");
            _af.Logout("o@a.com");
            _af.Login("o@a.com");

            Assert.AreEqual(_af.IsLoggedIn("oriatta@bgu.ac.il"), true);

        }


        [Test]
        public void TestLogin_fail()
        {
            try
            {
                _af.Login("o@bgu.ac.il");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
        }

        [Test]

        public void TestLogout_success()
        {
            _af.Logout("oriatta@bgu.ac.il");
            Assert.AreEqual(_af.IsLoggedIn("oriatta@bgu.ac.il"), false);
        }

        [Test]
        public void TestLogut_fail()
        {
            try
            {
                _af.Logout("o@bgu.ac.il");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
        }

        [Test]
        public void TestIsLoggedIn_success()
        {
            Assert.AreEqual(_af.IsLoggedIn("oriatta@bgu.ac.il"), true);

            _af.Logout("oriatta@bgu.ac.il");

            Assert.AreEqual(_af.IsLoggedIn("oriatta@bgu.ac.il"), false);
        }

        [Test]
        public void TestIsLoggedIn_fail()
        {
            try
            {
                _af.IsLoggedIn("a");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }      
        }
    }
}