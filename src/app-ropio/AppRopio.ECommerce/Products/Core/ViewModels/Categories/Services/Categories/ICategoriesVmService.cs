using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services
{
    public interface ICategoriesVmService
    {
        /// <summary>
        /// Загрузка списка категорий
        /// </summary>
        /// <returns>Список категорий</returns>
        /// <param name="categoryId">Идентификатор родительской категории. Равен null, если требуется загрузить категории нулевого уровня</param>
        Task<ObservableCollection<ICategoriesItemVM>> LoadItemsFor(string categoryId = null);
    }
}
