using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Products.Responses;
using AppRopio.Models.Products.Requests;

namespace AppRopio.ECommerce.Products.API.Services.Implementation
{
    public class ContentSearchService : BaseService, IContentSearchService
    {
        public string HINTS_URL = "search/hints";
        public string AUTOCOMPLETE_URL = "search/autocomplete";

        public async Task<List<string>> LoadHints(string searchText, CancellationToken token)
        {
            return await Post<List<string>>(HINTS_URL, ToStringContent(new ContentSearchRequest { SearchText = searchText }), cancellationToken: token);
        }

        public async Task<List<Autocomplete>> LoadAutocompletes(string searchText, CancellationToken token)
        {
            return await Post<List<Autocomplete>>(AUTOCOMPLETE_URL, ToStringContent(new ContentSearchRequest { SearchText = searchText }), cancellationToken: token);
        }
    }
}
