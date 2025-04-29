
using System.Reflection;
using System.Runtime.CompilerServices;

using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.Buiesness_layer;
using System.Threading.Tasks;
using System;
using log4net.Config;
using log4net;
using System.Text.Json.Nodes;
using System.Text.Json;

public class Test
{
    private ServiceFactory sf;

    public Test()
    {
        this.sf = new ServiceFactory();
        

    }

    public static void Main(string[] args)

    {
        var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config")); //inits the logger

        Test tester = new Test();
    }

    /// <summary>
    /// This function tests Requirement 7 for successful registration.
    /// It checks various invalid registration scenarios, such as null values, password length constraints, and password character requirements.
    /// The function also checks if valid registrations succeed, including checking for duplicate emails and invalid email formats.
    /// </summary>
    public bool TestSuccessfullRegister()
    {
        Response<UserSL> res = this.sf.us.Register(null, null);
        if (res.ErrorMsg == null)
        {
            return false;
            Console.WriteLine("❌ Test 1 Failed: Email and password cannot be null.");
        }

        res = this.sf.us.Register("aaa@gmail.com", null);
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 2 Failed: Password cannot be null.");
            return false;
        }

        res = this.sf.us.Register("bbb@gmail.com", "Aa7");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 3 Failed: Password too short.");
            return false;
        }

        res = this.sf.us.Register("ccc@gmail.com", "aaaaaaaAAAAAAAAAAAAAAA77");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 4 Failed: Password too long or violates max length.");
            return false;
        }

        res = this.sf.us.Register("ddd@gmail.com", "aaaaaaaaaa7");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 5 Failed: Missing uppercase letters in password.");
            return false;
        }


        res = this.sf.us.Register("eee@gmail.com", "AAAAAAAAAAA7");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 6 Failed: Missing lowercase letters in password.");
            return false;
        }


        res = this.sf.us.Register("fff@gmail.com", "AAAAAAaaaaa");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 7 Failed: Missing digit in password.");
            return false;
        }


        res = this.sf.us.Register("ggg@gmail.com", "aScA78f");
        if (res.ErrorMsg != null)
        {
            Console.WriteLine("❌ Test 8 Failed: Valid password should have succeeded.");
            return false;
        }

        res = this.sf.us.Register("ggg@gmail.com", "aScA78f");
        if (res.ErrorMsg != null)
        {
            Console.WriteLine("❌ Test 9 Failed: Valid password should have succeeded.");
            return false;
        }

        res = this.sf.us.Register("rrr@gmail.com", "AAAA555aaaa");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 10 Failed: Duplicate email should have failed.");
            return false;
        }


        res = this.sf.us.Register("rrrm", "AAAA555aaaa");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 11 Failed: Invalid email format should have failed (missing '@' and domain).");
            return false;
        }



        res = this.sf.us.Register("abcs@gmail.com", "AAAA5aaaa");
        if (res.returnValue == null)
        {
            Console.WriteLine("❌ Test 12 Failed: should have succeeded and return userSL");
            return false;
        }
        if (!res.returnValue.Email.Equals("abcs@gmail.com"))
        {
            Console.WriteLine("❌ Test 13 Failed: return wrong userSL email");
            return false;
        }
        if (res.returnValue.Boards.Count != 0)
        {
            Console.WriteLine("❌ Test 14 Failed: Did not set the board number to 0 ");
            return false;
        }

        return true;
    }

    /// <summary>
    /// This function tests the login functionality after successful registration.
    /// It checks if users can log in with the correct credentials and if login fails with incorrect credentials.
    /// Additionally, it checks if duplicate registrations for the same email are handled correctly.
    /// </summary>

    public bool TestSuccessfulllogin()
    {

        Response<UserSL> res = this.sf.us.Register("afffffbcs@gmail.com", "AAAAgd5aa");
        res = this.sf.us.Login("afffffbcs@gmail.com", "AAAAgd5aa");

        if (res.errorMsg != null)
        {
            Console.WriteLine("❌ Test 15 Failed: should have succeeded and return userSL");
            return false;
        }

        res = this.sf.us.Login("afffffbcs@gmail.com", "AAAAgd5aa");
        if (res.errorMsg == null)
        {
            Console.WriteLine("❌ Test 16 Failed: should have failed, password not correct ");
            return false;
        }

        res = this.sf.us.Login("afffffbcs@gmail.com", "AAAAgd5aa");
        if (res.errorMsg == null)
        {
            Console.WriteLine("❌ Test 17 Failed: should have failed, email not correct ");
            return false;
        }



        res = this.sf.us.Register("rrxxxr@gmail.com", "AAAA555aaaa");
        res = this.sf.us.Register("rrxxxr@gmail.com", "AAAA555aaaa");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 18 Failed: already register");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Method that test the logout operation.
    /// </summary>
    /// <returns></returns>
    public bool TestSuccessfulllogout()
    {

        Response<UserSL> res = this.sf.us.Register("testLogout@gmail.com", "AAAAgd5aa");

        res = this.sf.us.Login("testLogout@gmail.com", "AAAAgd5aa");

        Response<string> res2 = this.sf.us.Logout("testLogout@gmail.com");
        if (res2.errorMsg != null)
        {
            Console.WriteLine(res2.errorMsg);
            return false;
        }

        res2 = this.sf.us.Logout("testLogout@gmail.com");
        if (res2.errorMsg == null)
        {
            Console.WriteLine("Succefully logged out not logged in user.");
            return false;
        }
        res2 = this.sf.us.Logout("o@gmail.com");
        if (res2.errorMsg == null)
        {
            Console.WriteLine("Succefully logged out non existant user.");
            return false;
        }



        return true;
    }

    /*
        * =====================================
        * 
        * 
        * Methods for testing board operations
        * 
        * 
        * =====================================
        * Note:To run these successufuly we first must register 2 test users in the system:
        * email:oriatta@post.bgu.ac.il
        * email:oriattar12@post.bgu.ac.il
        */


    /*
     * Method for testing board creation first one is expceted to be successful, second one we expect
     */
    public void runBeforeBoardTest()
    {
        Response<UserSL> res = this.sf.us.Register("oriatta@post.bgu.ac.il", "Aa123456");
        res = this.sf.us.Register("oriattar12@post.bgu.ac.il", "Aa123456");
    }
    public bool testBoardCreation()
    {
        Response<string> res = JsonSerializer.Deserialize <Response<string>> (this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1"));//expecting success
        if (res.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
            return false;
        }


        Response<string> res2 = JsonSerializer.Deserialize < Response<string>> ((this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1"))); //expected error due to duplication
        if (res2.errorMsg == null)
        {
            Console.WriteLine("Error - a duplicate board was created for the user.");
            return false;
        }


        Response<string> res3 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriattar12@post.bgu.ac.il", "Test1"));//expcted success
        if (res3.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
            return false;
        }


        Response<string> res4 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("o@post.bgu.ac.il", "Test1"));// trying to create a board on a non existant user
        if (res2.errorMsg == null)
        {
            Console.WriteLine("Error - a  board was created for a non existant user.");
            return false;
        }


        return true;
    }

    /*
     * Method for testing board deletion, accounting for all the scenarios.
     * 
     */
    public bool testBoardDeletion()
    {
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1")); // creating a board to test
        if (res.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
        }


        Response<string> res1 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.DeleteBoard("oriatta@post.bgu.ac.il", "Test1")); //expecting a succefull delition
        if (res1.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
            return false;
        }


        Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.DeleteBoard("oriatta@post.bgu.ac.il", "Test1"));//expecting failiure
        if (res.errorMsg == null)
        {
            Console.WriteLine("Error - non existent or already deleted board was successfuly deleted");
            return false;
        }


        Response<string> res3 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.DeleteBoard("o@post.bgu.ac.il", "Test1"));//expectinf failiure
        if (res3.errorMsg == null) // failure is expceted as email isnt registered
        {
            Console.WriteLine("Error - non existent user's board was successfuly deleted");
            return false;
        }

        return true;
    }

    /*
     * A method for testing task collumns liming.
     */
    public bool testLimitingColls()
    {
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1"));
        if (res.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
        }

        int limit = 0;
        Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.SetLimit("oriatta@post.bgu.ac.il", "Test1", limit,0));//expecting failiure
        if (res2.errorMsg == null)
        {
            Console.WriteLine("ilegal limiting was succefull");
            return false;
        }


        limit = 20;
        res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.SetLimit("oriatta@post.bgu.ac.il", "Test1", limit,0));//expecting success
        if (res2.errorMsg != null)
        {
            Console.WriteLine(res2.errorMsg);
            return false;
        }

        return true;
    }

    /*
     * A method that tests the inProgress operation which lists all the tasks under 'ongoing' on all of a user's boards
     */
    public bool GetInProgressList()
    {
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1"));
        if (res.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
        }

        BoardSL b1 = null; //NOT IMPLIMENTED YET

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test2"));
        if (res.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
        }

        BoardSL b2 = null;// NOT IMPLIMENTED YET

        //creating temp boards
        DateTime deadline = DateTime.Parse("30/12/2025 10:00:00 AM"); // creating temp deadline


        Response<TaskSL> res2 = this.sf.ts.CreateTask(deadline, "Test", "TestDescription", "oriatta@post.bgu.ac.il", "Test1");
        if (res2.errorMsg != null)
        {
            Console.WriteLine(res2.errorMsg);
        }
        TaskSL t1 = res2.returnValue;

        res2 = this.sf.ts.CreateTask(deadline, "Test2", "TestDescription2", "oriatta@post.bgu.ac.il", "Test2");
        if (res2.errorMsg != null)
        {
            Console.WriteLine(res2.errorMsg);
        }
        TaskSL t2 = res2.returnValue;
        //creating test tasks

        Response<string> res3 = this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "Test1", t1.Id);
        if (res3.errorMsg != null)
        {
            Console.WriteLine(res3.errorMsg);
            return false;
        }
        res3 = this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "Test2", t2.Id);
        if (res3.errorMsg != null)
        {
            Console.WriteLine(res3.errorMsg);
            return false;
        }
        //advancing test tasks.


        Response<List<TaskSL>> res4 = this.sf.bs.GetInProgressList("oriatta@post.bgu.ac.il"); //testing the operation
        if (res4.errorMsg != null)
        {
            Console.WriteLine(res4.errorMsg);
            return false;
        }

        List<TaskSL> tasks = res4.returnValue;
        for (int i = 0; i < tasks.Count; i++) //printing the ongoing tasks
        {
            Console.WriteLine(tasks[i].Id + " " + tasks[i].Title + " " + tasks[i].Description);
        }
        return true;
    }

    /// <summary>
    /// Creating a board for task testing.
    /// </summary>
    public void createTestBoard()
    {
        this.runBeforeBoardTest();
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "test"));
    }

    /// <summary>
    /// A method for task creation testing
    /// </summary>
    /// <returns></returns>
    public bool checkCreateTask()
    {
        DateTime dateTime = DateTime.Now;
        String description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        String title = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        string bname = "test";
        string email = "oriatta@post.bgu.ac.il";


        Response<TaskSL> res = this.sf.ts.CreateTask(dateTime, title, description, bname, email);
        if (res.errorMsg != null)
        {
            Console.WriteLine("");
            return false;
        }

        //בדיקת עם שם לוח שלא קיים במערכת
        dateTime = DateTime.Now;
        description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        title = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        bname = "A";
        email = "oriatta@post.bgu.ac.il";

        res = this.sf.ts.CreateTask(dateTime, title, description, bname, email);
        if (res.errorMsg == null)
        {
            Console.WriteLine("");
            return false;
        }


        //בדיקת הוספת משימה ללוח קיים עם מייל שונה
        dateTime = DateTime.Now;
        description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        title = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        bname = "test";
        email = "aaaaaaaggg@gmail.com";
        res = this.sf.ts.CreateTask(dateTime, title, description, bname, email);
        if (res.errorMsg == null)
        {
            Console.WriteLine("");
            return false;
        }


        //בדיקה עם כותרת ריקה
        dateTime = DateTime.Now;
        description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        title = "";
        bname = "test";
        email = "oriatta@post.bgu.ac.il";


        res = this.sf.ts.CreateTask(dateTime, title, description, bname, email);
        if (res.errorMsg == null)
        {
            Console.WriteLine("");
            return false;
        }


        //בדיקה עם description ריק
        dateTime = DateTime.Now;
        description = "";
        title = "gggggggggg";
        bname = "test";
        email = "oriatta@post.bgu.ac.il";
        res = this.sf.ts.CreateTask(dateTime, title, description, bname, email);
        if (res.errorMsg == null)
        {
            Console.WriteLine("");
            return false;
        }

        //בדיקה עם PastDatetime ריק
        dateTime = DateTime.Now;
        description = "vvvvvvvvvvvv";
        title = "gggggggggg";
        bname = "test";
        email = "oriatta@post.bgu.ac.il";
        res = this.sf.ts.CreateTask(DateTime.Parse("30/12/2020 10:00:00 AM"), title, description, bname, email);
        if (res.errorMsg == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }

        res = this.sf.ts.CreateTask(dateTime, null, description, bname, email);
        if (res.errorMsg == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }

        res = this.sf.ts.CreateTask(dateTime, title, null, bname, email);
        if (res.errorMsg == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }

        res = this.sf.ts.CreateTask(dateTime, title, description, null, email);
        if (res.errorMsg == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }

        res = this.sf.ts.CreateTask(dateTime, title, description, bname, null);
        if (res.errorMsg == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }


        return true;


    }




    /// <summary>
    /// Method that tests task editing
    /// </summary>
    /// <returns></returns>
    public bool editTaskTest()
    {
        Response<TaskSL> res = this.sf.ts.CreateTask(DateTime.Now, "TestTask", "Test for editing", "test", "oriatta@post.bgu.ac.il"); //creating temp task
        if (res.errorMsg != null) 
        {
            Console.WriteLine(res.errorMsg);
            return false;
        }

        TaskSL ta = res.returnValue;

        res = this.sf.ts.EditTask(DateTime.Now, "NewTitle", "NewDescription", ta.Id, "test", "oriatta@post.bgu.ac.il"); // expecting success
        if (res.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
            return false;
        }

        res = this.sf.ts.EditTask(DateTime.Parse("30/12/2020 10:00:00 AM"), "NewTitle", "NewDescription", ta.Id, "test", "oriatta@post.bgu.ac.il"); //editing task with past deadline - failure
        if (res.errorMsg == null)
        {
            Console.WriteLine("A past deadline was successfully assigned");
            return false;
        }

        res = this.sf.ts.EditTask(DateTime.Now, null, "NewDescription", ta.Id, "test", "oriatta@post.bgu.ac.il"); //editing task with null title - failure
        if (res.errorMsg == null)
        {
            Console.WriteLine("A null title was assigned");
            return false;
        }
        res = this.sf.ts.EditTask(DateTime.Now, "NewTitle", null, ta.Id, "test", "oriatta@post.bgu.ac.il"); //editing task with null description - failure
        if (res.errorMsg == null)
        {
            Console.WriteLine("a null deadline was successfully assigned");
            return false;
        }

        return true;
    }

    /// <summary>
    /// A method to test task advancing.
    /// </summary>
    /// <returns></returns>
    public bool testTaskAdvancing()
    {
        Response<TaskSL> res = this.sf.ts.CreateTask(DateTime.Now, "TestTask", "Test for editing", "test", "oriatta@post.bgu.ac.il"); //creating temp task
        if (res.errorMsg != null)
        {
            Console.WriteLine(res.errorMsg);
            return false;
        }

        TaskSL t = res.returnValue;

        Response<string> res2 = this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "test", t.Id);//Advancing to on going
        if (res2.errorMsg != null)
        {
            Console.WriteLine(res2.errorMsg);
            return false;
        }

        res2 = this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "test", t.Id); //Advancing to done
        if (res2.errorMsg != null)
        {
            Console.WriteLine(res2.errorMsg);
            return false;
        }

        res2 = this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "test", t.Id); // expected failure - no where to advance.
        if (res2.errorMsg == null)
        {
            Console.WriteLine("A task was advanced beyond 'Done'.");
            return false;
        }

        res2 = this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "test", -1); //trying to advance non existant task
        if (res2.errorMsg == null)
        {
            Console.WriteLine(res2.errorMsg);
            return false;
        }

        return true;
    }

}



   
