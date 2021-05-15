using System;
using System.Collections.Generic;

#nullable disable

namespace KRV.LawnPro.PL
{
    public partial class tblAppointment
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Guid ServiceId { get; set; }
        public string Status { get; set; }

        public virtual tblCustomer Customer { get; set; }
        public virtual tblEmployee Employee { get; set; }
        public virtual tblServiceType Service { get; set; }
    }
}
