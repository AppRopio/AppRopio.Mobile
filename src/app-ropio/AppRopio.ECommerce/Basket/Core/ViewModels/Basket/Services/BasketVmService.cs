using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Messages.Localization;
using AppRopio.Base.Core.Services.TasksQueue;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items;
using AppRopio.Models.Basket.Responses.Basket;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Services
{
    public class BasketVmService : BaseVmService, IBasketVmService
    {
        #region Fields

        private CancellationTokenSource _basketCTS;

        private CancellationTokenSource _basketAmountCTS;

        #endregion

        #region Protected

        protected virtual IBasketItemVM SetupItem(BasketItem model)
        {
            return new BasketItemVM(model);
        }

        #endregion

        #region Services

        private IProductDeleteVmService _deleteVmService;
        public IProductDeleteVmService DeleteVmService => _deleteVmService ?? (_deleteVmService = Mvx.IoCProvider.Resolve<IProductDeleteVmService>());

        public IBasketService ApiService => Mvx.IoCProvider.Resolve<IBasketService>();

        #endregion

        #region Protected

        protected override void OnLanguageChanged(LanguageChangedMessage msg)
        {
            base.OnLanguageChanged(msg);

            LoadedBasket = null;
        }

        #endregion

        #region IBasketVmService implementation

        public BasketModel LoadedBasket { get; private set; }

        public async Task<ObservableCollection<IBasketItemVM>> LoadItemsIfNeeded(string basketVersionId = null)
        {
            var source = new ObservableCollection<IBasketItemVM>();

            if (_basketCTS != null)
                _basketCTS.Cancel();
            _basketCTS = new CancellationTokenSource();

            try
            {
                var isNeedToLoad = await Mvx.IoCProvider.Resolve<ITasksQueueService>()
                                            .Append(() => ApiService
                                                    .IsNeedToLoad(basketVersionId, _basketCTS.Token), _basketCTS.Token);
                if (isNeedToLoad)
                {
                    LoadedBasket = await Mvx.IoCProvider.Resolve<ITasksQueueService>()
                                          .Append(() => ApiService
                                                  .GetBasket(_basketCTS.Token), _basketCTS.Token);

                    if (LoadedBasket != null && !LoadedBasket.Items.IsNullOrEmpty())
                        source = new ObservableCollection<IBasketItemVM>(LoadedBasket.Items.Select(SetupItem));
                }
                else if (LoadedBasket != null && !LoadedBasket.Items.IsNullOrEmpty())
                    source = new ObservableCollection<IBasketItemVM>(LoadedBasket.Items.Select(SetupItem));
            }
            catch (OperationCanceledException)
            {
                //TODO
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return source;
        }

        public Task<bool> DeleteItem(string productId)
        {
            return DeleteVmService.DeleteProduct(productId);
        }

        public IMvxViewModel LoadLoyaltyVmIfExist()
        {
            var config = Mvx.IoCProvider.Resolve<IBasketConfigService>().Config;

            if (config.Loyalty != null)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(config.Loyalty.AssemblyName));

                    var loyaltyType = assembly.GetType(config.Loyalty.TypeName);

                    var loyaltyInstance = Activator.CreateInstance(loyaltyType);

                    return loyaltyInstance as IMvxViewModel;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{nameof(BasketVmService)}: {ex.Message}");
                }
                catch (FileNotFoundException ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{nameof(BasketVmService)}: {ex.Message}");
                }
                catch
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{nameof(BasketVmService)}: Can't load loyalty vm");
                }
            }

            return null;
        }

        public async Task<bool> IsBasketValid(string versionId, CancellationToken token)
        {
            bool isValid = false;

            try
            {
                var validity = await ApiService.CheckBasketValidity(versionId, token);

                isValid = validity.IsValid;

                if (!isValid)
                    await UserDialogs.Error(validity.NotValidMessage);
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return isValid;
        }

        public async Task<decimal> LoadBasketSummaryAmount()
        {
            var amountResponse = new BasketAmount();

            try
            {
                if (_basketAmountCTS != null)
                {
                    _basketAmountCTS.Cancel();
                    _basketAmountCTS = null;
                }

                _basketAmountCTS = new CancellationTokenSource();

                amountResponse = await Mvx.IoCProvider.Resolve<ITasksQueueService>()
                                          .Append(() => ApiService.GetBasketSummaryAmount(_basketAmountCTS.Token), _basketAmountCTS.Token);
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

            return amountResponse?.Amount ?? 0;
        }

        #endregion
    }
}
