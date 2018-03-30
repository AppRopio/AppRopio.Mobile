using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services.Fakes
{
    public class ContentSearchFakeService : IContentSearchService
    {
        public async Task<List<Autocomplete>> LoadAutocompletes(string searchText, CancellationToken token)
        {
            await Task.Delay(300);
            return new List<Autocomplete>
            {
                new Autocomplete { Hint = "телевизоры", Result = "телевизоры" },
                new Autocomplete { Hint = "телевизоры lg", Result = "телевизоры lg" },
                new Autocomplete { Hint = "телевизоры bbk", Result = "телевизоры bbk" },
                new Autocomplete { Hint = "thomson", Result = "thomson" },
            };
        }

        public async Task<List<string>> LoadHints(string searchText, CancellationToken token)
        {
            await Task.Delay(300);
            return new List<string>
            {
                "телевизоры sony",
                "телевизоры supra",
                "телевизоры bbk",
                "телевизоры thomson",
                "телевизоры mystery",
                "телевизоры shivaki",
                "телевизоры panasonic",
                "телевизоры telefunken",
                "телевизоры jvc",
                "телевизоры erisson"
            };
        }
    }
}
