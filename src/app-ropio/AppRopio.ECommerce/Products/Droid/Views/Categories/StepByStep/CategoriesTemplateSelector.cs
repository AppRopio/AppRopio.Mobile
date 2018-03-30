using AppRopio.Base.Droid.Adapters;

namespace AppRopio.ECommerce.Products.Droid.Views.Categories.StepByStep
{
    public class CategoriesTemplateSelector : IARFlatGroupTemplateSelector
    {
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
            return Resource.Layout.app_products_sscategories_item;
        }
    }
}