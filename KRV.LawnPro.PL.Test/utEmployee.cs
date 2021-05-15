using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KRV.LawnPro.PL.Test
{
    [TestClass]
    public class utEmployee
    {
        Guid employeeId;

        //Set up a transaction
        protected LawnProEntities dc;
        protected IDbContextTransaction transaction;

        [TestInitialize]
        public void TestInitialize()
        {
            dc = new LawnProEntities();
            transaction = dc.Database.BeginTransaction();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            dc = null;
        }

        [TestMethod]
        public void LoadTest()
        {
            int expected = 3;
            int actual = 0;

            actual = dc.tblEmployees.Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InsertTest()
        {
            int expected = 1;
            int actual = 0;

            Guid userId = dc.tblUsers.First().Id;

            tblEmployee newEmployee = new tblEmployee
            {
                Id = Guid.NewGuid(),
                City = "Appleton",
                StreetAddress = "123 Easy Street",
                Email = "Test@test.com",
                FirstName = "Bill",
                LastName = "Jenkins",
                Phone = "123-456-7890",
                State = "WI",
                UserId = userId,
                ZipCode = "54901",
            };

            dc.tblEmployees.Add(newEmployee);
            actual = dc.SaveChanges();

            employeeId = newEmployee.Id;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();
            int expected = 1;
            int actual = 0;

            tblEmployee updateRow = dc.tblEmployees.Where(a => a.Id == employeeId).FirstOrDefault();

            if (updateRow != null)
            {
                updateRow.Email = "UpdatedTest@test.com";
                actual = dc.SaveChanges();
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeleteTest()
        {
            InsertTest();
            int expected = 1;
            int actual = 0;

            tblEmployee deleteRow = dc.tblEmployees.Where(a => a.Id == employeeId).FirstOrDefault();

            if (deleteRow != null)
            {
                dc.tblEmployees.Remove(deleteRow);
                actual = dc.SaveChanges();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
