using System;
using System.Globalization;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax.Date
{
    public class DateMinMaxPciVm : BaseMinMaxPciVm<DateTime>, IDateMinMaxPciVm
    {
        #region Constructor

        public DateMinMaxPciVm(ProductParameter parameter)
            : base(parameter)
        {
        }

        #endregion

        #region Protected

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
