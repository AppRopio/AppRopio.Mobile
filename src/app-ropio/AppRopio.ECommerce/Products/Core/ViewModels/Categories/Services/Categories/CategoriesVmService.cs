using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Products.API.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using AppRopio.Models.Products.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services
{
    public class CategoriesVmService : BaseVmService, ICategoriesVmService
    {
        #region Services

        protected ICategoriesService CategoriesService { get { return Mvx.Resolve<ICategoriesService>(); } }

        #endregion

        #region Protected

        protected virtual ICategoriesItemVM SetupItem(Category category)
        {
            return new CategoriesItemVM(category.Id, category.Name, category.IconUrl, category.BackgroundImageUrl, category.ContainerType == CategoryContainerType.Categories);
        }

        #endregion

        #region ICategoriesVmService implementation

        public async Task<ObservableCollection<ICategoriesItemVM>> LoadItemsFor(string categoryId = null)
        {
            ObservableCollection<ICategoriesItemVM> dataSource = null;

            try
            {
                IEnumerable<Category> categories;

                if (!CachedObjects.ContainsKey(categoryId.IsNullOrEmtpy() ? "null" : categoryId))
                {
                    categories = await CategoriesService.LoadCategories(categoryId);

                    if (!categories.IsNullOrEmpty())
                        CachedObjects.Add(categoryId.IsNullOrEmtpy() ? "null" : categoryId, categories);
                }
                else
                    categories = CachedObjects[categoryId.IsNullOrEmtpy() ? "null" : categoryId].Cast<Category>();

                dataSource = new ObservableCollection<ICategoriesItemVM>(categories.Select(c => SetupItem(c)));
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return dataSource;
        }

        #endregion
    }
}
