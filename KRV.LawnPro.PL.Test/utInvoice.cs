using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KRV.LawnPro.PL.Test
{
    [TestClass]
    public class utInvoice
    {
        Guid invoiceId;

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
        public void GetCustomerBalanceTest()
        {

            tblInvoice invoice = dc.tblInvoices.Where(i => i.Status == "Issued").FirstOrDefault();
            tblCustomer customer = dc.tblCustomers.Where(c => c.Id == invoice.CustomerId).FirstOrDefault();

            decimal expected = invoice.ServiceRate * customer.PropertySqFt;
            decimal actual = 0;


            var customerId = new SqlParameter
            {
                ParameterName = "customerId",
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                Value = customer.Id
            };

            var results = dc.Set<spGetCustomerBalanceResult>().FromSqlRaw("exec spGetCustomerBalance @customerId", customerId).ToList();

            foreach (var r in results)
            {
                actual = r.Balance;
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LoadTest()
        {
            int actual = 0;

            actual = dc.tblInvoices.Count();

            Assert.IsTrue(actual > 0);
        }

        [TestMethod]
        public void InsertTest()
        {
            int expected = 1;
            int actual = 0;

            Guid customerId = dc.tblCustomers.First().Id;
            string serviceName = dc.tblServiceTypes.First().Description;
            decimal serviceRate = dc.tblServiceTypes.First().CostPerSqFt;
            string employeeName = dc.tblEmployees.First().FirstName + " " + dc.tblEmployees.First().LastName;

            tblInvoice newInvoice = new tblInvoice
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                EmployeeName = employeeName,
                ServiceName = serviceName,
                ServiceRate = serviceRate,
                ServiceDate = DateTime.Now,
                Status = "Completed"
            };

            dc.tblInvoices.Add(newInvoice);
            actual = dc.SaveChanges();

            invoiceId = newInvoice.Id;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();
            int expected = 1;
            int actual = 0;

            tblInvoice updateRow = dc.tblInvoices.Where(a => a.Id == invoiceId).FirstOrDefault();

            if (updateRow != null)
            {
                updateRow.Status = "Paid";
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

            tblInvoice deleteRow = dc.tblInvoices.Where(a => a.Id == invoiceId).FirstOrDefault();

            if (deleteRow != null)
            {
                dc.tblInvoices.Remove(deleteRow);
                actual = dc.SaveChanges();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
