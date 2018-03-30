using AppRopio.Base.Droid.Adapters;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Images;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.ShortInfo;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Droid.Views.ProductCard
{
    public class ProductCardTemplateSelector : IARFlatGroupTemplateSelector
    {
        public virtual int GetFooterViewType(object forItemObject)
        {
            throw new System.NotImplementedException();
        }

        public virtual int GetHeaderViewType(object forItemObject)
        {
            throw new System.NotImplementedException();
        }

        public virtual int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public virtual int GetItemViewType(object forItemObject)
        {
            var viewType = -1;

            if (forItemObject is IProductDetailsItemVM itemVm)
            {
                switch (itemVm.WidgetType)
                {
                    case ProductWidgetType.HorizontalCollection:
                        {
                            switch (itemVm.DataType)
                            {
                                case ProductDataType.Products:
                                    viewType = Resource.Layout.app_products_productCard_productsHorizontalCollection; // TODO
                                    break;
                                case ProductDataType.ShopsAvailability_Count:
                                    viewType = Resource.Layout.app_products_productCard_shopsHorizontalCollection_count;
                                    break;
                                case ProductDataType.ShopsAvailability_Indicator:
                                    viewType = Resource.Layout.app_products_productCard_shopsHorizontalCollection_indicator;
                                    break;
                                default:
                                    viewType = Resource.Layout.app_products_productCard_horizontalCollection;
                                    break;
                            }

                            break;
                        }
                    case ProductWidgetType.VerticalCollection:
                        viewType = Resource.Layout.app_products_productCard_verticalCollection;
                        break;
                    case ProductWidgetType.MinMax:
                        viewType = itemVm.DataType == ProductDataType.Date ?
                            Resource.Layout.app_products_productCard_dateMinMax :
                            Resource.Layout.app_products_productCard_numberMinMax;
                        break;
                    case ProductWidgetType.Picker:
                        viewType = Resource.Layout.app_products_productCard_picker;
                        break;
                    case ProductWidgetType.OneSelection:
                        viewType = Resource.Layout.app_products_productCard_oneSelection;
                        break;
                    case ProductWidgetType.MultiSelection:
                        viewType = Resource.Layout.app_products_productCard_multiSelection;
                        break;
                    case ProductWidgetType.Switch:
                        viewType = Resource.Layout.app_products_productCard_switch;
                        break;
                    case ProductWidgetType.MultilineText:
                        viewType = Resource.Layout.app_products_productCard_multilineText;
                        break;
                    case ProductWidgetType.Transition:
                        viewType = Resource.Layout.app_products_productCard_transition;
                        break;
                }
            }
            else if (forItemObject is IImagesProductsPciVm)
            {
                viewType = Resource.Layout.app_products_productCard_imagesCollection;
            }
            else if (forItemObject is IShortInfoProductsPciVm)
            {
                viewType = Resource.Layout.app_products_productCard_shortInfo;
            }

            return viewType;
        }
    }
}