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
    public class utEmployeeController
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

            HttpResponseMessage response = client.GetAsync("Employee").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Employee> employees = items.ToObject<List<Employee>>();

            Assert.IsTrue(employees.Count > 0);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            InitializeClient();

            // Get an exisiting EmployeeId
            HttpResponseMessage response = client.GetAsync("Employee").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Employee> employees = items.ToObject<List<Employee>>();

            var getResponse = client.GetAsync("Employee/byId/" + employees[0].Id).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;
            Employee employee = JsonConvert.DeserializeObject<Employee>(getResult);

            Assert.IsTrue(employee != null);
        }

        [TestMethod]
        public void GetByUserIdTest()
        {
            InitializeClient();

            // Get an exisiting EmployeeId
            HttpResponseMessage response = client.GetAsync("Employee").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Employee> employees = items.ToObject<List<Employee>>();

            var getResponse = client.GetAsync("Employee/byUserId/" + employees[0].UserId).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;
            Employee employee = JsonConvert.DeserializeObject<Employee>(getResult);

            Assert.IsTrue(employee != null);
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
                UserId = users[0].Id
            };

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(employee);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var insertResponse = client.PostAsync("Employee/" + rollback, content).Result;

            // returns the guid Id of the inserted record
            var insertResult = insertResponse.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(!string.IsNullOrEmpty(insertResult));
        }

        [TestMethod]
        public void PutTest()
        {
            InitializeClient();

            // Get an exisiting employee record to update
            HttpResponseMessage response = client.GetAsync("Employee").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Employee> employees = items.ToObject<List<Employee>>();

            Employee employee = employees[0];
            employee.FirstName = "Blah blah blah.";

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(employee);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var updateResponse = client.PutAsync("Employee/" + rollback, content).Result;
            var updateResult = updateResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(updateResult == "1");
        }

        [TestMethod]
        public void DeleteTest()
        {
            InitializeClient();

            // Get an exisiting employee record to delete
            HttpResponseMessage response = client.GetAsync("Employee").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Employee> employees = items.ToObject<List<Employee>>();

            bool rollback = true;
            var deleteResponse = client.DeleteAsync("Employee/" + employees[0].Id + "/" + rollback).Result;
            var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(deleteResult == "1");
        }
    }
}
