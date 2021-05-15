using KRV.LawnPro.BL.Models;
using KRV.LawnPro.Web.Extensions;
using KRV.LawnPro.Web.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KRV.LawnPro.Web.Controllers
{
    public class AppointmentController : Controller
    {
        HttpClient client = new HttpClient();

        public AppointmentController(IOptions<AppSettings> appSettings)
        {
            client.BaseAddress = new Uri(appSettings.Value.ApiBaseUri);
        }

        // GET: AppointmentsController
        public ActionResult Index()
        {
            if (Authenticate.IsAuthenticated(HttpContext))
            {
                ViewBag.Title = "My Appointments";
                ViewBag.Info = TempData["info"];

                Customer customer = HttpContext.Session.GetObject<Customer>("customer");

                HttpResponseMessage response = client.GetAsync("/Appointment/byCustomerId/" + customer.Id).Result;
                string result = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(result);
                    return View(appointments);
                }
                else
                {
                    TempData["error"] = "An error occured trying to retrieve your appointments.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "User", new { returnUrl = UriHelper.GetDisplayUrl(HttpContext.Request) });
            }
        }

        // GET: AppointmentsController/Details/5
        public ActionResult Details(Guid id)
        {
            if (Authenticate.IsAuthenticated(HttpContext))
            {
                ViewBag.Title = "My Appointment Details";
                HttpResponseMessage response = client.GetAsync("/Appointment/byId/" + id).Result;
                string result = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Appointment appointment = JsonConvert.DeserializeObject<Appointment>(result);
                    return View(appointment);
                }
                else
                {
                    TempData["error"] = "An error occured trying to get the appointment details.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("Login", "User", new { returnUrl = UriHelper.GetDisplayUrl(HttpContext.Request) });
            }
        }


        // GET: UserController/Create
        public ActionResult Create()
        {
            if (Authenticate.IsAuthenticated(HttpContext))
            {
                ViewBag.Title = "Request Service";
                AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
                appointmentViewModel.Appointment = new Appointment();

                HttpResponseMessage serviceTypeResponse = client.GetAsync("ServiceType").Result;
                string serviceTypeResult = serviceTypeResponse.Content.ReadAsStringAsync().Result;

                if (serviceTypeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<ServiceType> serviceTypes = JsonConvert.DeserializeObject<List<ServiceType>>(serviceTypeResult);
                    appointmentViewModel.Appointment.StartDateTime = DateTime.Now.AddDays(1);
                    appointmentViewModel.ServiceTypes = serviceTypes;

                    HttpContext.Session.SetObject("AppointmentViewModel", appointmentViewModel);

                    return View(appointmentViewModel);
                }
                else
                {
                    TempData["error"] = "An error occured trying to request service.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "User", new { returnUrl = UriHelper.GetDisplayUrl(HttpContext.Request) });
            }
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppointmentViewModel appointmentViewModel)
        {
            try
            {
                if (appointmentViewModel.Appointment.StartDateTime.Date > DateTime.Today.Date)
                {
                    ViewBag.Title = "Request Service";

                    Customer customer = HttpContext.Session.GetObject<Customer>("customer");

                    appointmentViewModel.Appointment.CustomerId = customer.Id;
                    appointmentViewModel.Appointment.EmployeeId = null;
                    appointmentViewModel.Appointment.EndDateTime = appointmentViewModel.Appointment.StartDateTime;
                    appointmentViewModel.Appointment.Status = "Unscheduled";

                    string serializedObject = JsonConvert.SerializeObject(appointmentViewModel.Appointment);
                    var content = new StringContent(serializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var insertAppointmentResponse = client.PostAsync("Appointment/", content).Result;
                    string insertAppointmentResult = insertAppointmentResponse.Content.ReadAsStringAsync().Result;

                    if (insertAppointmentResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Index", "Appointment");
                    }
                    else
                    {
                        throw new Exception(insertAppointmentResult);
                    }

                }
                else
                {
                    throw new Exception("Date must be in the future.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Request Service";
                appointmentViewModel = HttpContext.Session.GetObject<AppointmentViewModel>("AppointmentViewModel");
                ViewBag.Error = ex.Message;
                return View(appointmentViewModel);
            }
        }

        public IActionResult Delete(Guid id)
        {
            if (Authenticate.IsAuthenticated(HttpContext))
            {
                var getResponse = client.GetAsync("Appointment/byId/" + id).Result;
                var getResult = getResponse.Content.ReadAsStringAsync().Result;
                Appointment appointment = JsonConvert.DeserializeObject<Appointment>(getResult);

                if (getResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewBag.Title = "Delete Appointment on " + appointment.StartDateTime.ToShortDateString();
                    return View(appointment);
                }
                else
                {
                    TempData["error"] = "An error occured trying to delete the appointment.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("Login", "User", new { returnUrl = UriHelper.GetDisplayUrl(HttpContext.Request) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id, Appointment appointment)
        {
            try
            {
                ViewBag.Title = "Delete Appointment on " + appointment.StartDateTime.ToShortDateString();

                var deleteResponse = client.DeleteAsync("Appointment/" + appointment.Id).Result;
                var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;

                if (deleteResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw new Exception(deleteResult);
                }

            }
            catch (Exception ex)
            {
                ViewBag.Title = "Delete Appointment on " + appointment.StartDateTime.ToShortDateString();
                ViewBag.Error = ex.Message;
                return View(appointment);
            }
        }


    }
}
