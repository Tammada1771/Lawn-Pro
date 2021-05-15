using System;
using System.Collections.Generic;

#nullable disable

namespace KRV.LawnPro.PL
{
    public partial class tblUser
    {
        public tblUser()
        {
            TblCustomers = new HashSet<tblCustomer>();
            TblEmployees = new HashSet<tblEmployee>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }

        public virtual ICollection<tblCustomer> TblCustomers { get; set; }
        public virtual ICollection<tblEmployee> TblEmployees { get; set; }
    }
}
