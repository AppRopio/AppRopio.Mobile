using System;
using AppRopio.Models.Filters.Responses;
namespace AppRopio.Base.Filters.Core.ViewModels.Sort.Items
{
    public interface ISortItemVM
    {
        SortType Sort { get; }

        string Name { get; }

        bool Selected { get; set; }
    }
}
