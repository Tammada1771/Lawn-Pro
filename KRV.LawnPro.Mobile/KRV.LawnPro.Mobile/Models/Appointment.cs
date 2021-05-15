using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRV.LawnPro.Mobile.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Guid ServiceId { get; set; }
        public string Status { get; set; }

        public string ServiceType { get; set; }

        public decimal ServiceRate { get; set; }

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
        public string CustomerFullName { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeFullName { get; set; }

        [DisplayName("Full Address")]
        public string FullAddress { get; set; }

        public string AppointmentTime
        {
            get
            {
                return string.Format("{0} - {1} - {2}", StartDateTime.ToString("h:mm tt"), EndDateTime.ToString("h:mm tt"), StatusDetail);
            }
        }


        public string AppointmentDetails
        {
            get
            {
                return string.Format("{0} - {1}", CustomerLastName, FullAddress);
            }
        }

        public string ServiceDetails
        {
            get
            {
                return string.Format("Service: " + ServiceType);
            }
        }


        public string StatusDetail 
        {
            get 
            {
                if (Status == "Scheduled")
                {
                    return "SCHEDULED";
                }
                else if (Status == "InProgress")
                {
                    return "IN PROGRESS";
                }
                else if (Status == "Completed")
                {
                    return "COMPLETED";
                }
                else if (Status == "Canceled")
                {
                    return "CANCELED";
                }
                else
                {
                    return Status;
                }
            }
        }

        public Color StatusColor
        {
            get
            {
                if (Status == "Scheduled")
                {
                    return Color.Red;
                }
                else if (Status == "InProgress")
                {
                    return Color.Yellow;
                }
                else if (Status == "Completed")
                {
                    return Color.Black;
                }
                else if (Status == "Canceled")
                {
                    return Color.Black;
                }
                else
                {
                    return Color.Black;
                }
            }
        }



    }
}
