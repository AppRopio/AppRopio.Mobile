using System;
using AppRopio.Base.Core.ViewModels;
using System.Collections.ObjectModel;
using AppRopio.Models.Filters.Responses;
using AppRopio.Base.Filters.Core.ViewModels.Sort.Items;
using System.Windows.Input;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using MvvmCross;
using AppRopio.Base.Filters.Core.ViewModels.Sort.Services;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Filters.Core.Models.Bundle;
using System.Linq;

namespace AppRopio.Base.Filters.Core.ViewModels.Sort
{
    public class SortViewModel : BaseViewModel, ISortViewModel
    {
        #region Fields

        private string _categoryId;

        private string _selectedSortId;

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<ISortItemVM>(OnItemSelected));
            }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new MvxCommand(OnCancelExecute));
            }
        }

        #endregion

        #region Properties

        private ObservableCollection<ISortItemVM> _items;
        public ObservableCollection<ISortItemVM> Items
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

        private SortType _selectedSort;
        public SortType SelectedSort
        {
            get
            {
                return _selectedSort;
            }
            set
            {
                _selectedSort = value;
                RaisePropertyChanged(() => SelectedSort);
            }
        }

        #endregion

        #region Services

        protected ISortVmService VmService { get { return Mvx.Resolve<ISortVmService>(); } }

        #endregion

        #region Constructor

        public SortViewModel()
        {
            Items = new ObservableCollection<ISortItemVM>();
        }

        #endregion

        #region Private

        private async Task LoadContent()
        {
            Loading = true;

            var items = await VmService.LoadSortTypesInCategory(_categoryId, _selectedSortId);

            Loading = false;

            Items = items;
        }

        private void OnItemSelected(ISortItemVM item)
        {
            Close(this);

            var selectedItem = Items.FirstOrDefault(x => x.Selected);
            if (selectedItem != null && !selectedItem.Equals(item))
                selectedItem.Selected = false;

            item.Selected = true;

            VmService.ChangeSortTypeTo(_categoryId, item.Sort);
        }

        private void OnCancelExecute()
        {
            FireCancelCommandExecute();

            Close(this);
        }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var categoryBundle = parameters.ReadAs<SortBundle>();
            this.InitFromBundle(categoryBundle);
        }

        protected virtual void InitFromBundle(SortBundle parameters)
        {
            _categoryId = parameters.CategoryId;
            _selectedSortId = parameters.SelectedSortId;
            VmNavigationType = parameters.NavigationType;
        }

        #endregion

        protected virtual void FireCancelCommandExecute() { }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        #endregion
    }
}
