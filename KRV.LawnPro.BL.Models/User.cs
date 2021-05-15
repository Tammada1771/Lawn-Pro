using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRV.LawnPro.BL.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Password")]
        public string UserPass { get; set; }

        [DisplayName("Confirm Password")]
        [NotMapped]
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("UserPass", ErrorMessage = "Password doesn't match.")]
        public string UserPass2 { get; set; }

        [DisplayName("Name")]
        public string FullName { get { return LastName + ", " + FirstName; } }
        public string WelcomeName { get { return FirstName + " " + LastName; } }
    }
}
