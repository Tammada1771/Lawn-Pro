using KRV.LawnPro.BL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KRV.LawnPro.Web.ViewModel
{
    public class CustomerUserViewModel
    {
        public Customer Customer { get; set; }
        public User User { get; set; }
    }
}
