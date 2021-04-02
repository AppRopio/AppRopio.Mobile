using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Models.Products.Responses;
using MvvmCross.Commands;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax
{
    public abstract class BaseMinMaxPciVm<T> : ProductDetailsItemVM, IBaseMinMaxPciVm<T>
        where T : IComparable, IComparable<T>, IEquatable<T>
    {
        #region Command

        private ICommand _minValueChangedCommand;
        public ICommand MinValueChangedCommand
        {
            get
            {
                return _minValueChangedCommand ?? (_minValueChangedCommand = new MvxCommand(OnMinValueChanged));
            }
        }

        private ICommand _maxValueChangedCommand;
        public ICommand MaxValueChangedCommand
        {
            get
            {
                return _maxValueChangedCommand ?? (_maxValueChangedCommand = new MvxCommand(OnMaxValueChanged));
            }
        }

        #endregion

        #region Properties

        protected string MinValueRaw { get; set; }

        protected string MaxValueRaw { get; set; }

        public T Min { get; private set; }

        public T Max { get; private set; }

        #endregion

        #region Constructor

        protected BaseMinMaxPciVm(ProductParameter parameter)
            : base(parameter)
        {
            MaxValueRaw = parameter.MaxValue;
            MinValueRaw = parameter.MinValue;
        }

        #endregion

        #region Protected

        protected virtual void OnMinValueChanged()
        {
            Task.Run(() =>
            {
                var minValue = FromString(MinValueRaw);
                var maxValue = FromString(MaxValueRaw);

                var minCompareValue = Min.CompareTo(Max) < 0 ? Min : Max;
                minCompareValue = minCompareValue.CompareTo(minValue) < 0 ? minValue : minCompareValue;
                minCompareValue = minCompareValue.CompareTo(maxValue) > 0 ? maxValue : minCompareValue;

                InvokeOnMainThread(() => Min = minCompareValue);

                SelectedValue.MinValue = ToRawString(Min);
                RaisePropertyChanged(() => SelectedValue);
            });
        }

        protected virtual void OnMaxValueChanged()
        {
            Task.Run(() =>
            {
                var minValue = FromString(MinValueRaw);
                var maxValue = FromString(MaxValueRaw);

                var maxCompareValue = Max.CompareTo(Min) > 0 ? Max : Min;
                maxCompareValue = maxCompareValue.CompareTo(minValue) < 0 ? minValue : maxCompareValue;
                maxCompareValue = maxCompareValue.CompareTo(maxValue) > 0 ? maxValue : maxCompareValue;

                InvokeOnMainThread(() => Max = maxCompareValue);

                SelectedValue.MaxValue = ToRawString(Max);
                RaisePropertyChanged(() => SelectedValue);
            });
        }

        protected virtual void ParseRawValues()
        {
            Min = MinValueRaw.IsNullOrEmtpy() ? default(T) : FromString(MinValueRaw);
            Max = MaxValueRaw.IsNullOrEmtpy() ? default(T) : FromString(MaxValueRaw);

            _selectedValue = new ApplyedProductParameter { Id = this.Id, DataType = this.DataType, MinValue = ToRawString(Min), MaxValue = ToRawString(Max) };
        }

        protected abstract T FromString(string rawValue);

        protected abstract string ToRawString(T value);

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() => ParseRawValues());
        }

        #region IFiltersItemVm implementation

        public override void ClearSelectedValue()
        {
            SelectedValue = null;

            Min = FromString(MinValueRaw);
            Max = FromString(MaxValueRaw);
        }

        #endregion

        #endregion
    }
}
