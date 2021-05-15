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
    public class utServiceTypeController
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

            HttpResponseMessage response = client.GetAsync("ServiceType").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<ServiceType> serviceTypes = items.ToObject<List<ServiceType>>();

            Assert.IsTrue(serviceTypes.Count > 0);
        }

        [TestMethod]
        public void GetByServiceTypeIdTest()
        {
            InitializeClient();

            // Get an exisiting ServiceTypeId
            HttpResponseMessage response = client.GetAsync("ServiceType").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<ServiceType> serviceTypes = items.ToObject<List<ServiceType>>();

            var getResponse = client.GetAsync("ServiceType/" + serviceTypes[0].Id).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;
            ServiceType serviceType = JsonConvert.DeserializeObject<ServiceType>(getResult);

            Assert.IsTrue(serviceType != null);
        }


        [TestMethod]
        public void PostTest()
        {
            InitializeClient();

            ServiceType serviceType = new ServiceType
            {
                Description = "Rake Leaves",
                CostPerSQFT = 0.004M
            };

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(serviceType);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var insertResponse = client.PostAsync("ServiceType/" + rollback, content).Result;

            // returns the guid Id of the inserted record
            var insertResult = insertResponse.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(!string.IsNullOrEmpty(insertResult));
        }

        [TestMethod]
        public void PutTest()
        {
            InitializeClient();

            // Get an exisiting serviceType record to update
            HttpResponseMessage response = client.GetAsync("ServiceType").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<ServiceType> serviceTypes = items.ToObject<List<ServiceType>>();

            ServiceType serviceType = serviceTypes[0];
            serviceType.CostPerSQFT = .999m;

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(serviceType);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var updateResponse = client.PutAsync("ServiceType/" + rollback, content).Result;
            var updateResult = updateResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(updateResult == "1");
        }

        [TestMethod]
        public void DeleteTest()
        {
            InitializeClient();

            // Get an exisiting serviceType record to delete
            HttpResponseMessage response = client.GetAsync("ServiceType").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<ServiceType> serviceTypes = items.ToObject<List<ServiceType>>();

            bool rollback = true;
            var deleteResponse = client.DeleteAsync("ServiceType/" + serviceTypes[0].Id + "/" + rollback).Result;
            var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(deleteResult == "1");
        }
    }
}
