using System;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.Base.Core.Attributes;
namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Cataloged
{
    [Deeplink("products")]
    public class CCategoriesPageViewModel : CatalogViewModel, Base.Core.ViewModels.IMvxPageViewModel
    {
        #region Properties

        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        #endregion
    }
}
