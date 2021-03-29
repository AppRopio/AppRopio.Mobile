using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Filters.Core.Models;
using AppRopio.Base.Filters.Core.Models.Bundle;
using AppRopio.Base.Filters.Core.Services;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Messages;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Services;
using AppRopio.Models.Filters.Responses;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Commands;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters
{
    public class FiltersViewModel : BaseViewModel, IFiltersViewModel
    {
        #region Fields

        private string _categoryId;

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IFiltersItemVM>(OnItemSelected));
            }
        }

        private ICommand _applyCommand;
        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new MvxAsyncCommand(OnApplyExecute));
            }
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new MvxAsyncCommand(OnClearExecute));
            }
        }

        #endregion

        #region Properties

        protected List<ApplyedFilter> ApplyedFilters { get; set; }

        protected FiltersConfig Config { get { return Mvx.IoCProvider.Resolve<IFiltersConfigService>().Config; } }

        private ObservableCollection<IFiltersItemVM> _items;
        public ObservableCollection<IFiltersItemVM> Items
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

        public string ApplyTitle => LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Filters_Apply");

        #endregion

        #region Services

        protected IFiltersVmService VmService { get { return Mvx.IoCProvider.Resolve<IFiltersVmService>(); } }

        protected IFiltersNavigationVmService NavigationVmService { get { return Mvx.IoCProvider.Resolve<IFiltersNavigationVmService>(); } }

        #endregion

        #region Constructor

        public FiltersViewModel()
        {
            VmNavigationType = Base.Core.Models.Navigation.NavigationType.Push;
            Items = new ObservableCollection<IFiltersItemVM>();
        }

        #endregion

        #region Private

        private async Task LoadContent()
        {
            Loading = true;

            var dataSource = await VmService.LoadFiltersFor(_categoryId, ApplyedFilters);

            InvokeOnMainThread(() => Items = dataSource);

            Loading = false;
        }

        private void OnItemSelected(IFiltersItemVM item)
        {
            var selectableItem = item as ISelectableFilterItemVM;
            if (selectableItem != null)
                selectableItem.OnSelected();
        }

        #endregion

        #region Protected

        protected virtual async Task OnApplyExecute()
        {
            if (Items != null && Items.Any())
            {
                ApplyedFilters = Items
	                .Where(x => x.SelectedValue != null)
	                .Select(x => x.SelectedValue)
	                .ToList();

                VmService.ChangeFiltersTo(_categoryId, ApplyedFilters);
            }

            await NavigationVmService.Close(this);
        }

        protected virtual async Task OnClearExecute()
        {
            ApplyedFilters = new List<ApplyedFilter>();

            Items?.ForEach(x => x.ClearSelectedValue());

            Messenger.Publish(new FiltersReloadWhenValueChangedMessage(this));

            if (Config.ApplyFiltersWhenClearingUp)
            {
                VmService.ChangeFiltersTo(_categoryId, ApplyedFilters);

                await NavigationVmService.Close(this);
            }
        }

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var filtersBundle = parameters.ReadAs<FiltersBundle>();
            this.InitFromBundle(filtersBundle);
        }

        protected virtual void InitFromBundle(FiltersBundle parameters)
        {
            _categoryId = parameters.CategoryId;
            VmNavigationType = parameters.NavigationType;
            ApplyedFilters = parameters.ApplyedFilters;
        }

        #endregion

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        #endregion

    }
}
