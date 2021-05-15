using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace KRV.LawnPro.Mobile.Models
{
    public class User
    {

        public class UserCollection
        {
            // match the collection name in the Json string sent back from the API
            public User[] UserItems { get; set; }
        }



        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string UserPass2 { get; set; }

        public string FullName { 
            get 
            {
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                {
                    return string.Empty;
                }
                else
                {
                    return LastName + ", " + FirstName;
                }
            } 
        }
    }
}
