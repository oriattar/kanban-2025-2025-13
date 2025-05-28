using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace UnitTests
{
    /// <summary>
    /// All test were ensured using sql data browser
    /// </summary>
    internal class DBTesting
    {
        private AuthenticationFacade _authFacade;
        private BoardFacade _boardFacade;
        private UserFacade _userFacade;
        private UserBL testUser;
        private UserBL testUser2;

        [SetUp]
        public void Setup()
        {
            _authFacade = new AuthenticationFacade();
            _boardFacade = new BoardFacade(_authFacade);
            _userFacade = new UserFacade(this._authFacade);
            testUser = new UserBL("oriatta@post.bgu.ac.il", "Abcdefg123456");
            testUser2 = new UserBL("o@post.bgu.ac.il", "Abcdefg123456");


            this._userFacade.Register(testUser.Email, "Abcdefg123456");
            this._userFacade.Register(testUser2.Email, "Abcdefg123456");

            testUser2 = this._userFacade.GetUserBL(testUser2.Email);
            testUser = this._userFacade.GetUserBL(testUser.Email);

            _boardFacade.RegisterInFacade(testUser.Email);
            _boardFacade.RegisterInFacade(testUser2.Email);
        }

        [Test]
        public void DB_Testings()
        {
            this._boardFacade.CreateBoard("Test1", testUser.Email);
            this._boardFacade.CreateBoard("Test2", testUser2.Email);
            this._boardFacade.CreateBoard("Test3", testUser.Email);

            this._boardFacade.DeleteBoard(testUser2.Email, "Test2");

            this._boardFacade.AddUserAsMember(testUser2.Email, 0);
            this._boardFacade.TransferOwnership(testUser.Email, testUser2.Email, "Test1");
            this._boardFacade.Setlimit(testUser2.Email,"Test1", 0, 5);
            this._boardFacade.Setlimit(testUser2.Email, "Test1", 1, 0);
            this._boardFacade.RemoveUserAsMember(testUser.Email,0);

        }

    }
}
