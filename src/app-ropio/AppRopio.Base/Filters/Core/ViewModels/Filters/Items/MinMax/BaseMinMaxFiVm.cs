using System;
using AppRopio.Models.Filters.Responses;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax
{
    public abstract class BaseMinMaxFiVm<T> : FiltersItemVm, IBaseMinMaxFiVm<T>
        where T : IComparable, IComparable<T>, IEquatable<T>
    {
        #region Command

        private ICommand _minValueChangedCommand;
        public virtual ICommand MinValueChangedCommand
        {
            get
            {
                return _minValueChangedCommand ?? (_minValueChangedCommand = new MvxCommand(OnMinValueChanged));
            }
        }

        private ICommand _maxValueChangedCommand;
        public virtual ICommand MaxValueChangedCommand
        {
            get
            {
                return _maxValueChangedCommand ?? (_maxValueChangedCommand = new MvxCommand(OnMaxValueChanged));
            }
        }

        #endregion

        #region Properties

        public override ApplyedFilter SelectedValue { get; protected set; }

        protected string MinValueRaw { get; set; }

        protected string MaxValueRaw { get; set; }

        protected string MinSelectedValueRaw { get; set; }

        protected string MaxSelectedValueRaw { get; set; }

        public T AbsoluteMin { get { return string.IsNullOrEmpty(MinValueRaw) ? default(T) : FromString(MinValueRaw); } }

        public T AbsoluteMax { get { return string.IsNullOrEmpty(MaxValueRaw) ? default(T) : FromString(MaxValueRaw); } }

        private T _min;
        public T Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
                RaisePropertyChanged(() => Min);
            }
        }

        private T _max;
        public T Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                RaisePropertyChanged(() => Max);
            }
        }

        #endregion

        #region Constructor

        protected BaseMinMaxFiVm(Filter filter, string minSelectedValue, string maxSelectedValue)
            : base(filter)
        {
            MaxValueRaw = filter.MaxValue;
            MinValueRaw = filter.MinValue;

            MaxSelectedValueRaw = maxSelectedValue;
            MinSelectedValueRaw = minSelectedValue;
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
            });
        }

        protected virtual void ParseRawValues()
        {
            Min = MinSelectedValueRaw.IsNullOrEmtpy() ? FromString(MinValueRaw) : FromString(MinSelectedValueRaw);
            Max = MaxSelectedValueRaw.IsNullOrEmtpy() ? FromString(MaxValueRaw) : FromString(MaxSelectedValueRaw);

            SelectedValue = new ApplyedFilter { Id = this.Id, DataType = this.DataType, MinValue = ToRawString(Min), MaxValue = ToRawString(Max) };
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

            MinSelectedValueRaw = null;
            MaxSelectedValueRaw = null;

            Min = FromString(MinValueRaw);
            Max = FromString(MaxValueRaw);
        }

        #endregion

        #endregion
    }
}
