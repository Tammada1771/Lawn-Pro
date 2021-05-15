using KRV.LawnPro.BL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace KRV.LawnPro.API.Test
{
    [TestClass]
    public class utCustomerController
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

            HttpResponseMessage response = client.GetAsync("Customer").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Customer> customers = items.ToObject<List<Customer>>();

            Assert.IsTrue(customers.Count > 0);
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

            var getResponse = client.GetAsync("Customer/byId/" + customers[0].Id).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;
            Customer customer = JsonConvert.DeserializeObject<Customer>(getResult);

            Assert.IsTrue(customer != null);
        }


        [TestMethod]
        public void PostTest()
        {
            InitializeClient();

            // Get an exisiting UserId
            HttpResponseMessage response = client.GetAsync("User").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<User> users = items.ToObject<List<User>>();

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
                UserId = users[0].Id
            };

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(customer);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var insertResponse = client.PostAsync("Customer/" + rollback, content).Result;

            // returns the guid Id of the inserted record
            var insertResult = insertResponse.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(!string.IsNullOrEmpty(insertResult));
        }

        [TestMethod]
        public void PutTest()
        {
            InitializeClient();

            // Get an exisiting Customer record to update
            HttpResponseMessage response = client.GetAsync("Customer").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Customer> customers = items.ToObject<List<Customer>>();

            Customer customer = customers[0];
            customer.FirstName = "Blah blah blah.";

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(customer);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var updateResponse = client.PutAsync("Customer/" + rollback, content).Result;
            var updateResult = updateResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(updateResult == "1");
        }

        [TestMethod]
        public void DeleteTest()
        {
            InitializeClient();

            // Get an exisiting customer record to delete
            HttpResponseMessage response = client.GetAsync("Customer").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Customer> customers = items.ToObject<List<Customer>>();

            bool rollback = true;
            var deleteResponse = client.DeleteAsync("Customer/" + customers[0].Id + "/" + rollback).Result;
            var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(deleteResult == "1");
        }


    }
}
