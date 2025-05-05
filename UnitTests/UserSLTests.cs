using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace UnitTests
{
    internal class UserSLTests
    {

        private UserBL user;
        private UserSL userSL;

        [SetUp]
        public void SetUp() 
        {
            user = new UserBL("ama@gmail.com", "aaaaAff12");
            userSL=new UserSL(user);
        }



        [Test]
        public void ConstructerEmail_Test()
        {
            Assert.AreEqual(userSL.Email, user.Email);
        }

        //public void Constructerbourd_Test()
        //{
        //    CollectionAssert.AreEqual(user.Boards.Select(b => b.Name).ToList(),userSL.Boards.Select(b => b.Name).ToList());
        //}



    }
}
