using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Basket.Responses.Order;
using MvvmCross.ViewModels;
using MvvmCross;
using DeliveryModel = AppRopio.Models.Basket.Responses.Order.Delivery;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services.Implementation
{
    public class DeliveryVmService : BaseVmService, IDeliveryVmService
    {
        #region Protected

        protected virtual IOrderFieldItemVM SetupOrderFieldItem(OrderField model)
        {
            return new OrderFieldItemVM(model);
        }

        protected virtual IDeliveryTypeItemVM SetupDeliveryItem(DeliveryModel model)
        {
            return new DeliveryTypeItemVM(model);
        }

        protected virtual IDeliveryPointItemVM SetupDeliveryPointItem(DeliveryPoint model)
        {
            return new DeliveryPointItemVM(model);
        }

        protected virtual IDeliveryDayItemVM SetupDeliveryDayItem(DeliveryDay model)
        {
            return new DeliveryDayItemVM(model);
        }

        #endregion

        #region Services

        protected IDeliveryService DeliveryService => Mvx.IoCProvider.Resolve<IDeliveryService>();

        #endregion

        #region IDeliveryVmService implementation

        public async Task<MvxObservableCollection<IDeliveryTypeItemVM>> LoadDeliveryTypes()
        {
            MvxObservableCollection<IDeliveryTypeItemVM> source = null;

            try
            {
                var deliveries = await DeliveryService.GetDeliveries();
                if (!deliveries.IsNullOrEmpty())
                    source = new MvxObservableCollection<IDeliveryTypeItemVM>(deliveries.Select(SetupDeliveryItem));
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

        public async Task<MvxObservableCollection<IDeliveryPointItemVM>> LoadDeliveryPoints(string deliveryId, string searchText)
        {
            MvxObservableCollection<IDeliveryPointItemVM> source = null;

            try
            {
                var deliveryPoints = await DeliveryService.GetDeliveryPoints(deliveryId, searchText);
                source = new MvxObservableCollection<IDeliveryPointItemVM>(deliveryPoints.Select(SetupDeliveryPointItem));
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

        public async Task<MvxObservableCollection<IOrderFieldItemVM>> LoadDeliveryAddressFields(string deliveryId, Coordinates coordinates = null)
        {
            MvxObservableCollection<IOrderFieldItemVM> source = null;

            try
            {
                var addressFields = await DeliveryService.GetDeliveryAddressFields(deliveryId, coordinates);
                source = new MvxObservableCollection<IOrderFieldItemVM>(addressFields.Select(SetupOrderFieldItem));
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

        public async Task<bool> ValidateAndSaveDeliveryAddressFields(string deliveryId, MvxObservableCollection<IOrderFieldItemVM> fields)
        {
            var result = false;
            try
            {
                var validation = await DeliveryService.ConfirmDeliveryAddress(deliveryId, fields.ToDictionary(x => x.Id, x => x.Value));

                if (validation == null || (validation.NotValidFields.IsNullOrEmpty() && validation.Error.IsNullOrEmtpy()))
                    result = true;
                else
                {
                    foreach (var notValidField in validation.NotValidFields)
                    {
                        var field = fields.FirstOrDefault(x => x.Id.Equals(notValidField.Id));
                        field.IsValid = false;
                    }

                    if (!validation.Error.IsNullOrEmpty())
                        await UserDialogs.Error(validation.Error);
                }
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return result;
        }

        public async Task<bool> ValidateAndSaveDeliveryPoint(string deliveryId, string deliveryPointId)
        {
            var result = false;
            try
            {
                await DeliveryService.ConfirmDeliveryPoint(deliveryId, deliveryPointId);
                result = true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return result;
        }

        public async Task<decimal?> LoadDeliveryPrice(string deliveryId)
        {
            decimal? deliveryPrice = null;

            try
            {
                deliveryPrice = await DeliveryService.GetDeliveryPrice(deliveryId);
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return deliveryPrice;
        }

        public async Task<MvxObservableCollection<IDeliveryDayItemVM>> LoadDeliveryTime(string deliveryId)
        {
            MvxObservableCollection<IDeliveryDayItemVM> source = null;

            try
            {
                var deliveryDays = await DeliveryService.GetDeliveryTime(deliveryId);
                source = new MvxObservableCollection<IDeliveryDayItemVM>(deliveryDays.Select(SetupDeliveryDayItem));
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

        public async Task<bool> ConfirmDeliveryTime(string deliveryTimeId)
        {
            var result = false;
            try
            {
                await Mvx.IoCProvider.Resolve<IDeliveryService>().ConfirmDeliveryTime(deliveryTimeId);
                result = true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return result;
        }

        public async Task<bool> ValidateAndSaveDelivery(string id)
        {
            var result = false;
            try
            {
                await Mvx.IoCProvider.Resolve<IDeliveryService>().ConfirmDelivery(id);
                result = true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return result;
        }

        #endregion
    }
}
