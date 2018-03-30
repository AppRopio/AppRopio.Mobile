using AppRopio.Base.Droid.Adapters;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;

namespace AppRopio.ECommerce.Basket.Droid.Views.Order.Full
{
    public class FullOrderTemplateSelector : IARFlatGroupTemplateSelector
    {
        public int GetFooterViewType(object forItemObject)
        {
            return forItemObject is IDeliveryTypeItemVM ? Resource.Layout.app_basket_full_order_deliveryTime_footer : Resource.Layout.app_basket_full_order_footer;
        }

        public int GetHeaderViewType(object forItemObject)
        {
            return Resource.Layout.app_basket_full_order_header;
        }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public int GetItemViewType(object forItemObject)
        {
            if (forItemObject is IDeliveryTypeItemVM)
                return Resource.Layout.app_basket_full_order_item_deliveryField;
            if (forItemObject is IOrderFieldItemVM orderField)
            {
                if (orderField.IsOptional)
                    return Resource.Layout.app_basket_full_order_item_orderField_optional;
                
                switch (orderField.Type)
                {
                    case Models.Basket.Responses.Enums.OrderFieldType.Counter:
                        return Resource.Layout.app_basket_full_order_item_orderField_counter;
                    case Models.Basket.Responses.Enums.OrderFieldType.Phone:
                        return Resource.Layout.app_basket_full_order_item_orderField_phone;
                    default:
                        return Resource.Layout.app_basket_full_order_item_orderField_text;
                }
            }

            return -1;
        }
    }
}