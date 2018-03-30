using System;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Items;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace AppRopio.ECommerce.Products.Droid.Views.ProductCard
{
    public class ProductCardHorizontalCollectionTemplateSelector : IMvxTemplateSelector
    {
        public int GetItemViewType(object forItemObject)
        {
            var itemVm = forItemObject as CollectionItemVM;

            switch (itemVm.DataType)
            {
                case AppRopio.Models.Products.Responses.ProductDataType.Color:
                    return Resource.Layout.app_products_productCard_horizontalCollection_color;
                default:
                    return Resource.Layout.app_products_productCard_horizontalCollection_text;
            }
        }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }
    }
}
