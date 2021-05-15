using KRV.LawnPro.BL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRV.LawnPro.BL.Tests
{
    [TestClass]
    public class utUser
    {


        [TestInitialize]
        public void Intialize()
        {
            UserManager.Seed();
        }


        [TestMethod]
        public static void LoadTest()
        {
            var task = UserManager.Load();
            List<User> users = task.Result;
            task.Wait();

            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void InsertTest()
        {
            User user = new User { FirstName = "Jane", LastName = "Doe", UserName = "jdoe", UserPass = "1234" };

            var task = UserManager.Insert(user, true);
            bool result = task.Result;
            task.Wait();

            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void LoginSuccess()
        {
            User user = new User { FirstName = "Brian", LastName = "Foote", UserName = "bfoote", UserPass = "maple" };
            
            var task = UserManager.Login(user);
            user = task.Result;
            task.Wait();

            Assert.IsTrue(user.Id != Guid.Empty);
        }

        [TestMethod]
        public void LoginFail()
        {
            User user = new User { FirstName = "Brian", LastName = "Foote", UserName = "bfoote", UserPass = "xxxx" };
            try
            {
                var task = UserManager.Login(user);
                user = task.Result;
                task.Wait();
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            // Insert a user that is not tied to any customer or employee 
            User user = new User
            {
                FirstName = "Jane",
                LastName = "Doe",
                UserName = "jdoe",
                UserPass = "1234"
            };

            var task = UserManager.Insert(user);
            task.Wait();

            var task2 = UserManager.Delete(user.Id);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);

        }



    }
}
