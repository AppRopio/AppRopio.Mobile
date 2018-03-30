using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories
{
    public interface ICCategoriesViewModel : ICategoriesViewModel
    {
        /// <summary>
        /// Индекс текущей страницы
        /// </summary>
        int CurrentPage { get; set; }

        ICommand PageChangedCommand { get; }

        /// <summary>
        /// Список каталогов по категориям
        /// </summary>
        /// <value>The catalogs.</value>
        ObservableCollection<ICatalogViewModel> Pages { get; }
    }
}

