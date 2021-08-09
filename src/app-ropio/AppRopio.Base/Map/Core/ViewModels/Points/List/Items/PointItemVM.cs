using System;
using System.Windows.Input;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.Services.Launcher;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.Base.Map.Core.Models.Bundle;
using AppRopio.Base.Map.Core.Services;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Map.Responses;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace AppRopio.Base.Map.Core.ViewModels.Points.List.Items
{
    public class PointItemVM : MvxViewModel, IPointItemVM
    {
        #region Fields

        private PointBundle _bundle;

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

        #region Services

        protected ILocalizationService LocalizationService => Mvx.IoCProvider.Resolve<ILocalizationService>();

        protected ILauncherService LauncherService => Mvx.IoCProvider.Resolve<ILauncherService>();

        #endregion

        #region Constructor

        public PointItemVM(Point model)
        {
            _bundle = new PointBundle(model, NavigationType.PresentModal);

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

        public PointItemVM(PointBundle bundle)
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
            if (Phone.IsNullOrEmtpy())
                return;

            if (await Mvx.IoCProvider.Resolve<IUserDialogs>().Confirm($"{LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "List_CallTo")} {Phone}?", LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "List_Call")))
            {
                if (!await LauncherService.LaunchPhone(Phone))
                {
                    await Mvx.IoCProvider.Resolve<IUserDialogs>().Alert(LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "List_CallError"));
                    return;
                }
            }
        }

        protected virtual void OnAdditionalInfoExecute()
        {
            Mvx.IoCProvider.Resolve<IMapNavigationVmService>().NavigateToPointAdditionalInfo(_bundle);
        }

        protected virtual void OnRouteExecute()
        {
            if (Coordinates == null)
                return;

            InvokeOnMainThread(async () =>
            {
                try
                {
                    var options = new MapLaunchOptions()
                    {
                        Name = Name,
                        NavigationMode = NavigationMode.Driving
                    };
                    await Xamarin.Essentials.Map.OpenAsync(Coordinates.Latitude, Coordinates.Longitude, options);
                }
                catch
                {
                    await Mvx.IoCProvider.Resolve<IUserDialogs>().Alert(LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "List_RouteError"));
                }
            });
        }

        #endregion
    }
}
