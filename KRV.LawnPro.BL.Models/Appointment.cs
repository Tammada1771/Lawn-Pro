using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRV.LawnPro.BL.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
       
        public Guid CustomerId { get; set; }
        
        public Guid? EmployeeId { get; set; }
        
        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDateTime { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDateTime { get; set; }

        [DisplayName("Service")]
        public Guid ServiceId { get; set; }
       
        public string Status { get; set; }

        [DisplayName("Service Type")]
        public string ServiceType { get; set; }

        public decimal ServiceRate { get; set; }

        [DisplayName("Appointment Length")]
        public int AppointmentLength 
        { 
            get 
            {
                //  Calculate 30 minutes for each 10,000 square feet of property
                return (int)Math.Ceiling((decimal)(PropertySqFeet / 10000)) * 30;  
            } 
        }

        [DisplayName("Customer First Name")]
        public string CustomerFirstName { get; set; }

        [DisplayName("Customer Last Name")]
        public string CustomerLastName { get; set; }

        [DisplayName("Employee First Name")]
        public string EmployeeFirstName { get; set; }

        [DisplayName("Employee Last Name")]
        public string EmployeeLastName { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [DisplayName("Property Sq Ft")]
        public int PropertySqFeet { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerFullName
        {
            get
            {
                if (string.IsNullOrEmpty(CustomerLastName) && string.IsNullOrEmpty(CustomerFirstName))
                {
                    return string.Empty;
                }
                else
                {
                    return CustomerLastName + ", " + CustomerFirstName;
                }
            }
        }

        [DisplayName("Employee Name")]
        public string EmployeeFullName 
        {
            get
            {
                if (string.IsNullOrEmpty(EmployeeLastName) && string.IsNullOrEmpty(EmployeeFirstName))
                {
                    return string.Empty;        
                }
                else
                {
                    return EmployeeLastName + ", " + EmployeeFirstName;
                }
            }
        }

        [DisplayName("Full Address")]
        public string FullAddress 
        {
            get
            {
                return StreetAddress + " " + City + ", " + State + "  " + ZipCode;
            }
        }

    }
}
