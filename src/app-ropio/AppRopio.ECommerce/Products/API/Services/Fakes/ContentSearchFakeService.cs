using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Products.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Products.API.Services.Fakes
{
    public class ContentSearchFakeService : IContentSearchService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        public async Task<List<Autocomplete>> LoadAutocompletes(string searchText, CancellationToken token)
        {
            await Task.Delay(300);
            return new List<Autocomplete>
            {
                new Autocomplete { Hint = IsRussianCulture ? "телевизоры" : "TV", Result = IsRussianCulture ? "телевизоры" : "TV" },
                new Autocomplete { Hint = IsRussianCulture ? "телевизоры lg" : "TV LG", Result = IsRussianCulture ? "телевизоры lg" : "TV LG" },
                new Autocomplete { Hint = IsRussianCulture ? "телевизоры bbk" : "TV BBK", Result = IsRussianCulture ? "телевизоры bbk" : "TV BBK" },
                new Autocomplete { Hint = "thomson", Result = "thomson" },
            };
        }

        public async Task<List<string>> LoadHints(string searchText, CancellationToken token)
        {
            await Task.Delay(300);
            return new List<string>
            {
                IsRussianCulture ? "телевизоры sony" : "TV Sony",
                IsRussianCulture ? "телевизоры supra" : "TV Supra",
                IsRussianCulture ? "телевизоры bbk" : "TV BBK",
                IsRussianCulture ? "телевизоры thomson" : "TV Thomson",
                IsRussianCulture ? "телевизоры mystery" : "TV Mystery",
                IsRussianCulture ? "телевизоры shivaki" : "TV Shivaki",
                IsRussianCulture ? "телевизоры panasonic" : "TV Panasonic",
                IsRussianCulture ? "телевизоры telefunken" : "TV Telefunken",
                IsRussianCulture ? "телевизоры jvc" : "TV JVC",
                IsRussianCulture ? "телевизоры erisson" : "TV Erisson"
            };
        }
    }
}
