using System;
using AppRopio.Base.Filters.Core.Models;

namespace AppRopio.Base.Filters.Core.Services
{
    public interface IFiltersConfigService
    {
        FiltersConfig Config { get; }
    }
}
