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
    public class utInvoice
    {
        [TestMethod]
        public void LoadTest()
        {
            var task = InvoiceManager.Load();
            List<Invoice> invoices = task.Result;
            task.Wait();

            Assert.IsTrue(invoices.Count > 0);
        }

        [TestMethod]
        public void LoadByIdTest()
        {
            var task = InvoiceManager.Load();
            List<Invoice> invoices = task.Result;
            task.Wait();

            Guid id = invoices.FirstOrDefault().Id;

            var task2 = InvoiceManager.LoadById(id);
            Invoice invoice = task2.Result;
            task2.Wait();

            Assert.IsTrue(invoice.Id == id);
        }

        [TestMethod]
        public void LoadByCustomerIdTest()
        {
            var task = InvoiceManager.Load();
            List<Invoice> invoices = task.Result;
            task.Wait();

            Guid id = invoices.FirstOrDefault().CustomerId;

            var task2 = InvoiceManager.LoadByCustomerId(id);

            List<Invoice> invoice = task2.Result;
            task2.Wait();

            Invoice inv = invoice.FirstOrDefault();

            Assert.IsTrue(inv.CustomerId == id);
        }


        [TestMethod]
        public void InsertTest()
        {
            var task = CustomerManager.Load();
            Customer customer = task.Result.FirstOrDefault();
            task.Wait();

            Invoice invoice = new Invoice
            {
                CustomerId = customer.Id,
                ServiceDate = DateTime.Now,
                EmployeeFullName = "Johnny",
                ServiceType = "Mow",
                ServiceRate = 0.0015M,
                Status = InvoiceStatus.Paid.ToString()
            };

            var task2 = InvoiceManager.Insert(invoice, true);
            bool result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var task = InvoiceManager.Load();
            Invoice invoice = task.Result.FirstOrDefault();
            task.Wait();

            invoice.Status = "Test";

            var task2 = InvoiceManager.Update(invoice, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = InvoiceManager.Load();
            Invoice invoice = task.Result.FirstOrDefault();
            task.Wait();

            var task2 = InvoiceManager.Delete(invoice.Id, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }
    }
}
