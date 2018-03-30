using System.Collections.Generic;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items
{
    public interface ICatalogItemVM : IMvxViewModel, IMarkedItemVM
    {
        Product Model { get; }

        string Id { get; }

        string ImageUrl { get; }

        string Name { get; }

        decimal Price { get; }

        string UnitName { get; }

        decimal? OldPrice { get; }

        string UnitNameOld { get; }

        List<IProductBadgeItemVM> Badges { get; }

        string StateName { get; }
    }
}
