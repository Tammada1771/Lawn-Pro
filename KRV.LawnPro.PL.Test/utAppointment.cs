using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KRV.LawnPro.PL.Test
{
    [TestClass]
    public class utAppointment
    {
        Guid appointmentId;

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
            int actual = dc.tblAppointments.Count();

            Assert.IsTrue(actual > 0);
        }

        [TestMethod]
        public void InsertTest()
        {
            int expected = 1;
            int actual = 0;

            Guid customerId = dc.tblCustomers.First().Id;
            Guid employeeId = dc.tblEmployees.First().Id;
            Guid serviceId = dc.tblServiceTypes.First().Id;

            tblAppointment newAppointment = new tblAppointment
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                EmployeeId = employeeId,
                ServiceId = serviceId,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddHours(4),
                Status = "Pending"
            };

            dc.tblAppointments.Add(newAppointment);
            actual = dc.SaveChanges();

            appointmentId = newAppointment.Id;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();
            int expected = 1;
            int actual = 0;

            tblAppointment updateRow = dc.tblAppointments.Where(a => a.Id == appointmentId).FirstOrDefault();

            if (updateRow != null)
            {
                updateRow.Status = "In Progress";
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

            tblAppointment deleteRow = dc.tblAppointments.Where(a => a.Id == appointmentId).FirstOrDefault();

            if (deleteRow != null)
            {
                dc.tblAppointments.Remove(deleteRow);
                actual = dc.SaveChanges();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
