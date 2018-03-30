using System.Collections.Generic;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.ShortInfo
{
    public interface IShortInfoProductsPciVm : IProductBasicItemVM
    {
        IMvxCommand ShareCommand { get; }

        IMvxCommand MarkCommand { get; }

        bool Marked { get; }
        
        bool IsPriceDependsOnParams { get; }

        string Name { get; }

        decimal Price { get; }

        string UnitName { get; }

        decimal? OldPrice { get; }

        string UnitNameOld { get; }

        decimal? ExtraPrice { get; }

        string UnitNameExtra { get; }

        List<IProductBadgeItemVM> Badges { get; }

        string StateName { get; }
    }
}
