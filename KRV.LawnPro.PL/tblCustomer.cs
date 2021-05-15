using System;
using System.Collections.Generic;

#nullable disable

namespace KRV.LawnPro.PL
{
    public partial class tblCustomer
    {
        public tblCustomer()
        {
            TblAppointments = new HashSet<tblAppointment>();
            TblInvoices = new HashSet<tblInvoice>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PropertySqFt { get; set; }
        public Guid UserId { get; set; }

        public virtual tblUser User { get; set; }
        public virtual ICollection<tblAppointment> TblAppointments { get; set; }
        public virtual ICollection<tblInvoice> TblInvoices { get; set; }
    }
}
