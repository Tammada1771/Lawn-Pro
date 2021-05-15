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
    public class utEmployee
    {
        [TestMethod]
        public void LoadTest()
        {
            int expected = 3;

            var task = EmployeeManager.Load();
            List<Employee> employees = task.Result;
            task.Wait();

            Assert.AreEqual(expected, employees.Count);
        }

        [TestMethod]
        public void InsertTest()
        {
            var task = UserManager.Load();
            User user = task.Result.FirstOrDefault();
            task.Wait();

            Employee employee = new Employee
            {
                FirstName = "XXXXX",
                LastName = "XXXXX",
                StreetAddress = "XXXXX",
                City = "XXXXX",
                State = "XX",
                ZipCode = "XXXXX",
                Email = "XXXXXX",
                Phone = "XXX-XXX-XXXX",
                UserId = user.Id
            };

            var task2 = EmployeeManager.Insert(employee, true);
            bool result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var task = EmployeeManager.Load();
            Employee employee = task.Result.FirstOrDefault();
            task.Wait();

            employee.FirstName = "Blah blah";

            var task2 = EmployeeManager.Update(employee, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = EmployeeManager.Load();
            Employee employee = task.Result.FirstOrDefault();
            task.Wait();

            var task2 = EmployeeManager.Delete(employee.Id, true);
            int result = task2.Result;
            task2.Wait(0);

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void LoadByIdTest()
        {
            var task = EmployeeManager.Load();
            Employee employee = task.Result.FirstOrDefault();
            task.Wait();

            var task2 = EmployeeManager.LoadById(employee.Id);
            Employee employee2 = task2.Result;
            task2.Wait();

            Assert.AreEqual(employee2.Id, employee.Id);
        }

    }
}
