using AppRopio.Base.Droid.Adapters;

namespace AppRopio.ECommerce.Products.Droid.Views.Catalog
{
    public class CatalogTemplateSelector : IARFlatGroupTemplateSelector
    {
        private CatalogCollectionType _collectionType;

        public CatalogTemplateSelector(CatalogCollectionType collectionType)
        {
            _collectionType = collectionType;
        }

        public int GetFooterViewType(object forItemObject)
        {
            return -1;
        }

        public int GetHeaderViewType(object forItemObject)
        {
            return Resource.Layout.app_products_catalog_header;
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