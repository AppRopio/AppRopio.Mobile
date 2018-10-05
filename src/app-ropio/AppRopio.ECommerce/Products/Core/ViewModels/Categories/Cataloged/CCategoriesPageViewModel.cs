using AppRopio.Base.Core.Attributes;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Cataloged
{
    [Deeplink("products")]
    public class CCategoriesPageViewModel : CatalogViewModel, Base.Core.ViewModels.IMvxPageViewModel
    {
        #region Properties

        public override bool SearchBar => ConfigService.Config.SearchType == SearchType.Bar;

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
