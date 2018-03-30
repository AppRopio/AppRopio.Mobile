using System;
using AppRopio.Base.Contacts.Core.Models;

namespace AppRopio.Base.Contacts.Core.Services
{
    public interface IContactsConfigService
    {
        ContactsConfig Config { get; }
    }
}
