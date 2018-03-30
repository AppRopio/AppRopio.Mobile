using System;
using System.Collections.Generic;

namespace AppRopio.Base.Core.Models.Contacts
{
    public class Contact
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public List<Phone> Phones { get; set; }

        public List<Email> Emails { get; set; }
    }
}
