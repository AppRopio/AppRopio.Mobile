using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.Base.Core.ViewModels.Selection.Items;
using AppRopio.Base.Core.ViewModels.Selection.Services;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Core.ViewModels.Selection
{
    public abstract class BaseSelectionViewModel<TValue, TSelectedValue> : SearchViewModel, IBaseSelectionViewModel
        where TValue : class
        where TSelectedValue : class
    {
        #region Fields

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<ISelectionItemVM>(OnItemSelected));
            }
        }

        private ICommand _applyCommand;
        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new MvxCommand(OnApplyExecute));
            }
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new MvxCommand(OnClearExecute));
            }
        }

        #endregion

        #region Properties

        protected List<TValue> Values { get; set; }

        protected List<TSelectedValue> SelectedValues { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private ObservableCollection<ISelectionItemVM> _items;
        public ObservableCollection<ISelectionItemVM> Items
        {
            get => _items;
            set => SetProperty(ref _items, value, nameof(Items));
        }

        public abstract string ApplyTitle { get; }

        #endregion

        #region Services

        protected abstract IBaseSelectionVmService<TValue, TSelectedValue> VmService { get; }

        #endregion

        #region Constructor

        public BaseSelectionViewModel()
        {
            VmNavigationType = Base.Core.Models.Navigation.NavigationType.Push;
            Items = new ObservableCollection<ISelectionItemVM>();
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected abstract void OnItemSelected(ISelectionItemVM item);

        protected abstract void OnApplyExecute();

        protected abstract void OnClearExecute();

        protected virtual async Task LoadContent()
        {
            Loading = true;

            Items = await VmService.LoadItemsFor(Values, SelectedValues, SearchText);

            Loading = false;
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() => LoadContent());
        }

        #endregion
    }
}
