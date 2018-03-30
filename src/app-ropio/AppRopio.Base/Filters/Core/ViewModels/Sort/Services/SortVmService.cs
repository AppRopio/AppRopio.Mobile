using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Filters.API;
using AppRopio.Base.Filters.Core.Messages;
using AppRopio.Base.Filters.Core.ViewModels.Sort.Items;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Filters.Core.ViewModels.Sort.Services
{
    public class SortVmService : BaseVmService, ISortVmService
    {
        #region Services

        protected IMvxMessenger Messenger { get { return Mvx.Resolve<IMvxMessenger>(); } }

        protected IFiltersService FiltersService { get { return Mvx.Resolve<IFiltersService>(); } }

        #endregion

        #region Protected

        protected virtual ISortItemVM SetupItem(SortType model, string selectedSortId)
        {
            return new SortItemVM(model, model.Id == selectedSortId);
        }

        #endregion

        #region ISortVmService implementation

        public void ChangeSortTypeTo(string categoryId, SortType sort)
        {
            Messenger.Publish(new SortChangedMessage(this, categoryId, sort));
        }

        public async Task<ObservableCollection<ISortItemVM>> LoadSortTypesInCategory(string categoryId, string selectedSortId)
        {
            ObservableCollection<ISortItemVM> dataSource = null;

            try
            {
                IEnumerable<SortType> sortTypes = null;

                if (categoryId.IsNullOrEmtpy())
                    sortTypes = await FiltersService.LoadSortTypes(categoryId);
                else if (!CachedObjects.ContainsKey(categoryId))
                {
                    sortTypes = await FiltersService.LoadSortTypes(categoryId);

                    if (!sortTypes.IsNullOrEmpty())
                        CachedObjects.Add(categoryId, sortTypes);
                }
                else if (CachedObjects.ContainsKey(categoryId))
                    sortTypes = CachedObjects[categoryId].Cast<SortType>();

                dataSource = new ObservableCollection<ISortItemVM>(sortTypes.Select(c => SetupItem(c, selectedSortId)));
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
