using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Products.API.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Autocomplete;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Hint;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.History;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Services.DataBase;
using AppRopio.Models.Products.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Services
{
    public class ContentSearchVmService : BaseVmService, IContentSearchVmService
    {
        #region Services

        protected IHistorySearchDbService DbService { get { return Mvx.Resolve<IHistorySearchDbService>(); } }

        protected IContentSearchService ApiService { get { return Mvx.Resolve<IContentSearchService>(); } }

        #endregion

        #region Protected

        protected virtual IHistorySearchItemVM SetupHistoryItem(string searchText)
        {
            return new HistorySearchItemVM(searchText);
        }

        protected virtual IHintItemVM SetupHintItem(string searchText)
        {
            return new HintItemVM(searchText);
        }

        protected virtual IAutocompleteItemVM SetupAutocompleteItem(Autocomplete autocomplete)
        {
            return new AutocompleteItemVM(autocomplete);
        }

        #endregion

        #region IContentSearchVmService implementation

        #region History

        public Task ClearHistory()
        {
            return Task.Run(() =>
            {
                try
                {
                    DbService.DeleteAll();
                }
                catch (ConnectionException ex)
                {
                    OnConnectionException(ex);
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
            });
        }

        public Task<ObservableCollection<IHistorySearchItemVM>> LoadSearchHistory()
        {
            return Task.Run(() =>
            {
                ObservableCollection<IHistorySearchItemVM> dataSource = null;

                try
                {
                    var history = DbService.LoadAll().Reverse();

                    dataSource = new ObservableCollection<IHistorySearchItemVM>(history.Select(SetupHistoryItem));
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
            });
        }

        public Task SaveSearchRequestInHistory(string searchText)
        {
            return Task.Run(() =>
            {
                try
                {
                    DbService.InsertOrUpdate(searchText);
                }
                catch (ConnectionException ex)
                {
                    OnConnectionException(ex);
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
            });
        }

        #endregion

        #region Hints

        public async Task<ObservableCollection<IHintItemVM>> LoadHints(string searchText, CancellationToken token)
        {
            ObservableCollection<IHintItemVM> dataSource = null;

            try
            {
                var hints = await ApiService.LoadHints(searchText, token);

                if (!hints.IsNullOrEmpty())
                    dataSource = new ObservableCollection<IHintItemVM>(hints.Select(SetupHintItem));
            }
            catch (OperationCanceledException)
            {
                throw;
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

        #region Autocompletes

        public async Task<ObservableCollection<IAutocompleteItemVM>> LoadAutocompletes(string searchText, CancellationToken token)
        {
            ObservableCollection<IAutocompleteItemVM> dataSource = null;

            try
            {
                var hints = await ApiService.LoadAutocompletes(searchText, token);

                if (!hints.IsNullOrEmpty())
                    dataSource = new ObservableCollection<IAutocompleteItemVM>(hints.Select(SetupAutocompleteItem));
            }
            catch (OperationCanceledException)
            {
                throw;
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

        #endregion
    }
}
