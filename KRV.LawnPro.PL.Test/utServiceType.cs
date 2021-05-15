using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KRV.LawnPro.PL.Test
{
    [TestClass]
    public class utServiceType
    {
        Guid servicetypeId;

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

            actual = dc.tblServiceTypes.Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InsertTest()
        {
            int expected = 1;
            int actual = 0;

            tblServiceType newServiceType = new tblServiceType
            {
                Id = Guid.NewGuid(),
                Description = "Rake Leaves",
                CostPerSqFt = 0.0045M
            };

            dc.tblServiceTypes.Add(newServiceType);
            actual = dc.SaveChanges();

            servicetypeId = newServiceType.Id;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();
            int expected = 1;
            int actual = 0;

            tblServiceType updateRow = dc.tblServiceTypes.Where(a => a.Id == servicetypeId).FirstOrDefault();

            if (updateRow != null)
            {
                updateRow.CostPerSqFt = 0.0055M;
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

            tblServiceType deleteRow = dc.tblServiceTypes.Where(a => a.Id == servicetypeId).FirstOrDefault();

            if (deleteRow != null)
            {
                dc.tblServiceTypes.Remove(deleteRow);
                actual = dc.SaveChanges();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
