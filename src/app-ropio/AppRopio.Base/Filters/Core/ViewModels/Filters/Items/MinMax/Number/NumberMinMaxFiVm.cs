using System;
using AppRopio.Models.Filters.Responses;
using System.Globalization;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax.Number
{
    public class NumberMinMaxFiVm : BaseMinMaxFiVm<float>, INumberMinMaxFiVm
    {
        #region Constructor

        public NumberMinMaxFiVm(Filter filter, string minSelectedValue, string maxSelectedValue)
            : base(filter, minSelectedValue, maxSelectedValue)
        {
        }

        #endregion

        #region Protected

        #region BaseMinMaxFiVm implementation

        protected override float FromString(string rawValue)
        {
            double result = 0f;

            if (double.TryParse(rawValue, NumberStyles.Any, CultureInfo.CurrentCulture, out result) ||
                double.TryParse(rawValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out result) ||
                double.TryParse(rawValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return (float)result;

            return 0.0f;
        }

        protected override string ToRawString(float value)
        {
            return value.ToString();
        }

        #endregion

        #endregion
    }
}
