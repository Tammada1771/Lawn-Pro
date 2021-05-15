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
    public class utAppointment
    {
        [TestMethod]
        public void LoadTest()
        {
            var task = AppointmentManager.Load();
            List<Appointment> appointments = task.Result;
            task.Wait();

            Assert.IsTrue(appointments.Count > 0);
        }

        [TestMethod]
        public void LoadByIdTest()
        {
            var task = AppointmentManager.Load();
            List<Appointment> appointments = task.Result;
            task.Wait();

            Guid id = appointments.FirstOrDefault().Id;

            var task2 = AppointmentManager.LoadById(id);
            Appointment appointment = task2.Result;
            task.Wait();

            Assert.IsTrue(appointment.Id == id);
        }

        [TestMethod]
        public void LoadByCustomerIdTest()
        {
            var task = AppointmentManager.Load();
            List<Appointment> appointments = task.Result;
            task.Wait();

            Guid id = appointments.FirstOrDefault().CustomerId;

            var task2 = AppointmentManager.LoadByCustomerId(id);
            List<Appointment> customerAppointments = task2.Result;
            task2.Wait();

            Assert.IsTrue(customerAppointments.Count > 0);
        }

        [TestMethod]
        public void LoadByEmployeeIdTest()
        {
            var task = AppointmentManager.Load();
            List<Appointment> appointments = task.Result;
            task.Wait();

            Guid id = (Guid)appointments.Where(a=>a.EmployeeId != null).FirstOrDefault().EmployeeId;

            var task2 = AppointmentManager.LoadByEmployeeId(id);
            List<Appointment> employeeAppointments = task2.Result;
            task2.Wait();

            Assert.IsTrue(employeeAppointments.Count > 0);
        }

        [TestMethod]
        public void InsertTest()
        {
            var task1 = CustomerManager.Load();
            Customer customer = task1.Result.FirstOrDefault();
            task1.Wait();

            var task2 = EmployeeManager.Load();
            Employee employee = task2.Result.FirstOrDefault();
            task2.Wait();

            var task3 = ServiceTypeManager.Load();
            ServiceType serviceType = task3.Result.FirstOrDefault();
            task3.Wait();

            Appointment appointment = new Appointment
            {
                CustomerId = customer.Id,
                EmployeeId = employee.Id,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddHours(2),
                ServiceId = serviceType.Id,
                Status = AppointmentStatus.Scheduled.ToString()
            };

            var task4 = AppointmentManager.Insert(appointment, true);
            bool result = task4.Result;
            task4.Wait();

            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var task = AppointmentManager.Load();
            List<Appointment> appointments = task.Result;
            task.Wait();

            Appointment appointment = appointments.FirstOrDefault();
            appointment.Status = AppointmentStatus.InProgress.ToString();

            var task2 = AppointmentManager.Update(appointment, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = AppointmentManager.Load();
            List<Appointment> appointments = task.Result;
            task.Wait();

            Appointment appointment = appointments.FirstOrDefault();

            var task2 = AppointmentManager.Delete(appointment.Id, true);
            int result = task2.Result;
            task2.Wait();

            Assert.IsTrue(result > 0);
        }
    }
}
