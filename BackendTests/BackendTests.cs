
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
        GradingService g = new GradingService();
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
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register(null, null));
        if (res.ErrorMessage == null)
        {
            return false;
            Console.WriteLine("❌ Test 1 Failed: Email and password cannot be null.");
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("aaa@gmail.com", null));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 2 Failed: Password cannot be null.");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("bbb@gmail.com", "Aa7"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 3 Failed: Password too short.");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("ccc@gmail.com", "aaaaaaaAAAAAAAAAAAAAAA77"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 4 Failed: Password too long or violates max length.");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("ddd@gmail.com", "aaaaaaaaaa7"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 5 Failed: Missing uppercase letters in password.");
            return false;
        }


        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("eee@gmail.com", "AAAAAAAAAAA7"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 6 Failed: Missing lowercase letters in password.");
            return false;
        }


        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("fff@gmail.com", "AAAAAAaaaaa"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 7 Failed: Missing digit in password.");
            return false;
        }


        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("ggg@gmail.com", "aScA78f"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine("❌ Test 8 Failed: Valid password should have succeeded.");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("ggg@gmail.com", "aScA78f"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine("❌ Test 9 Failed: Valid password should have succeeded.");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("rrr@gmail.com", "AAAA555aaaa"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 10 Failed: Duplicate email should have failed.");
            return false;
        }


        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("rrrm", "AAAA555aaaa"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 11 Failed: Invalid email format should have failed (missing '@' and domain).");
            return false;
        }



        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("abcs@gmail.com", "AAAA5aaaa"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine("❌ Test 12 Failed: should have succeeded");
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

        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("afffffbcs@gmail.com", "AAAAgd5aa"));
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Login("afffffbcs@gmail.com", "AAAAgd5aa"));

        if (res.ErrorMessage != null)
        {
            Console.WriteLine("❌ Test 15 Failed: should have succeeded");
            return false;
        }
        if (!res.ReturnValue.Equals("afffffbcs@gmail.com"))
        {
            Console.WriteLine("❌ Test 15 Failed: returned value should be:" + "afffffbcs@gmail.com");
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Login("afffffbcs@gmail.com", "AAAAgd5aa"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 16 Failed: should have failed, password not correct ");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Login("afffffbcs@gmail.com", "AAAAgd5aa"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("❌ Test 17 Failed: should have failed, email not correct ");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("rrxxxr@gmail.com", "AAAA555aaaa"));
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("rrxxxr@gmail.com", "AAAA555aaaa"));
        if (res.ErrorMessage == null)
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

        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("testLogout@gmail.com", "AAAAgd5aa"));

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Login("testLogout@gmail.com", "AAAAgd5aa"));

        Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Logout("testLogout@gmail.com"));
        if (res2.ErrorMessage != null)
        {
            Console.WriteLine(res2.ErrorMessage);
            return false;
        }

        res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Logout("testLogout@gmail.com"));
        if (res2.ErrorMessage == null)
        {
            Console.WriteLine("Succefully logged out not logged in user.");
            return false;
        }
        res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Logout("o@gmail.com"));
        if (res2.ErrorMessage == null)
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
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("oriatta@post.bgu.ac.il", "Aa123456"));
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.us.Register("oriattar12@post.bgu.ac.il", "Aa123456"));
    }
    public bool testBoardCreation()
    {
        Response<string> res = JsonSerializer.Deserialize <Response<string>> (this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1"));//expecting success
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }


        Response<string> res2 = JsonSerializer.Deserialize < Response<string>> ((this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1")));// expected error due to duplication
        if (res2.ErrorMessage == null)
        {
            Console.WriteLine("Error - a duplicate board was created for the user.");
            return false;
        }


        Response<string> res3 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriattar12@post.bgu.ac.il", "Test1"));//expcted success
        if (res3.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }


        Response<string> res4 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("o@post.bgu.ac.il", "Test1"));// trying to create a board on a non existant user
        if (res2.ErrorMessage == null)
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
        this.runBeforeBoardTest();
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1")); // creating a board to test
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
        }


        Response<string> res1 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.DeleteBoard("oriatta@post.bgu.ac.il", "Test1")); //expecting a succefull delition
        if (res1.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }


        Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.DeleteBoard("oriatta@post.bgu.ac.il", "Test1"));//expecting failiure
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Error - non existent or already deleted board was successfuly deleted");
            return false;
        }


        Response<string> res3 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.DeleteBoard("o@post.bgu.ac.il", "Test1"));//expecting failiure
        if (res3.ErrorMessage == null)  //failure is expceted as email isnt registered
        {
            Console.WriteLine("Error - non existent user's board was successfuly deleted");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1")); // creating a board to test
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
        }

        this.sf.bs.JoinBoard("oriatta@post.bgu.ac.il", "Test1", "oriattar12@gmail.com");

        res3 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.DeleteBoard("oriattar12@post.bgu.ac.il", "Test1"));
        if (res3.ErrorMessage == null)  //failure is expceted as email isnt registered
        {
            Console.WriteLine("Error - board was deleted by a normal user, not the owner");
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
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
        }

        int limit = 0;
        Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.SetLimit("oriatta@post.bgu.ac.il", "Test1", limit,0));//expecting failiure
        if (res2.ErrorMessage == null)
        {
            Console.WriteLine("ilegal limiting was succefull");
            return false;
        }


        limit = 20;
        res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.SetLimit("oriatta@post.bgu.ac.il", "Test1", limit,0));//expecting success
        if (res2.ErrorMessage != null)
        {
            Console.WriteLine(res2.ErrorMessage);
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
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
        }

        

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test2"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
        }

        

        //creating temp boards
        DateTime deadline = DateTime.Parse("30/12/2025 10:00:00 AM"); // creating temp deadline


        Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask("oriatta@post.bgu.ac.il", "Test", "TestDescription", "Test1", deadline));
        if (res2.ErrorMessage != null)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        

        res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask("oriatta@post.bgu.ac.il", "Test2", "TestDescription2", "Test2", deadline));
        if (res2.ErrorMessage != null)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        //creating test tasks

        Response<string> res3 = JsonSerializer.Deserialize <Response<string>>(this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "Test1",0, 0));
        if (res3.ErrorMessage != null)
        {
            Console.WriteLine(res3.ErrorMessage);
            return false;
        }
        res3 = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "Test2",0, 1));
        if (res3.ErrorMessage != null)
        {
            Console.WriteLine(res3.ErrorMessage);
            return false;
        }
       // advancing test tasks.


        Response<List<TaskSL>> res4 = JsonSerializer.Deserialize <Response<List<TaskSL>>>( this.sf.bs.GetInProgressList("oriatta@post.bgu.ac.il"));// testing the operation
        if (res4.ErrorMessage != null)
        {
            Console.WriteLine(res4.ErrorMessage);
            return false;
        }

        List<TaskSL> tasks = res4.ReturnValue;
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
        DateTime dateTime = DateTime.MaxValue;
        String description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        String title = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        string bname = "test";
        string email = "oriatta@post.bgu.ac.il";


        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, title, description, bname, dateTime));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine("");
            return false;
        }

      //  בדיקת עם שם לוח שלא קיים במערכת
        dateTime = DateTime.MaxValue;
        description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        title = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        bname = "A";
        email = "oriatta@post.bgu.ac.il";

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, title, description, bname, dateTime));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("");
            return false;
        }


    //    בדיקת הוספת משימה ללוח קיים עם מייל שונה
        dateTime = DateTime.MaxValue;
        description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        title = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        bname = "test";
        email = "aaaaaaaggg@gmail.com";
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, title, description, bname, dateTime));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("");
            return false;
        }


    //    בדיקה עם כותרת ריקה
        dateTime = DateTime.MaxValue;
        description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        title = "";
        bname = "test";
        email = "oriatta@post.bgu.ac.il";


        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, title, description, bname, dateTime));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("");
            return false;
        }


    //    בדיקה עם description ריק
        dateTime = DateTime.MaxValue;
        description = "";
        title = "gggggggggg";
        bname = "test";
        email = "oriatta@post.bgu.ac.il";
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, title, description, bname, dateTime));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("");
            return false;
        }

   //     בדיקה עם PastDatetime ריק
        dateTime = DateTime.MaxValue;
        description = "vvvvvvvvvvvv";
        title = "gggggggggg";
        bname = "test";
        email = "oriatta@post.bgu.ac.il";
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, title, description, bname, DateTime.MinValue));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, null, description, bname, dateTime));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, title, null, bname, dateTime));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(email, title, description, null, dateTime));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask(null, title, description, bname, dateTime));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("A null value was successfully assigned");
            return false;
        }


        return true;


    }

    public bool checkBoardJoin()
    {
        this.runBeforeBoardTest();
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1")); // creating a board to test
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.JoinBoard("oriatta@post.bgu.ac.il", "Test1","oriattar12@gmail.com"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.JoinBoard("oriatta@post.bgu.ac.il", "Test1", "a"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("An unregistered user was assigned to a board");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.JoinBoard("oriatta@post.bgu.ac.il", "Test2", "oriattar12@gmail.com"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("User joined non existent board");
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.JoinBoard("oriatta@post.bgu.ac.il", "Test1", "oriattar12@gmail.com"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("User already in board and successfuly joined");
            return false;
        }


        return true;
    }

    public bool checkBoardLeave()
    {

        this.runBeforeBoardTest();
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1")); // creating a board to test
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.JoinBoard("oriatta@post.bgu.ac.il", "Test1", "oriattar12@gmail.com"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.LeaveBoard("oriatta@post.bgu.ac.il", "Test1", "oriattar12@gmail.com"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.LeaveBoard("oriatta@post.bgu.ac.il", "Test1", "oriattar12@gmail.com"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Unassigned user left successfuly");
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.LeaveBoard("oriatta@post.bgu.ac.il", "Test2", "oriattar12@gmail.com"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("non existent board - user left successfuly");
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.LeaveBoard("oriatta@post.bgu.ac.il", "Test1", "oriatta@post.bgu.ac.il"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Owner left successfuly");
            return false;
        }

        return true;
    }

    public bool checkOwnerChange()
    {
        this.runBeforeBoardTest();
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.CreateBoard("oriatta@post.bgu.ac.il", "Test1")); // creating a board to test
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.JoinBoard("oriatta@post.bgu.ac.il", "Test1", "oriattar12@gmail.com"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.ChangeOwner("oriatta@post.bgu.ac.il", "Test1", "oriattar12@gmail.com"));
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.ChangeOwner("oriatta@post.bgu.ac.il", "Test1", "a"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Unassinged user was promoted");
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.ChangeOwner("oriatta@post.bgu.ac.il", "Test2", "oriattar12@gmail.com"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Unassinged board - user was promoted");
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.bs.ChangeOwner("oriattar12@gmail.com", "Test1", "oriattar12@gmail.com"));
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Already Assigned owner was assigned");
            return false;
        }
        return true;
    }





    /// <summary>
    ///Method that tests task editing
    /// </summary>
    /// <returns></returns>
    public bool editTaskTest()
    {
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask("oriatta@post.bgu.ac.il", "TestTask", "Test for editing", "test", DateTime.MaxValue));// creating temp task
        if (res.ErrorMessage != null) 
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }


        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.UpdateTaskDeadline("oriatta@post.bgu.ac.il", "test",0,0,DateTime.MaxValue)); // expecting success
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.UpdateTaskDeadline("oriatta@post.bgu.ac.il", "test", 0, 0, DateTime.MinValue)); // expecting failure
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("invalid deadline was set");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.UpdateTaskTitle("oriatta@post.bgu.ac.il", "test", 0, 0, "NewT"));  //expecting success
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.UpdateTaskTitle("oriatta@post.bgu.ac.il", "test", 0, 0, "")); // expecting failure
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Imvalid title was set");
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.UpdateTaskTitle("oriatta@post.bgu.ac.il", "test", 0, 0, null)); // expecting failure
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Imvalid title was set");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.UpdateTaskDescription("oriatta@post.bgu.ac.il", "test", 0, 0, ""));  //expecting success
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }
        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.UpdateTaskDescription("oriatta@post.bgu.ac.il", "test", 0, 0,"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabcdefffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")); // expecting failure
        if (res.ErrorMessage == null)
        {
            Console.WriteLine("Imvalid desc was set");
            return false;
        }

        return true;
    }

    /// <summary>
    /// A method to test task advancing.
    ///</summary>
    /// <returns></returns>
    public bool testTaskAdvancing()
    {
        Response<string> res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask("oriatta@post.bgu.ac.il", "TestTask", "Test for editing", "test", DateTime.MaxValue)); //creating temp task
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "test", 0,0));//Advancing to on going
        if (res2.ErrorMessage != null)
        {
            Console.WriteLine(res2.ErrorMessage);
            return false;
        }

        res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "test", 1, 0)); //Advancing to done
        if (res2.ErrorMessage != null)
        {
            Console.WriteLine(res2.ErrorMessage);
            return false;
        }

        res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "test", 2, 0)); ;  //expected failure - no where to advance.
        if (res2.ErrorMessage == null)
        {
            Console.WriteLine("A task was advanced beyond 'Done'.");
            return false;
        }

        res = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.CreateTask("oriatta@post.bgu.ac.il", "TestTask", "Test for editing", "test", DateTime.MaxValue)); //creating temp task
        if (res.ErrorMessage != null)
        {
            Console.WriteLine(res.ErrorMessage);
            return false;
        }

        res2 = JsonSerializer.Deserialize<Response<string>>(this.sf.ts.AdvanceTask("oriatta@post.bgu.ac.il", "test",0, -1)); //trying to advance non existant task
        if (res2.ErrorMessage == null)
        {
            Console.WriteLine(res2.ErrorMessage);
            return false;
        }

        return true;
    }

}



   
