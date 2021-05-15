using KRV.LawnPro.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KRV.LawnPro.Web.Models
{
    public class InvoiceModel
    {
        public List<Invoice> Invoices { get; set; }
        public decimal InvoiceBalance { get; set; }
    }
}
