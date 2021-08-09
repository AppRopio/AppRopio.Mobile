using System;
using MvvmCross.ViewModels;
using AppRopio.Models.Products.Responses;
namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items
{
    public class ProductBadgeItemVM : MvxViewModel, IProductBadgeItemVM
    {
        public ProductBadge Model { get; }

        public string Name { get; protected set; }

        public string Color { get; protected set; }

        public ProductBadgeItemVM(ProductBadge model)
        {
            Model = model;

            Name = model.Name;
            Color = model.Color;
        }
    }
}
