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
    public class utAppointmentController
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

            HttpResponseMessage response = client.GetAsync("Appointment").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Appointment> appointments = items.ToObject<List<Appointment>>();

            Assert.IsTrue(appointments.Count > 0);
        }

        [TestMethod]
        public void GetByAppointmentIdTest()
        {
            InitializeClient();

            // Get an exisiting AppointmentId
            HttpResponseMessage response = client.GetAsync("Appointment").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Appointment> appointments = items.ToObject<List<Appointment>>();

            var getResponse = client.GetAsync("Appointment/byId/" + appointments[0].Id).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;
            Appointment appointment = JsonConvert.DeserializeObject<Appointment>(getResult);

            Assert.IsTrue(appointment != null);
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
            Customer customer = customers.FirstOrDefault(c=>c.LastName=="Smith");

            var getResponse = client.GetAsync("Appointment/byCustomerId/" + customer.Id).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;

            List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(getResult);

            Assert.IsTrue(appointments.Count > 0);
        }

        [TestMethod]
        public void GetByEmployeeIdTest()
        {
            InitializeClient();

            // Get an exisiting CustomerId
            HttpResponseMessage response = client.GetAsync("Employee").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Employee> employees = items.ToObject<List<Employee>>();

            var getResponse = client.GetAsync("Appointment/byEmployeeId/" + employees[0].Id).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;

            List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(getResult);

            Assert.IsTrue(appointments.Count > 0);
        }

        [TestMethod]
        public void PostTest()
        {
            InitializeClient();

            // Get an exisiting CustomerId
            HttpResponseMessage customerResponse = client.GetAsync("Customer").Result;
            string customerResult = customerResponse.Content.ReadAsStringAsync().Result;
            dynamic customerItems = (JArray)JsonConvert.DeserializeObject(customerResult);
            List<Customer> customers = customerItems.ToObject<List<Customer>>();

            // Get an exisiting EmployeeId
            HttpResponseMessage employeeResponse = client.GetAsync("Customer").Result;
            string employeeResult = employeeResponse.Content.ReadAsStringAsync().Result;
            dynamic employeeItems = (JArray)JsonConvert.DeserializeObject(employeeResult);
            List<Employee> employees = employeeItems.ToObject<List<Employee>>();

            // Get an exisiting ServiceTypeId
            HttpResponseMessage serviceTypeResponse = client.GetAsync("Customer").Result;
            string serviceTypeResult = serviceTypeResponse.Content.ReadAsStringAsync().Result;
            dynamic serviceTypeItems = (JArray)JsonConvert.DeserializeObject(serviceTypeResult);
            List<ServiceType> serviceTypes = serviceTypeItems.ToObject<List<ServiceType>>();


            Appointment appointment = new Appointment
            {
                CustomerId = customers[0].Id,
                EmployeeId = employees[0].Id,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddHours(2),
                ServiceId = serviceTypes[0].Id,
                Status = "Scheduled"
            };

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(appointment);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var insertResponse = client.PostAsync("Appointment/" + rollback, content).Result;

            // returns the guid Id of the inserted record
            var insertResult = insertResponse.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(!string.IsNullOrEmpty(insertResult));
        }

        [TestMethod]
        public void PutTest()
        {
            InitializeClient();

            // Get an exisiting Appointment record to update
            HttpResponseMessage response = client.GetAsync("Appointment").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Appointment> appointments = items.ToObject<List<Appointment>>();

            Appointment appointment = appointments[0];
            appointment.Status = "Cancelled";

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(appointment);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var updateResponse = client.PutAsync("Appointment/" + rollback, content).Result;
            var updateResult = updateResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(updateResult == "1");
        }

        [TestMethod]
        public void DeleteTest()
        {
            InitializeClient();

            // Get an exisiting appointment record to delete
            HttpResponseMessage response = client.GetAsync("Appointment").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<Appointment> appointments = items.ToObject<List<Appointment>>();

            bool rollback = true;
            var deleteResponse = client.DeleteAsync("Appointment/" + appointments[0].Id + "/" + rollback).Result;
            var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(deleteResult == "1");
        }


    }
}
