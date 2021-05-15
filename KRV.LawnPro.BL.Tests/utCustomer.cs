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
    public class utCustomer
    {
        [TestMethod]
        public void LoadTest()
        {
            var task = CustomerManager.Load();
            List<Customer> customers = task.Result;
            task.Wait();

            Assert.IsTrue(customers.Count > 0);
        }

        [TestMethod]
        public void InsertTest()
        {
            var task = UserManager.Load();
            User user = task.Result.FirstOrDefault();
            task.Wait();

            Customer customer = new Customer
            {
                FirstName = "XXXXX",
                LastName = "XXXXX",
                StreetAddress = "XXXXX",
                City = "XXXXX",
                State = "XX",
                ZipCode = "XXXXX",
                Email = "XXXXXX",
                PropertySqFeet = 99,
                Phone = "XXX-XXX-XXXX",
                UserId = user.Id
            };

            var task2 = CustomerManager.Insert(customer, true);
            bool result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var task = CustomerManager.Load();
            Customer customer = task.Result.FirstOrDefault();
            task.Wait();

            customer.FirstName = "Blah blah";

            var task2 = CustomerManager.Update(customer, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = CustomerManager.Load();
            Customer customer = task.Result.FirstOrDefault();
            task.Wait();

            var task2 = CustomerManager.Delete(customer.Id, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void LoadByIdTest()
        {
            var task = CustomerManager.Load();
            Customer customer = task.Result.FirstOrDefault();
            task.Wait();

            var task2 = CustomerManager.LoadById(customer.Id);
            Customer customer2 = task2.Result;
            task2.Wait();

            Assert.AreEqual(customer2.Id, customer.Id);
        }

    }
}
