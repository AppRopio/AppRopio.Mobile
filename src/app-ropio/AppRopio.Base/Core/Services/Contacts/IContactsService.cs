using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Contacts;

namespace AppRopio.Base.Core.Services.Contacts
{
    public interface IContactsService
    {
        Task<List<Contact>> GetContacts();

        Task<Contact> SelectContact();

        Task<Phone> SelectPhone();

        Task<Email> SelectEmail();
    }
}
