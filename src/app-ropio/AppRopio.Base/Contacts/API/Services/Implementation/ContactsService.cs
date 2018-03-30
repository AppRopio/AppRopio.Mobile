using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Contacts.Requests;
using AppRopio.Models.Contacts.Responses;

namespace AppRopio.Base.Contacts.API.Services.Implementation
{
    public class ContactsService : BaseService, IContactsService
    {
        protected string CONTACTS_URL = "contacts";

        public async Task<ListResponse> LoadContacts(Coordinates location)
        {
            var request = new ListRequest()
            {
                UserLocation = location
            };

            return await Post<ListResponse>(CONTACTS_URL, ToStringContent(request));
        }
    }
}