using System;
using System.Collections.Generic;

#nullable disable

namespace KRV.LawnPro.PL
{
    public partial class tblEmployee
    {
        public tblEmployee()
        {
            TblAppointments = new HashSet<tblAppointment>();
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
        public Guid UserId { get; set; }

        public virtual tblUser User { get; set; }
        public virtual ICollection<tblAppointment> TblAppointments { get; set; }
    }
}
