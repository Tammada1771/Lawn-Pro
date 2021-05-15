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
using System.Net.Http;

namespace KRV.LawnPro.Web.Controllers
{
    public class UserController : Controller
    {
        HttpClient client = new HttpClient();
 
        public UserController(IOptions<AppSettings> appSettings)
        {
            client.BaseAddress = new Uri(appSettings.Value.ApiBaseUri);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Sign Up";
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerUserViewModel customerUser)
        {
            try
            {
                ViewBag.Title = "Sign Up";

                string serializedObject = JsonConvert.SerializeObject(customerUser.User);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var insertUserResponse = client.PostAsync("User/", content).Result;
                string insertUserResult = insertUserResponse.Content.ReadAsStringAsync().Result;

                if (insertUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // returns the guid Id of the inserted record
                    customerUser.User.Id = JsonConvert.DeserializeObject<Guid>(insertUserResult);
                    customerUser.Customer.UserId = customerUser.User.Id;
                    customerUser.Customer.FirstName = customerUser.User.FirstName;
                    customerUser.Customer.LastName = customerUser.User.LastName;

                    // save customer
                    serializedObject = JsonConvert.SerializeObject(customerUser.Customer);
                    content = new StringContent(serializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var insertCustomerResponse = client.PostAsync("Customer/", content).Result;
                    string insertCustomerResult = insertCustomerResponse.Content.ReadAsStringAsync().Result;

                    if (insertCustomerResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // returns the guid Id of the inserted record
                        customerUser.Customer.Id = JsonConvert.DeserializeObject<Guid>(insertCustomerResult);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception(insertCustomerResult);
                    }
                }
                else
                {
                    throw new Exception(insertUserResult);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Sign Up";
                ViewBag.Error = ex.Message;
                return View(customerUser);
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (Authenticate.IsAuthenticated(HttpContext))
            {
                ViewBag.Title = "Edit My Profile";

                CustomerUserViewModel customerUser = new CustomerUserViewModel
                {
                    User = HttpContext.Session.GetObject<User>("user"),
                    Customer = HttpContext.Session.GetObject<Customer>("customer"),
                };

                return View(customerUser);
            }
            else
            {
                return RedirectToAction("Login", "User", new { returnUrl = UriHelper.GetDisplayUrl(HttpContext.Request) });
            }
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, CustomerUserViewModel customerUser)
        {
            try
            {
                ViewBag.Title = "Edit My Profile";

                string serializedObject = JsonConvert.SerializeObject(customerUser.User);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var insertUserResponse = client.PutAsync("User/", content).Result;
                string insertUserResult = insertUserResponse.Content.ReadAsStringAsync().Result;

                if (insertUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // We only collected user first name and Last name.  Make customers the same.
                    customerUser.Customer.FirstName = customerUser.User.FirstName;
                    customerUser.Customer.LastName = customerUser.User.LastName;

                    // save customer
                    serializedObject = JsonConvert.SerializeObject(customerUser.Customer);
                    content = new StringContent(serializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var insertCustomerResponse = client.PutAsync("Customer/", content).Result;
                    string insertCustomerResult = insertCustomerResponse.Content.ReadAsStringAsync().Result;

                    if (insertCustomerResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContext.Session.SetObject("user", customerUser.User);
                        HttpContext.Session.SetObject("customer", customerUser.Customer);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception(insertCustomerResult);
                    }
                }
                else
                {
                    throw new Exception(insertUserResult);
                }

            }
            catch (Exception ex)
            {
                ViewBag.Title = "Edit My Profile";
                ViewBag.Error = ex.Message;
                return View(customerUser);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetObject("user", null);
            return RedirectToAction("Index", "Home");
        }

        // GET: UserController/Create
        public IActionResult Login(string returnUrl)
        {
            TempData["returnurl"] = returnUrl;
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user, string returnUrl)
        {
            try
            {
                user.FirstName = "unknown";
                user.LastName = "unknown";
                user.UserPass2 = user.UserPass;

                string serializedObject = JsonConvert.SerializeObject(user);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var loginResponse = client.PostAsync("user/login/", content).Result;
                var loginResult = loginResponse.Content.ReadAsStringAsync().Result;

                if (loginResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic item = JsonConvert.DeserializeObject(loginResult);
                    user = item.ToObject<User>();

                    HttpResponseMessage response = client.GetAsync("Customer/byUserId/" + user.Id).Result;
                    string getCustomerResult = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Customer customer = JsonConvert.DeserializeObject<Customer>(getCustomerResult);

                        HttpContext.Session.SetObject("user", user);
                        HttpContext.Session.SetObject("customer", customer);


                        if (TempData["returnurl"] != null)
                        {
                            return Redirect(TempData["returnurl"].ToString());
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ViewBag.Error = getCustomerResult;
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = loginResult;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }


    }
}
