using System;
namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items
{
    public interface ICategoriesItemVM
    {
        string CategoryID { get; }

        string Name { get; }

        string IconUrl { get; }

        string BackgroundImageUrl { get; }

        bool IsFolder { get; }
    }
}

