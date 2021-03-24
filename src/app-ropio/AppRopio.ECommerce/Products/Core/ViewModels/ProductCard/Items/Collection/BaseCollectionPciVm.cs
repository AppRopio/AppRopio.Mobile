using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Models.Products.Responses;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection
{
    public abstract class BaseCollectionPciVm<TItemVM, TValue> : ProductDetailsItemVM, IBaseCollectionPciVm<TItemVM, TValue>
        where TItemVM : class, IMvxViewModel
        where TValue : class
    {
        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<TItemVM>(OnItemSelected));
            }
        }

        #endregion

        #region Properties

        public List<TValue> Values { get; protected set; }

        protected CancellationTokenSource CTS { get; set; }

        private ObservableCollection<TItemVM> _items;
        public ObservableCollection<TItemVM> Items
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

        #endregion

        #region Constructor

        protected BaseCollectionPciVm(ProductParameter filter)
            : base(filter)
        {
            Items = new ObservableCollection<TItemVM>();
        }

        #endregion

        #region Protected

        protected virtual void LoadContent()
        {
            Items = new ObservableCollection<TItemVM>(Values.Select(x => SetupItem(this.DataType, x)));
            BuildSelectedValue(false);
        }

        protected abstract TItemVM SetupItem(ProductDataType dataType, TValue value);

        protected abstract Task BuildSelectedValue(bool withNotifyPropertyChanged = true);

        protected abstract void OnItemSelected(TItemVM item);

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() => LoadContent());
        }

        #region IFiltersItemVm implementation

        public override void ClearSelectedValue()
        {
        }

        #endregion

        #endregion
    }
}
