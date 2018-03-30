using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services
{
    public interface IContentSearchService
    {
        Task<List<string>> LoadHints(string searchText, CancellationToken token);

        Task<List<Autocomplete>> LoadAutocompletes(string searchText, CancellationToken token);
    }
}
