using System;
using System.Collections.Generic;

#nullable disable

namespace KRV.LawnPro.PL
{
    public partial class tblServiceType
    {
        public tblServiceType()
        {
            TblAppointments = new HashSet<tblAppointment>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal CostPerSqFt { get; set; }

        public virtual ICollection<tblAppointment> TblAppointments { get; set; }
    }
}
