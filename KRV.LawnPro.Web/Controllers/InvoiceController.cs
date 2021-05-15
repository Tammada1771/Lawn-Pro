using KRV.LawnPro.BL.Models;
using KRV.LawnPro.Web.Extensions;
using KRV.LawnPro.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KRV.LawnPro.Web.Controllers
{
    public class InvoiceController : Controller
    {
        HttpClient client = new HttpClient();

        public InvoiceController(IOptions<AppSettings> appSettings)
        {
            client.BaseAddress = new Uri(appSettings.Value.ApiBaseUri);
        }

        // GET: InvoicesController
        public ActionResult Index()
        {
            if (Authenticate.IsAuthenticated(HttpContext))
            {
                ViewBag.Title = "My Invoices";
                ViewBag.Info = TempData["info"];

                Customer customer = HttpContext.Session.GetObject<Customer>("customer");

                HttpResponseMessage getInvoiceResponse = client.GetAsync("/Invoice/byCustomerId/" + customer.Id).Result;
                string getInvoiceResult = getInvoiceResponse.Content.ReadAsStringAsync().Result;

                if (getInvoiceResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(getInvoiceResult);
                    HttpResponseMessage getBalanceResponse = client.GetAsync("/Customer/GetBalance/" + customer.Id).Result;
                    string getBalanceResult = getBalanceResponse.Content.ReadAsStringAsync().Result;
                    
                    decimal invoiceBalance = 0;
                    if (getBalanceResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        invoiceBalance = JsonConvert.DeserializeObject<decimal>(getBalanceResult);
                    }
                    else
                    {
                        TempData["error"] = "An error occured trying to retrieve your outstanding balance.";
                    }

                    InvoiceModel invoiceModel = new InvoiceModel
                    {
                        Invoices = invoices,
                        InvoiceBalance = invoiceBalance
                    };

                    return View(invoiceModel);
                }
                else
                {
                    TempData["error"] = "An error occured trying to retrieve your invoices.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "User", new { returnUrl = UriHelper.GetDisplayUrl(HttpContext.Request) });
            }
        }

        // GET: InvoicesController/Details/5
        public ActionResult Details(Guid id)
        {
            if (Authenticate.IsAuthenticated(HttpContext))
            {
                ViewBag.Title = "My Invoice Details";
                ViewBag.Info = TempData["info"];

                HttpResponseMessage response = client.GetAsync("/Invoice/byId/" + id).Result;
                string result = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Invoice invoice = JsonConvert.DeserializeObject<Invoice>(result);
                    return View(invoice);
                }
                else
                {
                    TempData["error"] = "An error occured trying to get the invoice details.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("Login", "User", new { returnUrl = UriHelper.GetDisplayUrl(HttpContext.Request) });
            }
        }

        public ActionResult DetailsAsPDF(Guid id)
        {
            if (Authenticate.IsAuthenticated(HttpContext))
            {
                HttpResponseMessage response = client.GetAsync("/Invoice/byId/" + id).Result;
                string result = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Invoice invoice = JsonConvert.DeserializeObject<Invoice>(result);
                    return new ViewAsPdf("InvoiceAsPDF", invoice);
                }
                else
                {
                    TempData["error"] = "An error occured trying to export your invoice.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("Login", "User", new { returnUrl = UriHelper.GetDisplayUrl(HttpContext.Request) });
            }
        }



    }
}
