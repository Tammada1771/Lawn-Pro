using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KRV.LawnPro.PL.Test
{
    [TestClass]
    public class utCustomer
    {
        Guid customerId;

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
            Assert.IsTrue(dc.tblCustomers.Count() > 0);
        }

        [TestMethod]
        public void InsertTest()
        {
            int expected = 1;
            int actual = 0;

            Guid userId = dc.tblUsers.First().Id;

            tblCustomer newCustomer = new tblCustomer
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
                PropertySqFt = 50000
            };

            dc.tblCustomers.Add(newCustomer);
            actual = dc.SaveChanges();

            customerId = newCustomer.Id;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();
            int expected = 1;
            int actual = 0;

            tblCustomer updateRow = dc.tblCustomers.Where(a => a.Id == customerId).FirstOrDefault();

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

            tblCustomer deleteRow = dc.tblCustomers.Where(a => a.Id == customerId).FirstOrDefault();

            if (deleteRow != null)
            {
                dc.tblCustomers.Remove(deleteRow);
                actual = dc.SaveChanges();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
