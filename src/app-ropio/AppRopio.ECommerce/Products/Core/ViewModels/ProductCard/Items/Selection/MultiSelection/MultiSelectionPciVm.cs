using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection.Items;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection
{
    public class MultiSelectionPciVm : BaseSelectionPciVm, IMultiSelectionPciVm
    {
        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<MultiCollectionItemVM>(OnItemSelected));
            }
        }

        #endregion

        #region Properties

        private ObservableCollection<MultiCollectionItemVM> _items;
        public ObservableCollection<MultiCollectionItemVM> Items
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

        public MultiSelectionPciVm(ProductParameter parameter)
            : base(parameter)
        {
            Items = new ObservableCollection<MultiCollectionItemVM>();
        }

        #endregion

        #region Private

        private void LoadContent()
        {
            Items = ApplyedFilterValues == null ?
                new ObservableCollection<MultiCollectionItemVM>()
                :
                new ObservableCollection<MultiCollectionItemVM>(
                    this.Values
                        .Where(x => this.ApplyedFilterValues.Any(y => y.Id == x.Id))
                        .Select(z => new MultiCollectionItemVM(z.Id, z.ValueName))
            );
        }

        private void OnItemSelected(MultiCollectionItemVM item)
        {
            Items.Remove(item);

            SelectedValue.Values.RemoveAll(x => x.Id == item.Id);

            if (Items.IsNullOrEmpty())
                Messenger.Publish(new ProductDetailsReloadWhenValueChangedMessage(this));
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() => LoadContent());
        }

        #region IFilterItemVM implementation

        public override void ClearSelectedValue()
        {
            SelectedValue = null;
            ApplyedFilterValues = new List<ApplyedProductParameterValue>();
            Items = new ObservableCollection<MultiCollectionItemVM>();
        }

        #endregion

        #region BaseSelectionFiVm implementation

        protected override void OnSelectionMessageReceived(ProductDetailsSelectionChangedMessage msg)
        {
            if (this.Id != msg.ParameterId)
                return;

            var newSelectedValues = msg.ApplyedParameterValues;
            if (newSelectedValues.IsNullOrEmpty())
                ClearSelectedValue();
            else
            {
                SelectedValue = new ApplyedProductParameter
                {
                    Id = this.Id,
                    DataType = this.DataType,
                    Values = newSelectedValues
                };

                Items = new ObservableCollection<MultiCollectionItemVM>(Values
                                                                        .Where(x => newSelectedValues.Any(y => y.Id == x.Id))
                                                                        .Select(z => new MultiCollectionItemVM(z.Id, z.ValueName))
                                                                       );
            }

            Messenger.Publish(new ProductDetailsReloadWhenValueChangedMessage(this));
        }

        #endregion

        #endregion
    }
}
