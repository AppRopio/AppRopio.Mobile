using System;
using System.Globalization;
using System.Windows.Input;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Commands;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax.Date
{
    public class DateMinMaxFiVm : BaseMinMaxFiVm<DateTime>, IDateMinMaxFiVm
    {
        #region Commands

        private ICommand _minValueChangedCommand;
        public override ICommand MinValueChangedCommand
        {
            get
            {
                return _minValueChangedCommand ?? (_minValueChangedCommand = new MvxCommand<DateTime?>(OnMinValueChanged));
            }
        }

        private ICommand _maxValueChangedCommand;
        public override ICommand MaxValueChangedCommand
        {
            get
            {
                return _maxValueChangedCommand ?? (_maxValueChangedCommand = new MvxCommand<DateTime?>(OnMaxValueChanged));
            }
        }

        #endregion

        #region Constructor

        public DateMinMaxFiVm(Filter filter, string minSelectedValue, string maxSelectedValue)
            : base(filter, minSelectedValue, maxSelectedValue)
        {
        }

        #endregion

        #region Protected

        protected virtual void OnMinValueChanged(DateTime? date)
        {
            if (date.HasValue)
                Min = date.Value;

            base.OnMinValueChanged();
        }

        protected virtual void OnMaxValueChanged(DateTime? date)
        {
            if (date.HasValue)
                Max = date.Value;

            base.OnMaxValueChanged();
        }

        #region BaseMinMaxFiVm implementation

        protected override DateTime FromString(string rawValue)
        {
            DateTime result = default(DateTime);

            DateTime.TryParse(rawValue, out result);

            return result;
        }

        protected override string ToRawString(DateTime value)
        {
            return value.ToString("s", CultureInfo.InvariantCulture);
        }

        #endregion

        #endregion
    }
}
