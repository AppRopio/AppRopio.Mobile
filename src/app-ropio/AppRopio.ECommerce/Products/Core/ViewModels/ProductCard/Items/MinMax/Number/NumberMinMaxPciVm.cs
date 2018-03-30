using System;
using System.Globalization;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax.Number
{
    public class NumberMinMaxPciVm : BaseMinMaxPciVm<float>, INumberMinMaxPciVm
    {
        #region Constructor

        public NumberMinMaxPciVm(ProductParameter parameter)
            : base(parameter)
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
