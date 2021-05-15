using KRV.LawnPro.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KRV.LawnPro.Web.ViewModel
{
    public class AppointmentViewModel
    {
        public Appointment Appointment { get; set; }
        public List<ServiceType> ServiceTypes { get; set; }
    }
}
