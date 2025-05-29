using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using IntroSE.Kanban.Backend.DataAccessLayer;

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
        private SystemController _systemController;

        [SetUp]
        public void Setup()
        {
            this._systemController = new SystemController();

            _authFacade = new AuthenticationFacade();
            _userFacade = new UserFacade(this._authFacade);
            _boardFacade = new BoardFacade(_authFacade, _systemController);

            this._systemController.ClearData();

            this._userFacade.Register("oriatta@post.bgu.ac.il", "Abcdefg123456");
            this._userFacade.Register("o@post.bgu.ac.il", "Abcdefg123456");

            testUser2 = this._userFacade.GetUserBL("o@post.bgu.ac.il");
            testUser = this._userFacade.GetUserBL("oriatta@post.bgu.ac.il");

            _boardFacade.RegisterInFacade(testUser.Email);
            _boardFacade.RegisterInFacade(testUser2.Email);
        }

        [Test]
        public void DB_Testings()
        {
            this._boardFacade.CreateBoard("Test1", testUser.Email);
            this._boardFacade.CreateBoard("Test2", testUser2.Email);
            this._boardFacade.CreateBoard("Test3", testUser.Email);

            this._boardFacade.CreateTask(testUser2.Email, "Test2", "A", "", DateTime.MaxValue);
            this._boardFacade.DeleteBoard(testUser2.Email, "Test2");

            this._boardFacade.AddUserAsMember(testUser2.Email, 0);
            this._boardFacade.CreateTask(testUser2.Email, "Test1", "A", "", DateTime.MaxValue);
            this._boardFacade.AssignTaskToMember(testUser2.Email, "Test1", 0, 1, testUser2.Email);
            this._boardFacade.EditTask(testUser2.Email, "Test1", 0, 1, DateTime.Now, null, null);
            this._boardFacade.EditTask(testUser2.Email, "Test1", 0, 1, DateTime.MinValue, "AA", null);
            this._boardFacade.EditTask(testUser2.Email, "Test1", 0, 1, DateTime.MinValue, null, "Desc");
            this._boardFacade.CreateTask(testUser.Email, "Test1", "B", "", DateTime.MaxValue);
            this._boardFacade.TransferOwnership(testUser.Email, testUser2.Email, "Test1");
            this._boardFacade.TransferOwnership(testUser2.Email, testUser.Email, "Test1");


            this._boardFacade.Setlimit(testUser2.Email, "Test1", 0, 5);
            this._boardFacade.Setlimit(testUser2.Email, "Test1", 1, 0);
            this._boardFacade.RemoveUserAsMember(testUser2.Email, 0);



        }
        [Test]

        public void Clear()
        {
            this._systemController.ClearData();
        }
        

    }
}
