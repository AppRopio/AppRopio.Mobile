using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection.Items;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Messages;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Commands;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection
{
    public class MultiSelectionFiVm : BaseSelectionFiVm, IMultiSelectionFiVm
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

        public MultiSelectionFiVm(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
            : base(filter, applyedFilterValues)
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
                Messenger.Publish(new FiltersReloadWhenValueChangedMessage(this));
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
            ApplyedFilterValues = new List<ApplyedFilterValue>();
            Items = new ObservableCollection<MultiCollectionItemVM>();
        }

        #endregion

        #region BaseSelectionFiVm implementation

        protected override void OnSelectionMessageReceived(FiltersSelectionChangedMessage msg)
        {
            if (this.Id != msg.FilterId)
                return;

            var newSelectedValues = msg.ApplyedFilterValues;
            if (newSelectedValues.IsNullOrEmpty())
                ClearSelectedValue();
            else
            {
                SelectedValue = new ApplyedFilter
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

            Messenger.Publish(new FiltersReloadWhenValueChangedMessage(this));
        }

        #endregion

        #endregion
    }
}
