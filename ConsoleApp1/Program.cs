
using System.Reflection;
using System.Runtime.CompilerServices;

using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.Buiesness_layer;

public class Test
{
    private ServiceFactory sf;

    public Test()
    {
        this.sf = new ServiceFactory();

    }


    /// <summary>
    /// This function tests Requirement 7 for successful registration.
    /// It checks various invalid registration scenarios, such as null values, password length constraints, and password character requirements.
    /// The function also checks if valid registrations succeed, including checking for duplicate emails and invalid email formats.
    /// </summary>
    private bool TestSuccessfullRegister()
    {
        Response res = this.sf.Register(null, null);
        if (res.ErrorMsg == null)
        {
            return false;
            Console.WriteLine("❌ Test 1 Failed: Email and password cannot be null.");
        }

        res = this.sf.Register("aaa@gmail.com", null);
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 2 Failed: Password cannot be null.");
            return false;
        }

        res = this.sf.Register("bbb@gmail.com", "Aa7");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 3 Failed: Password too short.");
            return false;
        }

        res = this.sf.Register("ccc@gmail.com", "aaaaaaaAAAAAAAAAAAAAAA77");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 4 Failed: Password too long or violates max length.");
            return false;
        }

        res = this.sf.Register("ddd@gmail.com", "aaaaaaaaaa7");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 5 Failed: Missing uppercase letters in password.");
            return false;
        }


        res = this.sf.Register("eee@gmail.com", "AAAAAAAAAAA7");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 6 Failed: Missing lowercase letters in password.");
            return false;
        }


        res = this.sf.Register("fff@gmail.com", "AAAAAAaaaaa");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 7 Failed: Missing digit in password.");
            return false;
        }


        res = this.sf.Register("ggg@gmail.com", "aScA78f");
        if (res.ErrorMsg != null)
        {
            Console.WriteLine("❌ Test 8 Failed: Valid password should have succeeded.");
            return false;
        }

        res = this.sf.Register("ggg@gmail.com", "aScA78f");
        if (res.ErrorMsg != null)
        {
            Console.WriteLine("❌ Test 9 Failed: Valid password should have succeeded.");
            return false;
        }

        res = this.sf.Register("rrr@gmail.com", "AAAA555aaaa");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 10 Failed: Duplicate email should have failed.");
            return false;
        }


        res = this.sf.Register("rrrm", "AAAA555aaaa");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 11 Failed: Invalid email format should have failed (missing '@' and domain).");
            return false;
        }



        res = this.sf.Register("abcs@gmail.com", "AAAA5aaaa");
        if (res.returnValue == null)
        {
            Console.WriteLine("❌ Test 12 Failed: should have succeeded and return userSL");
            return false;
        }
        if (!res.returnValue.email.Equals("abcs@gmail.com"))
        {
            Console.WriteLine("❌ Test 13 Failed: return wrong userSL email");
            return false;
        }
        if (res.returnValue.boards.Count != 0)
        {
            Console.WriteLine("❌ Test 14 Failed: Did not set the board number to 0 ");
            return false;
        }


    }

    /// <summary>
    /// This function tests the login functionality after successful registration.
    /// It checks if users can log in with the correct credentials and if login fails with incorrect credentials.
    /// Additionally, it checks if duplicate registrations for the same email are handled correctly.
    /// </summary>

    private bool TestSuccessfulllogin()
    {
        
        res = this.sf.Register("afffffbcs@gmail.com", "AAAAgd5aa");
        if (this.sf.Login("afffffbcs@gmail.com", "AAAAgd5aa") == false)
        {
            Console.WriteLine("❌ Test 15 Failed: should have succeeded and return userSL");
            return false;
        }


        if (this.sf.Login("afffffbcs@gmail.com", "AAAffffAgd5aa") == true)
        {
            Console.WriteLine("❌ Test 16 Failed: should have failed, password not correct ");
            return false;
        }

        if (this.sf.Login("aooocs@gmail.com", "AAAAgd5aa") == true)
        {
            Console.WriteLine("❌ Test 17 Failed: should have failed, email not correct ");
            return false;
        }



        res = this.sf.Register("rrxxxr@gmail.com", "AAAA555aaaa");
        res = this.sf.Register("rrxxxr@gmail.com", "AAAA555aaaa");
        if (res.ErrorMsg == null)
        {
            Console.WriteLine("❌ Test 18 Failed: already register");
            return false;
        }


    }

}

   
