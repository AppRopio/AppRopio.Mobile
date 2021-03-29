using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker.Items;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Messages;
using AppRopio.Models.Filters.Responses;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker
{
    public class PickerFiVm : FiltersItemVm, IPickerFiVm
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

        protected List<FilterValue> Values { get; set; }

        protected List<ApplyedFilterValue> ApplyedFilterValues { get; set; }

        protected CancellationTokenSource CTS { get; set; }

        public override ApplyedFilter SelectedValue { get; protected set; }

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

        protected IMvxMessenger MessengerService { get { return Mvx.IoCProvider.Resolve<IMvxMessenger>(); } }

        #endregion

        #region Constructor

        public PickerFiVm(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
            : base(filter)
        {
            Values = filter.Values;
            ApplyedFilterValues = applyedFilterValues;
        }

        #endregion

        #region Private

        private void LoadContent()
        {
            if (!ApplyedFilterValues.IsNullOrEmpty())
            {
                var selectedValue = Values.FirstOrDefault(x => ApplyedFilterValues.Any(y => y.Id == x.Id));
                if (selectedValue != null)
                {
                    SelectedItemIndex = Values.IndexOf(selectedValue);
                    ValueName = selectedValue.ValueName;
                }
            }

            Items = new List<PickerCollectionItemVM>(Values.Select(x => SetupItem(x)));

            BuildSelectedValue(SelectedItem);
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

        protected virtual PickerCollectionItemVM SetupItem(FilterValue value)
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
                SelectedValue = new ApplyedFilter
                {
                    Id = this.Id,
                    DataType = this.DataType,
                    Values = new List<ApplyedFilterValue> { new ApplyedFilterValue { Id = item.Id } }
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
            ApplyedFilterValues = new List<ApplyedFilterValue>();
        }

        #endregion

        #region IPickerFiVm implementation

        public void OnSelected()
        {
            Selected = !Selected;

            if (SelectedItemIndex < Items.Count)
                ValueName = Items[SelectedItemIndex].ValueName;

            MessengerService.Publish(new FiltersReloadWhenValueChangedMessage(this));
        }

        #endregion

        #endregion
    }
}
