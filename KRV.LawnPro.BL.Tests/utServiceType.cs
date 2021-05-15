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
    public class utServiceType
    {
        [TestMethod]
        public void LoadTest()
        {
            int expected = 3;

            var task = ServiceTypeManager.Load();
            List<ServiceType> serviceTypes = task.Result;
            task.Wait();

            Assert.AreEqual(expected, serviceTypes.Count);
        }

        [TestMethod]
        public void LoadByIdTest()
        {
            var task = ServiceTypeManager.Load();
            List<ServiceType> serviceTypes = task.Result;
            task.Wait();

            Guid id = serviceTypes.FirstOrDefault().Id;

            var task2 = ServiceTypeManager.LoadById(id);
            ServiceType serviceType = task2.Result;
            task2.Wait();

            Assert.IsTrue(serviceType.Id == id);
        }

        [TestMethod]
        public void LoadByIdDescription()
        {
            var task = ServiceTypeManager.Load();
            List<ServiceType> serviceTypes = task.Result;
            task.Wait();

            string description = serviceTypes.FirstOrDefault().Description;

            var task2 = ServiceTypeManager.LoadByDescription(description);
            ServiceType serviceType = task2.Result;
            task2.Wait();

            Assert.IsTrue(serviceType.Description == description);
        }


        [TestMethod]
        public void InsertTest()
        {
            var task = CustomerManager.Load();
            Customer customer = task.Result.FirstOrDefault();
            task.Wait();

            ServiceType serviceType = new ServiceType
            {
                 Description = "Rake Leaves",
                 CostPerSQFT = 0.004M
            };

            var task2 = ServiceTypeManager.Insert(serviceType, true);
            bool result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var task = ServiceTypeManager.Load();
            ServiceType serviceType = task.Result.FirstOrDefault();
            task.Wait();

            serviceType.CostPerSQFT = 0.003M;

            var task2 = ServiceTypeManager.Update(serviceType, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = ServiceTypeManager.Load();
            ServiceType serviceType = task.Result.FirstOrDefault();
            task.Wait();

            var task2 = ServiceTypeManager.Delete(serviceType.Id, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }
    }
}
