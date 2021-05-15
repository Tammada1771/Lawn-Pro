using System;
using System.Collections.Generic;

#nullable disable

namespace KRV.LawnPro.PL
{
    public partial class tblInvoice
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServiceRate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string EmployeeName { get; set; }
        public string Status { get; set; }

        public virtual tblCustomer Customer { get; set; }
    }
}
