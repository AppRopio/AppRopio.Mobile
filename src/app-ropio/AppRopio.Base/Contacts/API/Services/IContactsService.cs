using System;
using System.Threading.Tasks;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Contacts.Responses;

namespace AppRopio.Base.Contacts.API.Services
{
    public interface IContactsService
    {
        Task<ListResponse> LoadContacts(Coordinates location);
    }
}
