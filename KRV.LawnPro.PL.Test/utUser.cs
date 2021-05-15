using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KRV.LawnPro.PL.Test
{
    [TestClass]
    public class utUser
    {
        Guid userId;

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
             Assert.IsTrue(dc.tblUsers.Count() > 0);
        }

        [TestMethod]
        public void InsertTest()
        {
            int expected = 1;
            int actual = 0;

            tblUser newUser = new tblUser
            {
                Id = Guid.NewGuid(),
                UserName = "TestUser",
                UserPass = "TestPass",
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };

            dc.tblUsers.Add(newUser);
            actual = dc.SaveChanges();

            userId = newUser.Id;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();
            int expected = 1;
            int actual = 0;

            tblUser updateRow = dc.tblUsers.Where(a => a.Id == userId).FirstOrDefault();

            if (updateRow != null)
            {
                updateRow.UserPass = "NewTestPass";
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

            tblUser deleteRow = dc.tblUsers.Where(a => a.Id == userId).FirstOrDefault();

            if (deleteRow != null)
            {
                dc.tblUsers.Remove(deleteRow);
                actual = dc.SaveChanges();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
