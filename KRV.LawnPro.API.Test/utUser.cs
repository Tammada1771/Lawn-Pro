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
    public class utUserController
    {
        HttpClient client;

        private void InitializeClient()
        {
            var server = new TestServer(new WebHostBuilder().UseEnvironment("Development").UseStartup<Startup>());
            client = server.CreateClient();

            // Seed the user passords
            HttpResponseMessage response = client.GetAsync("User/Seed").Result;
            string result = response.Content.ReadAsStringAsync().Result;

        }

        [TestMethod]
        public void GetTest()
        {
            InitializeClient();

            HttpResponseMessage response = client.GetAsync("User").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<User> users = items.ToObject<List<User>>();

            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void GetByUserIdTest()
        {
            InitializeClient();

            // Get an exisiting UserId
            HttpResponseMessage response = client.GetAsync("User").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<User> users = items.ToObject<List<User>>();

            var getResponse = client.GetAsync("User/" + users[0].Id).Result;
            var getResult = getResponse.Content.ReadAsStringAsync().Result;
            User user = JsonConvert.DeserializeObject<User>(getResult);

            Assert.IsTrue(user != null);
        }

        [TestMethod]
        public void LoginSuccessTest()
        {
            InitializeClient();

            User user = new User
            {
                FirstName = "unknown",
                LastName = "unknown",
                UserName = "kvicchiollo",
                UserPass = "password",
                UserPass2 = "password"
            };
            string serializedObject = JsonConvert.SerializeObject(user);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var loginResponse = client.PostAsync("User/Login/", content).Result;
            var loginResult = loginResponse.Content.ReadAsStringAsync().Result;
            dynamic item = JsonConvert.DeserializeObject(loginResult);
            user = item.ToObject<User>();

            // Got a fully populated user back
            Assert.IsTrue(user.Id != Guid.Empty);
        }

        [TestMethod]
        public void LoginFailTest()
        {
            InitializeClient();

            User user = new User
            {
                FirstName = "unknown",
                LastName = "unknown",
                UserName = "kvicchiollo",
                UserPass = "wrongpassword",
                UserPass2 = "wrongpassword"
            };
            try
            {
                string serializedObject = JsonConvert.SerializeObject(user);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var loginResponse = client.PostAsync("User/Login/", content).Result;
                var loginResult = loginResponse.Content.ReadAsStringAsync().Result;
                dynamic item = JsonConvert.DeserializeObject(loginResult);
                user = item.ToObject<User>();

                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }


        [TestMethod]
        public void PostTest()
        {
            InitializeClient();

            User user = new User { FirstName = "Jane", LastName = "Doe", UserName = "jdoe", UserPass = "1234", UserPass2 = "1234" };

            bool rollback = true;
            string serializedObject = JsonConvert.SerializeObject(user);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var insertResponse = client.PostAsync("User/" + rollback, content).Result;

            // returns the guid Id of the inserted record
            var insertResult = insertResponse.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(!string.IsNullOrEmpty(insertResult));
        }

        [TestMethod]
        public void PutTest()
        {
            InitializeClient();

            // Get an exisiting user record to update
            HttpResponseMessage response = client.GetAsync("User").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            List<User> users = items.ToObject<List<User>>();

            User user = users[0];
            user.FirstName = "XXXXX";
            user.UserPass2 = user.UserPass;

            bool rollback = true;
            bool nameonly = false;
            string serializedObject = JsonConvert.SerializeObject(user);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var updateResponse = client.PutAsync("User/"+ nameonly + "/" + rollback, content).Result;
            var updateResult = updateResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(updateResult == "1");
        }

        [TestMethod]
        public void DeleteTest()
        {
            InitializeClient();

            // Insert a user that is not tied to any customer or employee 
            User user = new User { FirstName = "Jane", LastName = "Doe", UserName = "jdoe", UserPass = "1234", UserPass2 = "1234" };

            bool rollback = false;
            string serializedObject = JsonConvert.SerializeObject(user);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var insertResponse = client.PostAsync("User/" + rollback, content).Result;
            var insertResult = insertResponse.Content.ReadAsStringAsync().Result;

            Guid id = Guid.Parse(insertResult.Replace("\"", ""));
            var deleteResponse = client.DeleteAsync("User/" + id).Result;
            var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;

            Assert.IsTrue(deleteResult == "1");
        }
    }
}
