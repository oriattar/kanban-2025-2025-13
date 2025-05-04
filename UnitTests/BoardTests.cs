using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                this._userFacade.CreateBoard("Test", testUser.Email);

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
                this._userFacade.CreateBoard("Test", testUser.Email);
                this._userFacade.CreateBoard("Test", testUser.Email);
                Assert.Fail();

            }
            catch (Exception ex)
            {
            }

            try
            {
                this._userFacade.CreateBoard("a", "a");
                Assert.Fail();

            }
            catch (Exception ex)
            {
            }
        }

        [Test]
        public void BoardDeleteTest_Success()
        {
            try
            {
                this._userFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.DeleteBoard(testUser.Email, "Test");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]

        public void BoardDelete_Fail()
        {
            try
            {
                this._boardFacade.DeleteBoard(testUser.Email, "Test");
                Assert.Fail();
            }
            catch (Exception ex)
            { }
            try
            {
                this._userFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.DeleteBoard("a", "Test");
                Assert.Fail();
            }
            catch (Exception ex)
            { }
            try
            {
                this._userFacade.CreateBoard("Test1", testUser.Email);
                this._boardFacade.DeleteBoard(testUser.Email, "Test1");
                this._boardFacade.DeleteBoard(testUser.Email, "Test1");
                Assert.Fail();
            }
            catch (Exception ex)
            { }

        }

        [Test]

        public void ColLimitSet_Success()
        {
            try
            {
                this._userFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test", 0, 10);
                this._boardFacade.Setlimit(testUser.Email, "Test", 1, 20);
                this._boardFacade.Setlimit(testUser.Email, "Test", 2, 30);
                this._boardFacade.Setlimit(testUser.Email, "Test", 2, -1);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

        }

        [Test]

        public void ColLimitSet_Fail()
        {
            try
            {
                this._userFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test", 3, 10);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._userFacade.CreateBoard("Tes1t", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test1", -1, 10);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._userFacade.CreateBoard("Test2", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test2", 0, -2);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._userFacade.CreateBoard("Test3", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test3", 0, 1);
                this._boardFacade.CreateTask(testUser.Email, "Test3", "T", "a", DateTime.MaxValue);
                this._boardFacade.Setlimit(testUser.Email, "Test3", 0, 0);
                Assert.Fail();
            }
            catch { }

        }

        [Test]

        public void ColByID_success()
        {
            try
            {
                this._userFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test", 0, 10);
                this._boardFacade.Setlimit(testUser.Email, "Test", 1, 10);
                this._boardFacade.Setlimit(testUser.Email, "Test", 2, 10);

                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "Test", 0);
                Assert.AreEqual(c.Name, "backlog");
                Assert.AreEqual(c.TaskLimit, 10);
                //Assert.AreEqual(c.Tasks, null);

                c = this._boardFacade.GetColByID(testUser.Email, "Test", 1);
                Assert.AreEqual(c.Name, "in progress");
                Assert.AreEqual(c.TaskLimit, 10);
                //Assert.AreEqual(c.Tasks, null);

                c = this._boardFacade.GetColByID(testUser.Email, "Test", 2);
                Assert.AreEqual(c.Name, "done");
                Assert.AreEqual(c.TaskLimit, 10);
                //Assert.AreEqual(c.Tasks, null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]

        public void ColByID_Fail()
        {
            try
            {
                this._userFacade.CreateBoard("Test", testUser.Email);
                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "Test", 3);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._userFacade.CreateBoard("Test1", testUser.Email);
                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "Test1", -1);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._userFacade.CreateBoard("Test2", testUser.Email);
                ColumnBL c = this._boardFacade.GetColByID("A", "Test2", 0);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._userFacade.CreateBoard("Test3", testUser.Email);
                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "A", 0);
                Assert.Fail();
            }
            catch { }
        }
        [Test]

        public void CreateTask_success()
        {
            try
            {
                this._userFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "", DateTime.MaxValue);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]

        public void CreateTask_failure()
        {

            this._userFacade.CreateBoard("Test", testUser.Email);

            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T", "a", DateTime.MinValue);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "", "a", DateTime.MaxValue);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", null, "a", DateTime.MaxValue);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabcdefffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff", DateTime.MaxValue);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.Setlimit(testUser.Email, "Test", 0, 0);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T", "a", DateTime.MaxValue);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test1", "T", "a", DateTime.MaxValue);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateTask("a", "Test", "T", "a", DateTime.MaxValue);
                Assert.Fail();
            }
            catch { }
        }

        [Test]

        public void taskAdvancing_success()
        {
            this._userFacade.CreateBoard("Test", testUser.Email);
            this._boardFacade.Setlimit(testUser.Email, "Test", 0, 1);
            this._boardFacade.CreateTask(testUser.Email, "Test", "T", "a", DateTime.MaxValue);

            try
            {
                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 0);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "a", DateTime.MaxValue);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 1);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 1, 1);
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [Test]

        public void taskAdvancing_failure()
        {
            this._userFacade.CreateBoard("Test", testUser.Email);
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T", "a", DateTime.MaxValue);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 0);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 1, 0);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 2, 0);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "", DateTime.MaxValue);
                this._boardFacade.Setlimit(testUser.Email, "Test", 1, 0);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 1);
                Assert.Fail();
            }
            catch { }
            this._boardFacade.Setlimit(testUser.Email, "Test", 1, -1);
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T3", "", DateTime.MaxValue);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 20);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T3", "", DateTime.MaxValue);
                this._boardFacade.advanceTasK(testUser.Email, "Test10", 0, 3);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T3", "", DateTime.MaxValue);
                this._boardFacade.advanceTasK("o", "Test", 0, 4);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._userFacade.CreateBoard("Test2", testUser.Email);
                this._boardFacade.CreateTask(testUser.Email, "Test2", "T1", "", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test2", "T2", "", DateTime.MaxValue);
                this._boardFacade.advanceTasK(testUser.Email, "Test2", 0, 5);

                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "Test2", 0);
                if (c.Tasks.Count != 1)
                    Assert.Fail();
            }
            catch { }
        }

        [Test]

        public void GetInProgress()
        {
            this._userFacade.CreateBoard("Test", testUser.Email);
            this._userFacade.CreateBoard("Test1", testUser.Email);
            this._userFacade.CreateBoard("Test2", testUser.Email);
            try
            {
                
                this._boardFacade.CreateTask(testUser.Email, "Test", "T1", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T3", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T4", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T5", "a", DateTime.MaxValue);

                this._boardFacade.CreateTask(testUser.Email, "Test1", "T6", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test1", "T7", "a", DateTime.MaxValue);

                this._boardFacade.CreateTask(testUser.Email, "Test2", "T8", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test2", "T9", "a", DateTime.MaxValue);

                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 0);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 1, 0);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 1);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 3);
                this._boardFacade.advanceTasK(testUser.Email, "Test", 0, 4);

                this._boardFacade.advanceTasK(testUser.Email, "Test1", 0, 6);

                this._boardFacade.advanceTasK(testUser.Email, "Test2", 0, 7);
                this._boardFacade.advanceTasK(testUser.Email, "Test2", 0, 8);
                this._boardFacade.advanceTasK(testUser.Email, "Test2", 1, 8);

                List<TaskBL> t = this._boardFacade.GetInProgressList(testUser.Email);
                Assert.AreEqual(t[0].Id, 1);
                Assert.AreEqual(t[1].Id, 3);
                Assert.AreEqual(t[2].Id, 4);
                Assert.AreEqual(t[3].Id, 6);
                Assert.AreEqual(t[4].Id, 7);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
