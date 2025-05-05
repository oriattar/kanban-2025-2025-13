using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.Win32;

namespace UnitTests
{

    
    internal class UserServicesTests
    {

        private UserServices us;
        private UserFacade uFacade;
        private AuthenticationFacade aFacade;
        private string email;
        private string password;


        [SetUp]
        public void SetUp()
        {
            aFacade= new AuthenticationFacade();
            uFacade = new UserFacade(aFacade);
            us = new UserServices(uFacade);
            email = "amal@gmail.com";
            password = "asdfA123";

        }


        [Test]
        public void RegisterSuccessReturnsUserJson_Test()
        {
            UserBL user = new UserBL(email, password);
            UserSL userSL = new UserSL(user);

            string result = us.Register(email, password);

            string a = JsonSerializer.Serialize(new Response<string>(null, null));

            Assert.AreEqual(a, result);
        }

        [Test]
        public void Register_Failure_ReturnsErrorJson()
        {

            string result = us.Register(email, password);
            result = us.Register(email, password);

            Response<UserSL> response = JsonSerializer.Deserialize<Response<UserSL>>(result);

            Assert.AreEqual(response.ErrorMessage, "Email already exists");
            Assert.IsNull(response.ReturnValue);
        }


        [Test]
        public void Logout_Success_ReturnsSuccessJson()
        {

            us.Register(email, password);
            string result = us.Logout(email);

            string a = JsonSerializer.Serialize(new Response<string>(null, null));

            Assert.AreEqual(a, result);
        }

        [Test]
        public void Logout_Failure_ReturnsErrorJson()
        {

            string result = us.Logout(email);

            string a = JsonSerializer.Serialize(new Response<string>("User is not registerd in authenticationFacade -AUTH_FACADE", null ));
            Assert.AreEqual(a, result);
        }

        [Test]
        public void Login_Success_ReturnsSuccessJson()
        {
            us.Register(email, password);
            string result = us.Login(email, password);

            string expected = JsonSerializer.Serialize(new Response<string>(null,email));
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Login_Failure_ReturnsErrorJson()
        {
            string result = us.Login("not@registered.com", "AnyPass123");

            string expected = JsonSerializer.Serialize(new Response<string>("Email or password incorrect.", null));
            Assert.AreEqual(expected, result);
        }

    }
}
