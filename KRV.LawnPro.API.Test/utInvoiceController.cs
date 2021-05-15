using KRV.LawnPro.BL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace KRV.LawnPro.API.Test
{
    [TestClass]
    public class utInvoiceController
    {
        HttpClient client;

        private void InitializeClient()
        {
            var server = new TestServer(new WebHostBuilder().UseEnvironment("Development").UseStartup<Startup>());
            client = server.CreateClient();
        }

        [TestMethod]
        public void GetTest()
        {
            InitializeClient();

            HttpResponseMessage response = client.GetAsync("Invoice").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Invoice> invoices = items.ToObject<List<Invoice>>();

            Assert.IsTrue(invoices.Count > 0);
        }

        [TestMethod]
        public void GetByInvoiceIdTest()
        {
            InitializeClient();

            // Get an exisiting InvoiceId
            HttpResponseMessage response = client.GetAsync("Invoice").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Invoice> invoices = items.ToObject<List<Invoice>>();

            response = client.GetAsync("Invoice/byId/" + invoices[0].Id).Result;
            result = response.Content.ReadAsStringAsync().Result;
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(result);

            Assert.IsTrue(invoice != null);
        }



        [TestMethod]
        public void GetByCustomerIdTest()
        {
            InitializeClient();

            // Get an exisiting CustomerId
            HttpResponseMessage response = client.GetAsync("Customer").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Customer> customers = items.ToObject<List<Customer>>();
            Customer customer = customers.FirstOrDefault(c => c.LastName == "Smith");

            var getResponse = client.GetAsync("Invoice/byCustomerId/" + customer.Id).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(getResult);

            Assert.IsTrue(invoices.Count > 0);
        }

        [TestMethod]
        public void PostTest()
        {
            InitializeClient();

            // Get an exisiting CustomerId
            HttpResponseMessage response = client.GetAsync("Customer").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Customer> customers = items.ToObject<List<Customer>>();

            Invoice invoice = new Invoice
            {
                CustomerId = customers[0].Id,
                ServiceDate = DateTime.Now,
                EmployeeFullName = "Johnny",
                ServiceType = "Mow",
                ServiceRate = 0.0015M,
                Status = "Paid"
            };

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(invoice);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var insertResponse = client.PostAsync("Invoice/" + rollback, content).Result;

            // returns the guid Id of the inserted record
            var insertResult = insertResponse.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(!string.IsNullOrEmpty(insertResult));
        }

        [TestMethod]
        public void PutTest()
        {
            InitializeClient();

            // Get an exisiting invoice record to update
            HttpResponseMessage response = client.GetAsync("Invoice").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Invoice> invoices = items.ToObject<List<Invoice>>();

            Invoice invoice = invoices[0];
            invoice.ServiceRate = 0.055m;

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(invoice);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var updateResponse = client.PutAsync("Invoice/" + rollback, content).Result;
            var updateResult = updateResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(updateResult == "1");
        }

        [TestMethod]
        public void DeleteTest()
        {
            InitializeClient();

            // Get an exisiting invoice record to delete
            HttpResponseMessage response = client.GetAsync("Invoice").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Invoice> invoices = items.ToObject<List<Invoice>>();

            bool rollback = true;
            var deleteResponse = client.DeleteAsync("Invoice/" + invoices[0].Id + "/" + rollback).Result;
            var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(deleteResult == "1");
        }

    }
}
