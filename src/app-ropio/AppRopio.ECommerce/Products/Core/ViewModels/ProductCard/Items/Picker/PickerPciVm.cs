using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker.Items;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker
{
    public class PickerPciVm : ProductDetailsItemVM, IPickerPciVm
    {
        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<PickerCollectionItemVM>(OnItemSelected));
            }
        }

        #endregion

        #region Properties

        protected List<ProductParameterValue> Values { get; set; }

        protected CancellationTokenSource CTS { get; set; }

        private List<PickerCollectionItemVM> _items;
        public List<PickerCollectionItemVM> Items
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

        private string _valueName;
        public string ValueName
        {
            get
            {
                return _valueName;
            }
            set
            {
                _valueName = value;
                RaisePropertyChanged(() => ValueName);
            }
        }

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            protected set
            {
                SetProperty(ref _selected, value);
            }
        }

        public int SelectedItemIndex { get; protected set; }

        public PickerCollectionItemVM SelectedItem { get { return Items.IsNullOrEmpty() ? null : Items[SelectedItemIndex]; } }

        #endregion

        #region Services

        protected IMvxMessenger MessengerService { get { return Mvx.Resolve<IMvxMessenger>(); } }

        #endregion

        #region Constructor

        public PickerPciVm(ProductParameter filter)
            : base(filter)
        {
            Values = filter.Values;
        }

        #endregion

        #region Private

        private void LoadContent()
        {
            InvokeOnMainThread(() => Items = new List<PickerCollectionItemVM>(Values.Select(x => SetupItem(x))));
        }

        private void OnItemSelected(PickerCollectionItemVM item)
        {
            SelectedItemIndex = Items.IndexOf(item);

            ValueName = item.ValueName;

            try
            {
                BuildSelectedValue(item);
            }
            catch (System.OperationCanceledException)
            {
                SelectedValue = null;
            }
        }

        #endregion

        #region Protected

        protected virtual PickerCollectionItemVM SetupItem(ProductParameterValue value)
        {
            return new PickerCollectionItemVM(value);
        }

        protected Task BuildSelectedValue(PickerCollectionItemVM item)
        {
            if (CTS != null)
                CTS.Cancel(false);

            CTS = new CancellationTokenSource();

            return Task.Run(() =>
            {
                SelectedValue = new ApplyedProductParameter
                {
                    Id = this.Id,
                    DataType = this.DataType,
                    Values = new List<ApplyedProductParameterValue> { new ApplyedProductParameterValue { Id = item.Id } }
                };
            }, CTS.Token);
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() => LoadContent());
        }

        #region IFiltersItemVm implementation

        public override void ClearSelectedValue()
        {
            SelectedValue = null;
            Selected = false;
            ValueName = string.Empty;
        }

        #endregion

        #region IPickerFiVm implementation

        public void OnSelected()
        {
            Selected = !Selected;

            if (SelectedItemIndex < Items.Count)
                ValueName = Items[SelectedItemIndex].ValueName;

            MessengerService.Publish(new ProductDetailsReloadWhenValueChangedMessage(this));
        }

        #endregion

        #endregion
    }
}
