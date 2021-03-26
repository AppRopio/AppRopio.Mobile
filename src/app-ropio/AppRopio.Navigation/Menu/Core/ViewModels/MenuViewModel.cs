using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Messages.Localization;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Navigation.Menu.Core.ViewModels.Items;
using AppRopio.Navigation.Menu.Core.ViewModels.Services;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Commands;
using MvvmCross.Logging;

namespace AppRopio.Navigation.Menu.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel, IMenuViewModel
    {
        #region Fields

        private MvxSubscriptionToken _languageToken;

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IMenuItemVM>(OnItemSelected));
            }
        }

        #endregion

        #region Properties

        private IMvxViewModel _headerVm;
        public IMvxViewModel HeaderVm
        {
            get
            {
                return _headerVm;
            }
            set
            {
                _headerVm = value;
                RaisePropertyChanged(() => HeaderVm);
            }
        }

        private MvxObservableCollection<IMenuItemVM> _items;
        public MvxObservableCollection<IMenuItemVM> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                RaisePropertyChanged(() => Items);
            }
        }

        private IMvxViewModel _footerVm;
        public IMvxViewModel FooterVm
        {
            get
            {
                return _footerVm;
            }
            set
            {
                _footerVm = value;
                RaisePropertyChanged(() => FooterVm);
            }
        }

        public Type DefaultViewModel
        {
            get
            {
                var defaultType = MenuService.DefaultViewModelType();
                MenuSettings.FirstLaunch = false;
                return defaultType;
            }
        }

        #endregion

        #region Services

        private IMenuVmService _menuService;
        protected IMenuVmService MenuService { get { return _menuService ?? (_menuService = Mvx.IoCProvider.Resolve<IMenuVmService>()); } }

        protected IRouterService RouterService { get { return Mvx.IoCProvider.Resolve<IRouterService>(); } }

        #endregion

        #region Constructor

        public MenuViewModel()
        {
            VmNavigationType = Base.Core.Models.Navigation.NavigationType.None;

            Items = new MvxObservableCollection<IMenuItemVM>();
        }

        #endregion

        #region Private

        private void LoadContent()
        {
            LoadHeaderVm();

            LoadMenuItems();

            LoadFooterVm();
        }

        private async void LoadHeaderVm()
        {
            try
            {
                HeaderVm = MenuService.LoadHeaderVmIfExist();

                await HeaderVm?.Initialize();
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error($"MenuViewModel: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Log.Error($"MenuViewModel: {ex.Message}");
            }
            catch
            {
                Log.Error("MenuViewModel: Can't load header vm");
            }
        }

        private void LoadMenuItems()
        {
            try
            {
                Items.Clear();
                Items.AddRange(MenuService.LoadItemsFromConfigJSON());
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error($"MenuViewModel: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Log.Error($"MenuViewModel: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"MenuViewModel: Can't load menu items\n{ex.BuildAllMessagesAndStackTrace()}");
            }
        }

        private async void LoadFooterVm()
        {
            try
            {
                FooterVm = MenuService.LoadFooterVmIfExist();

                await FooterVm?.Initialize();
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error($"MenuViewModel: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Log.Error($"MenuViewModel: {ex.Message}");
            }
            catch
            {
                Log.Error("MenuViewModel: Can't load footer vm");
            }
        }

        #endregion

        #region Protected

        protected void OnItemSelected(IMenuItemVM item)
        {
            AnalyticsNotifyingService.NotifyEventIsHandled("menu", "menu_item_selected", item.Type);

            if (!RouterService.NavigatedTo(item.Type, new BaseBundle(NavigationType.ClearAndPush)))
                Log.Error($"MenuViewModel: Can't navigate to ViewModel of type {item.Type}");
        }

        protected void OnLanguageChanged(LanguageChangedMessage obj)
        {
            LoadMenuItems();
        }

        #endregion

        #region Public

        public override void Prepare()
        {
            base.Prepare();

            _languageToken = Messenger.Subscribe<LanguageChangedMessage>(OnLanguageChanged);
        }

        public override Task Initialize()
        {
            return Task.Run(() => LoadContent());
        }

        public override void Unbind()
        {
            base.Unbind();

            if (_languageToken != null)
            {
                Messenger.Unsubscribe<LanguageChangedMessage>(_languageToken);
                _languageToken.Dispose();
                _languageToken = null;
            }
        }

        #endregion
    }
}

