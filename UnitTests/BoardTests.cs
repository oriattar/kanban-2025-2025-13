using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using NUnit.Framework;

namespace UnitTests
{
    public class BoardTests
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
            _boardFacade = new BoardFacade(_authFacade,_systemController,0,0);
           
            
            



            this._userFacade.Register("oriatta@post.bgu.ac.il", "Abcdefg123456");
            this._userFacade.Register("o@post.bgu.ac.il", "Abcdefg123456");

            testUser2 = this._userFacade.GetUserBL("o@post.bgu.ac.il");
            testUser = this._userFacade.GetUserBL("oriatta@post.bgu.ac.il");

            _boardFacade.RegisterInFacade(testUser.Email);
            _boardFacade.RegisterInFacade(testUser2.Email);
        }

        [TearDown]
        public void Teardown()
        {
            this._systemController.ClearData();
        }

        [Test]
        public void BoardCreateTest_Success()
        {
            try
            {
                this._boardFacade.CreateBoard("Test", testUser.Email);

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
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.CreateBoard("Test", testUser.Email);
                Assert.Fail();

            }
            catch (Exception ex)
            {
            }

            try
            {
                this._boardFacade.CreateBoard("a", "a");
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
                this._boardFacade.CreateBoard("Test", testUser.Email);
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
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.DeleteBoard("a", "Test");
                Assert.Fail();
            }
            catch (Exception ex)
            { }
            try
            {
                this._boardFacade.CreateBoard("Test1", testUser.Email);
                this._boardFacade.DeleteBoard(testUser.Email, "Test1");
                this._boardFacade.DeleteBoard(testUser.Email, "Test1");
                Assert.Fail();
            }
            catch (Exception ex)
            { }
            try
            {
                this._boardFacade.CreateBoard("Test2", testUser.Email);
                this._boardFacade.AddUserAsMember(testUser2.Email, 2); //delete by non owner
                this._boardFacade.DeleteBoard(testUser2.Email, "Test2");

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        [Test]

        public void ColLimitSet_Success()
        {
            try
            {
                this._boardFacade.CreateBoard("Test", testUser.Email);
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
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test", 3, 10);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateBoard("Tes1t", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test1", -1, 10);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateBoard("Test2", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test2", 0, -2);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateBoard("Test3", testUser.Email);
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
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test", 0, 10);
                this._boardFacade.Setlimit(testUser.Email, "Test", 1, 10);
                this._boardFacade.Setlimit(testUser.Email, "Test", 2, 10);

                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "Test", 0);
                Assert.AreEqual(c.Name, "backlog");
                Assert.AreEqual(c.TaskLimit, 10);




                c = this._boardFacade.GetColByID(testUser.Email, "Test", 1);
                Assert.AreEqual(c.Name, "in progress");
                Assert.AreEqual(c.TaskLimit, 10);


                c = this._boardFacade.GetColByID(testUser.Email, "Test", 2);
                Assert.AreEqual(c.Name, "done");
                Assert.AreEqual(c.TaskLimit, 10);

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
                this._boardFacade.CreateBoard("Test", testUser.Email);
                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "Test", 3);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateBoard("Test1", testUser.Email);
                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "Test1", -1);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateBoard("Test2", testUser.Email);
                ColumnBL c = this._boardFacade.GetColByID("A", "Test2", 0);
                Assert.Fail();
            }
            catch { }
            try
            {
                this._boardFacade.CreateBoard("Test3", testUser.Email);
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
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "", DateTime.MaxValue);
                this._boardFacade.AddUserAsMember(testUser2.Email, 0);
                this._boardFacade.CreateTask(testUser2.Email, "Test", "T3", "", DateTime.MaxValue);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]

        public void CreateTask_failure()
        {

            this._boardFacade.CreateBoard("Test", testUser.Email);

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
            try
            {
                this._boardFacade.CreateTask(testUser2.Email, "Test", "T3", "a111", DateTime.MaxValue);
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        [Test]

        public void taskAdvancing_success()
        {
            this._boardFacade.CreateBoard("Test", testUser.Email);
            this._boardFacade.Setlimit(testUser.Email, "Test", 0, 1);
            this._boardFacade.CreateTask(testUser.Email, "Test", "T", "a", DateTime.MaxValue);
            this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 0, testUser.Email);

            try
            {

                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 0);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "a", DateTime.MaxValue);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 1, testUser.Email);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 1);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 1, 1);
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [Test]

        public void taskAdvancing_failure()
        {
            this._boardFacade.CreateBoard("Test", testUser.Email);
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T", "a", DateTime.MaxValue);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 0, testUser.Email);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 0);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 1, 0);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 2, 0);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "", DateTime.MaxValue);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 1, testUser.Email);
                this._boardFacade.Setlimit(testUser.Email, "Test", 1, 0);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 1);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            this._boardFacade.Setlimit(testUser.Email, "Test", 1, -1);
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T3", "", DateTime.MaxValue);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 2, testUser.Email);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 20);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T3", "", DateTime.MaxValue);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 3, testUser.Email);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test10", 0, 3);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T3", "", DateTime.MaxValue);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 4, testUser.Email);
                this._boardFacade.AdvanceTasK("o", "Test", 0, 4);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.CreateBoard("Test2", testUser.Email);
                this._boardFacade.CreateTask(testUser.Email, "Test2", "T1", "", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test2", "T2", "", DateTime.MaxValue);

                this._boardFacade.AssignTaskToMember(testUser.Email, "Test2", 0, 5, testUser.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test2", 0, 6, testUser.Email);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test2", 0, 5);

                ColumnBL c = this._boardFacade.GetColByID(testUser.Email, "Test2", 0);
                if (c.Tasks.Count != 1)
                    Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.CreateTask(testUser.Email, "Test", "T10", "", DateTime.MaxValue);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 7);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

        [Test]

        public void GetInProgress()
        {
            this._boardFacade.CreateBoard("Test", testUser.Email);
            this._boardFacade.CreateBoard("Test1", testUser.Email);
            this._boardFacade.CreateBoard("Test2", testUser.Email);
            try
            {

                this._boardFacade.CreateTask(testUser.Email, "Test", "T1", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T3", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T4", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T5", "a", DateTime.MaxValue);

                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 0, testUser.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 1, testUser.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 2, testUser.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 3, testUser.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 4, testUser.Email);

                this._boardFacade.CreateTask(testUser.Email, "Test1", "T6", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test1", "T7", "a", DateTime.MaxValue);

                this._boardFacade.AssignTaskToMember(testUser.Email, "Test1", 0, 5, testUser.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test1", 0, 6, testUser.Email);

                this._boardFacade.CreateTask(testUser.Email, "Test2", "T8", "a", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test2", "T9", "a", DateTime.MaxValue);

                this._boardFacade.AssignTaskToMember(testUser.Email, "Test2", 0, 7, testUser.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test2", 0, 8, testUser.Email);

                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 0);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 1, 0);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 1);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 3);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 4);

                this._boardFacade.AdvanceTasK(testUser.Email, "Test1", 0, 6);

                this._boardFacade.AdvanceTasK(testUser.Email, "Test2", 0, 7);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test2", 0, 8);
                this._boardFacade.AdvanceTasK(testUser.Email, "Test2", 1, 8);

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

        [Test]

        public void testGetBoardName_success()
        {
            try
            {
                this._boardFacade.CreateBoard("Test", testUser.Email);
                string name = this._boardFacade.GetBoardNameById(0);
                Assert.AreEqual("Test", name);
                this._boardFacade.CreateBoard("Test1", testUser.Email);
                this._boardFacade.CreateBoard("Test2", testUser.Email);
                name = this._boardFacade.GetBoardNameById(2);
                Assert.AreEqual("Test2", name);
                this._boardFacade.CreateBoard("Test3", testUser2.Email);
                name = this._boardFacade.GetBoardNameById(3);
                Assert.AreEqual("Test3", name);
            }
            catch (Exception ex) { Assert.Fail(); }
        }


        [Test]
        public void testGetBoardName_failure()
        {
            this._boardFacade.CreateBoard("Test", testUser.Email);
            try
            {
                _boardFacade.GetBoardNameById(13);
                Assert.Fail();
            }
            catch (Exception ex) { }
            try
            {
                string name = _boardFacade.GetBoardNameById(0);
                if (!name.Equals("Test"))
                    Assert.Fail();
            }
            catch (Exception ex) { }
        }

        [Test]
        public void testCollectIDs_success()
        {
            try
            {
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.CreateBoard("Test1", testUser.Email);
                this._boardFacade.CreateBoard("Test2", testUser.Email);
                this._boardFacade.CreateBoard("Test3", testUser.Email);
                this._boardFacade.CreateBoard("Test4", testUser.Email);

                this._boardFacade.AddUserAsMember(testUser2.Email, 0);

                List<int> IDs = this._boardFacade.CollectBoardIds(testUser.Email);
                for (int i = 0; i < 4; i = i + 1)
                {
                    if (!IDs.Contains(i))
                        Assert.Fail();
                }
                IDs = this._boardFacade.CollectBoardIds(testUser2.Email);

                if (IDs.Count != 1 || !IDs.Contains(0))
                    Assert.Fail();

            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [Test]
        public void testJoinBoard_sucess()
        {
            try
            {
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.AddUserAsMember(testUser2.Email, 0);
                BoardBL b = this._boardFacade.GetBoard(testUser.Email, "Test");
                if (!b.isActiveMember(testUser2.Email))
                    Assert.Fail();
                List<BoardBL> list = this._boardFacade.GetUserBoards(testUser.Email);
                if (!list.Contains(b))
                    Assert.Fail();

            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [Test]
        public void testJoinBoard_failure()
        {
            this._boardFacade.CreateBoard("Test", testUser.Email);
            try
            {
                this._boardFacade.CreateBoard("Test", testUser2.Email);
                this._boardFacade.AddUserAsMember(testUser2.Email, 0);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {

                this._boardFacade.AddUserAsMember(testUser2.Email, 100);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {

                this._boardFacade.AddUserAsMember("testUser2.Email", 0);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.CreateBoard("Test2", testUser2.Email);
                this._boardFacade.AddUserAsMember(testUser2.Email, 2);
                this._boardFacade.AddUserAsMember(testUser2.Email, 2);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        [Test]

        public void testBoardLeave_success()
        {
            try
            {
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.AddUserAsMember(testUser2.Email, 0);

                this._boardFacade.CreateTask(testUser2.Email, "Test", "T1", "", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser.Email, "Test", "T2", "", DateTime.MaxValue);
                this._boardFacade.CreateTask(testUser2.Email, "Test", "T3", "", DateTime.MaxValue);

                BoardBL b = this._boardFacade.GetBoard(testUser.Email, "Test");
                List<TaskBL> t = b.Columns[0].Tasks;
                t[0].Asignee = testUser2.Email;
                t[2].Asignee = testUser2.Email;

                this._boardFacade.RemoveUserAsMember(testUser2.Email, 0);

                if (b.isActiveMember(testUser2.Email))
                    Assert.Fail();
                List<BoardBL> list = this._boardFacade.GetUserBoards(testUser2.Email);
                if (list.Contains(b))
                    Assert.Fail();

                Assert.AreEqual(t[0].Asignee, "unassigned");
                Assert.AreEqual(t[2].Asignee, "unassigned");
            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [Test]
        public void testBoardLeave_failure()
        {
            this._boardFacade.CreateBoard("Test", testUser.Email);
            try
            {
                this._boardFacade.AddUserAsMember(testUser2.Email, 0);

                this._boardFacade.RemoveUserAsMember(testUser2.Email, 0);
                this._boardFacade.RemoveUserAsMember(testUser2.Email, 0);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.RemoveUserAsMember(testUser2.Email, 1);

                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.RemoveUserAsMember(testUser.Email, 0);

                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.AddUserAsMember(testUser2.Email, 0);
                this._boardFacade.TransferOwnership(testUser.Email, testUser2.Email, "Test");
                this._boardFacade.RemoveUserAsMember(testUser2.Email, 0);

                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

        [Test]
        public void testTransferOwnership_success()
        {
            try
            {
                this._boardFacade.CreateBoard("Test", testUser.Email);
                this._boardFacade.AddUserAsMember(testUser2.Email, 0);
                this._boardFacade.TransferOwnership(testUser.Email, testUser2.Email, "Test");

                BoardBL b = this._boardFacade.GetBoard(testUser2.Email, "Test");
                Assert.AreEqual(b.Owner, testUser2.Email);
                this._boardFacade.DeleteBoard(testUser2.Email, "Test");
            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [Test]
        public void testTransferOwnership_failure()
        {
            this._boardFacade.CreateBoard("Test", testUser.Email);
            this._boardFacade.AddUserAsMember(testUser2.Email, 0);

            try
            {
                this._boardFacade.TransferOwnership(testUser.Email, testUser.Email, "Test");
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.RemoveUserAsMember(testUser2.Email, 0);
                this._boardFacade.TransferOwnership(testUser.Email, testUser2.Email, "Test");
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            this._boardFacade.AddUserAsMember(testUser2.Email, 0);
            try
            {
                this._boardFacade.TransferOwnership(testUser.Email, "testUser2.Email", "Test");
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.TransferOwnership(testUser.Email, testUser2.Email, "T");
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        [Test]

        public void testAssignemnt_success()
        {
            this._boardFacade.CreateBoard("Test", testUser.Email);
            this._boardFacade.AddUserAsMember(testUser2.Email, 0);

            try
            {
                this._boardFacade.CreateTask(testUser2.Email, "Test", "T1", "", DateTime.MaxValue);

                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 0, testUser.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 0, testUser2.Email);
                this._boardFacade.AssignTaskToMember(testUser2.Email, "Test", 0, 0, testUser.Email);

                this._boardFacade.AdvanceTasK(testUser.Email, "Test", 0, 0);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 1, 0, testUser2.Email);
                this._boardFacade.AdvanceTasK(testUser2.Email, "Test", 1, 0);
                this._boardFacade.AssignTaskToMember(testUser2.Email, "Test", 2, 0, testUser.Email);
            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [Test]
        public void testAssignemnt_failure()
        {

            this._boardFacade.CreateBoard("Test", testUser.Email);
            this._boardFacade.AddUserAsMember(testUser2.Email, 0);
            this._boardFacade.CreateTask(testUser2.Email, "Test", "T1", "", DateTime.MaxValue);

            try
            {
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 0, "testUser.Email");
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 0, testUser2.Email);
                this._boardFacade.AssignTaskToMember(testUser.Email, "Test", 0, 0, testUser.Email);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.AssignTaskToMember("testUser.Email", "Test", 0, 0, testUser2.Email);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            try
            {
                this._boardFacade.AssignTaskToMember("testUser.Email", "Test", 0, -9, testUser2.Email);
                Assert.Fail();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
