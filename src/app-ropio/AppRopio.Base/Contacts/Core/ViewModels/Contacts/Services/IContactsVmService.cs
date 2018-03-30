using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Contacts.Responses;

namespace AppRopio.Base.Contacts.Core.ViewModels.Contacts.Services
{
    public interface IContactsVmService
    {
		Task<List<ListResponseItem>> LoadContacts();

        Task HandleContactSelection(ListResponseItem contact);
    }
}