using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories
{
    public interface ICategoriesViewModel : IProductsViewModel
    {
        /// <summary>
        /// Список категорий
        /// </summary>
        ObservableCollection<ICategoriesItemVM> Items { get; }
    }
}

