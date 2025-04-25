using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace UnitTests
{
    public class BoardTests
    {
        
        private AuthenticationFacade _authFacade;
        private BoardFacade _boardFacade;
        private UserFacade _userFacade;
        private UserBL testUser;

        [SetUp]
        public void Setup()
        {
            _authFacade = new AuthenticationFacade();
            _boardFacade = new BoardFacade(_authFacade);
            _userFacade = new UserFacade(this._authFacade);
            testUser = new UserBL("oriatta@post.bgu.ac.il", "abcd");
            _authFacade.Register(testUser.Email);
            _boardFacade.RegisterInFacade(testUser);
            _userFacade.Register(testUser.Email, testUser);
        }

        [Test]
        public void BoardCreateTest_Success()
        {
            try
            {
                BoardBL b = this._userFacade.CreateBoard("Test", testUser.Email, 10);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        [Test]
        public void BoardCreateTest_Fail()
        {
            try
            {
                BoardBL b = this._userFacade.CreateBoard("Test", testUser.Email, 10);
                b = this._userFacade.CreateBoard("Test", testUser.Email, 10);
                Assert.Fail();

            }
            catch (Exception ex)
            {
            }

            try
            {
                BoardBL b = this._userFacade.CreateBoard("a", "a", 10);
                Assert.Fail();

            }
            catch (Exception ex)
            {
            }

            try
            {
                BoardBL b = this._userFacade.CreateBoard("Test", testUser.Email, -1);
                Assert.Fail();

            }
            catch (Exception ex)
            {
            }

        }
    }
}
