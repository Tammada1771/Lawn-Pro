using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRV.LawnPro.BL.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerStreetAddress { get; set; }

        public string CustomerFullName { get; set; }

        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZip { get; set; }

        public string FullAddress { get; set; }

        public string CustomerEmail { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ServiceDate { get; set; }

        [DisplayName("Service Description")]
        public string ServiceType { get; set; }

        public int PropertySqFt { get; set; }

        public Decimal ServiceRate { get; set; }

        [DisplayName("Technician")]
        public string EmployeeFullName { get; set; }

        public String Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal InvoiceTotal
        {
            get
            {
                return ServiceRate * PropertySqFt;
            }
        }



    }
}
