using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.Base.Settings.Core.Models.Bundle;
using AppRopio.Base.Settings.Core.ViewModels.Messages;
using AppRopio.Base.Settings.Core.ViewModels.Regions.Items;
using AppRopio.Base.Settings.Core.ViewModels.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Settings.Core.ViewModels.Regions
{
    public class RegionsViewModel : SearchViewModel, IRegionsViewModel
    {
        #region Fields

        private MvxSubscriptionToken _subscribtionToken;

        protected string _selectedRegionId;

        protected string _selectedRegionTitle;

        #endregion

        #region Commands

        private IMvxCommand _selectionChangedCommand;
        public IMvxCommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IRegionItemVm>(OnItemSelected));
            }
        }

        #endregion

        #region Properties

        private MvxObservableCollection<IRegionGroupItemVm> _regions;
        public MvxObservableCollection<IRegionGroupItemVm> Regions
        {
            get { return _regions; }
            set { SetProperty(ref _regions, value); }
        }

        private IRegionItemVm _selectedRegion;
        public IRegionItemVm SelectedRegion
        {
            get { return _selectedRegion; }
            set { SetProperty(ref _selectedRegion, value); }
        }

        #endregion

        #region Services

        protected ISettingsVmService VmService { get { return Mvx.Resolve<ISettingsVmService>(); } }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<SettingsPickerBundle>();
            this.InitFromBundle(navigationBundle);
        }

        protected virtual void InitFromBundle(SettingsPickerBundle parameters)
        {
            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush :
                                                            parameters.NavigationType;

            _selectedRegionId = parameters.Id;
            _selectedRegionTitle = parameters.Title;
        }

        #endregion

        protected virtual async Task LoadContent()
        {
            Loading = true;

            var regions = await VmService.LoadRegions();
            InvokeOnMainThread(() =>
            {
                Regions = regions;
                if (!Regions.IsNullOrEmpty() && !string.IsNullOrEmpty(_selectedRegionId))
                {
                    SelectedRegion = Regions.SelectMany(c => c.Items).FirstOrDefault(c => c.Id == _selectedRegionId);
                    if (SelectedRegion != null)
                    {
                        SelectedRegion.Selected = true;
                    }
                }
            });

            Loading = false;
        }

        protected virtual void OnItemSelected(IRegionItemVm item)
        {
            if (SelectedRegion != null)
                SelectedRegion.Selected = false;

            item.Selected = !item.Selected;
            SelectedRegion = item;

            VmService.ChangeSelectedRegion(item);


        }

        protected virtual void OnSettingsReloadMessageReceived(SettingsReloadMessage msg)
        {
            if (msg != null && msg.ElementType == Models.SettingsElementType.Region)
            {
                if (SelectedRegion != null)
                    SelectedRegion.Selected = false;

                if (!Regions.IsNullOrEmpty())
                {
                    var region = Regions.SelectMany(r => r.Items).FirstOrDefault(r => r.Id == msg.Id);
                    if (region != null)
                    {
                        SelectedRegion = region;
                        region.Selected = true;
                    }
                }
            }
        }

        protected override void OnSearchTextChanged(string searchText)
        {

        }

        protected override async void SearchCommandExecute()
        {
            Loading = true;

            var results = await VmService.SearchRegions(SearchText);
            InvokeOnMainThread(() => Regions = results);

            Loading = false;
        }

        protected override async void CancelSearchExecute()
        {
            await LoadContent();
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            if (_subscribtionToken == null)
                _subscribtionToken = Messenger.Subscribe<SettingsReloadMessage>(OnSettingsReloadMessageReceived);

            return LoadContent();
        }

        public override void Unbind()
        {
            base.Unbind();

            if (_subscribtionToken != null)
            {
                _subscribtionToken.Dispose();
                _subscribtionToken = null;
            }
        }

        #endregion
    }
}
