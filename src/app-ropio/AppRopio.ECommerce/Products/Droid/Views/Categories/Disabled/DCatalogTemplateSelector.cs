using System;
using AppRopio.Base.Droid.Adapters;
using AppRopio.ECommerce.Products.Droid.Views.Catalog;

namespace AppRopio.ECommerce.Products.Droid.Views.Categories.Disabled
{
    public class DCatalogTemplateSelector : IARFlatGroupTemplateSelector
    {
        private CatalogCollectionType _collectionType;

        public DCatalogTemplateSelector(CatalogCollectionType collectionType)
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
            return _collectionType == CatalogCollectionType.Grid ?
                                                           Resource.Layout.app_products_catalog_item_grid
                                                               :
                                                           Resource.Layout.app_products_catalog_item_list;
        }
    }
}
