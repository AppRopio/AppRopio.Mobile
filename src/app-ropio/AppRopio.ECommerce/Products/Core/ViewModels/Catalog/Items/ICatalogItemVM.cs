using System.Collections.Generic;
using AppRopio.Base.Core.ViewModels._base;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items
{
    public interface ICatalogItemVM : IMvxViewModel, IMarkedItemVM, IActionCommandViewModel
    {
        Product Model { get; }

        string Id { get; }

        string ImageUrl { get; }

        string Name { get; }

        string Price { get; }

        string MaxPrice { get; }

        string OldPrice { get; }

        List<IProductBadgeItemVM> Badges { get; }

        string StateName { get; }

        IMvxViewModel BasketBlockViewModel { get; }

        bool HasAction { get; }
    }
}
