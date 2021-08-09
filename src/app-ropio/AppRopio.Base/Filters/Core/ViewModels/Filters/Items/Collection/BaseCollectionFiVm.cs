using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Commands;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection
{
    public abstract class BaseCollectionFiVm : FiltersItemVm, IBaseCollectionFiVm
    {
        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<CollectionItemVM>(OnItemSelected));
            }
        }

        #endregion

        #region Properties

        public override ApplyedFilter SelectedValue { get; protected set; }

        public List<FilterValue> Values { get; set; }

        protected List<ApplyedFilterValue> ApplyedFilterValues { get; set; }

        protected CancellationTokenSource CTS { get; set; }

        private ObservableCollection<CollectionItemVM> _items;
        public ObservableCollection<CollectionItemVM> Items
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

        protected BaseCollectionFiVm(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
            : base(filter)
        {
            Values = filter.Values;
            ApplyedFilterValues = applyedFilterValues;

            Items = new ObservableCollection<CollectionItemVM>();
        }

        #endregion

        #region Protected

        protected virtual void LoadContent()
        {
            Items = new ObservableCollection<CollectionItemVM>(Values
                                                               .Select(x => SetupItem(this.DataType, x, ApplyedFilterValues != null && ApplyedFilterValues.Any(y => y.Id == x.Id)))
                                                              );
            BuildSelectedValue();
        }

        protected CollectionItemVM SetupItem(FilterDataType dataType, FilterValue value, bool selected)
        {
            return new CollectionItemVM(dataType, value, selected);
        }

        protected Task BuildSelectedValue()
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
                    Values = Items.Where(x => x.Selected).Select(y => new ApplyedFilterValue { Id = y.Id }).ToList()
                };
            }, CTS.Token);
        }

        protected abstract void OnItemSelected(CollectionItemVM item);

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() => LoadContent());
        }

        #region IFiltersItemVm implementation

        public override void ClearSelectedValue()
        {
            Task.Run(() =>
            {
                SelectedValue = null;
                ApplyedFilterValues = new List<ApplyedFilterValue>();

                Items.ForEach(x => InvokeOnMainThread(() => x.Selected = false));
            });
        }

        #endregion

        #endregion
    }
}
