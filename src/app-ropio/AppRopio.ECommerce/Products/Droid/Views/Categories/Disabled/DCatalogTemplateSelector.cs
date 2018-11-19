using AppRopio.Base.Droid.Adapters;
using AppRopio.ECommerce.Products.Core.Models;

namespace AppRopio.ECommerce.Products.Droid.Views.Categories.Disabled
{
    public class DCatalogTemplateSelector : IARFlatGroupTemplateSelector
    {
        private CollectionType _collectionType;

        public DCatalogTemplateSelector(CollectionType collectionType)
        {
            _collectionType = collectionType;
        }

        public int GetFooterViewType(object forItemObject)
        {
            return Resource.Layout.app_products_sscategories_footer;
        }

        public int GetHeaderViewType(object forItemObject)
        {
            return Resource.Layout.app_products_sscategories_header;
        }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public int GetItemViewType(object forItemObject)
        {
            return _collectionType == CollectionType.Grid ?
                                                           Resource.Layout.app_products_catalog_item_grid
                                                               :
                                                           Resource.Layout.app_products_catalog_item_list;
        }
    }
}
