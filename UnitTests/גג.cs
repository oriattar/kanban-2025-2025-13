using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using NUnit.Framework.Constraints;

namespace UnitTests
{
    internal class גג
    {

        [Test]
        public void test()
        {
            Console.WriteLine("hhh");

            UserController u =new UserController();

            List<UserDAL> uy=u.SelectAll();

            foreach (UserDAL name in uy)
            {
                Console.WriteLine(name.Email);
            }

        }
    }
}
