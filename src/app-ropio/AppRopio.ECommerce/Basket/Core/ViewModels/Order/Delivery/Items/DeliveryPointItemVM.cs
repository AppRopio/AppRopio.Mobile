using System;
using System.Windows.Input;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Basket.Responses.Order;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.ExternalMaps;
using Plugin.Messaging;
using AppRopio.Base.Core.Services.Analytics;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items
{
    public class DeliveryPointItemVM : MvxViewModel, IDeliveryPointItemVM
    {
        #region Fields

        private DeliveryPointBundle _bundle;

        #endregion

        #region Commands

        private ICommand _callCommand;
        public ICommand CallCommand
        {
            get
            {
                return _callCommand ?? (_callCommand = new MvxCommand(OnCallExecute));
            }
        }

        private ICommand _infoCommand;
        public ICommand AdditionalInfoCommand
        {
            get
            {
                return _infoCommand ?? (_infoCommand = new MvxCommand(OnAdditionalInfoExecute));
            }
        }

        private ICommand _mapCommand;
        public ICommand MapCommand
        {
            get
            {
                return _mapCommand ?? (_mapCommand = new MvxCommand(OnMapExecute));
            }
        }

        private ICommand _routeCommand;
        public ICommand RouteCommand
        {
            get
            {
                return _routeCommand ?? (_routeCommand = new MvxCommand(OnRouteExecute));
            }
        }

        #endregion

        #region Properties

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Address { get; private set; }

        public string WorkTime { get; private set; }

        public string Distance { get; private set; }

        public string Phone { get; private set; }

        public string AdditionalInfo { get; private set; }

        public Coordinates Coordinates { get; private set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        #endregion

        #region Constructor

        public DeliveryPointItemVM(DeliveryPoint model)
        {
            var orderType = Mvx.Resolve<IBasketConfigService>().Config.OrderViewType;
            _bundle = new DeliveryPointBundle(model, orderType == Enums.OrderViewType.Partial ? NavigationType.PresentModal : NavigationType.Push);

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Id = model.Id;
            Name = model.Name;
            Address = model.Address;
            WorkTime = model.WorkTime;
            Distance = model.Distance;
            Phone = model.Phone;
            AdditionalInfo = model.AdditionalInfo;
            Coordinates = model.Coordinates;
        }

        public DeliveryPointItemVM(DeliveryPointBundle bundle)
        {
            _bundle = bundle;

            if (bundle == null)
                throw new ArgumentNullException(nameof(bundle));

            Id = bundle.Id;
            Name = bundle.Name;
            Address = bundle.Address;
            WorkTime = bundle.WorkTime;
            Distance = bundle.Distance;
            Phone = bundle.Phone;
            AdditionalInfo = bundle.Info;
            if (bundle.Latitude > 0 && bundle.Longitude > 0)
            {
                Coordinates = new Coordinates
                {
                    Latitude = bundle.Latitude,
                    Longitude = bundle.Longitude
                };
            }
        }

        #endregion

        #region Protected

        protected virtual async void OnCallExecute()
        {
            if (Mvx.CanResolve<IAnalyticsNotifyingService>())
                Mvx.Resolve<IAnalyticsNotifyingService>().NotifyEventIsHandled("order", "order_delivery_pickup_item_button");

            if (await Mvx.Resolve<IUserDialogs>().Confirm($"Набрать {Phone}?", "Набрать")
                        && CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
            {
                CrossMessaging.Current.PhoneDialer.MakePhoneCall(Phone, Name); 
            }
            else
            {
                await Mvx.Resolve<IUserDialogs>().Alert("Ваше устройство не поддерживает функцию звонка");
            }
        }

        protected virtual void OnAdditionalInfoExecute()
        {
            Mvx.Resolve<IBasketNavigationVmService>().NavigateToDeliveryPointAdditionalInfo(_bundle);
        }

        protected virtual void OnMapExecute()
        {
            Mvx.Resolve<IBasketNavigationVmService>().NavigateToDeliveryPointOnMap(_bundle);
        }

        protected virtual void OnRouteExecute()
        {
            if (Coordinates == null)
                return;

            InvokeOnMainThread(async () =>
            {
                var success = await CrossExternalMaps.Current.NavigateTo(Name, Coordinates.Latitude, Coordinates.Longitude);
                if (!success)
                    await Mvx.Resolve<IUserDialogs>().Alert("Не удалось построить маршрут.");
            });
        }

        #endregion
    }
}
