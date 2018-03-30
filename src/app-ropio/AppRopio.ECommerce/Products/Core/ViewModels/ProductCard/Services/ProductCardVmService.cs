using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Products;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Vertical;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Images;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax.Date;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax.Number;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Multiline;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.OneSelection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.ShortInfo;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Switch;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Transition;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Services
{
    public class ProductCardVmService : BaseVmService, IProductCardVmService
    {
        #region Fields

        private readonly Dictionary<string, ProductDetails> _cachedDetails = new Dictionary<string, ProductDetails>();

        private IMvxViewModel _basketBlockViewModel;

        #endregion

        #region Services

        protected API.Services.IProductService ProductService => Mvx.Resolve<API.Services.IProductService>();

        protected IProductConfigService ConfigService => Mvx.Resolve<IProductConfigService>();

        #endregion

        #region Private

        private IProductDetailsItemVM SetupParametersItem(string groupId, string productId, ProductParameter parameter)
        {
            IProductDetailsItemVM itemVm = null;

            switch (parameter.WidgetType)
            {
                case ProductWidgetType.HorizontalCollection:
                case ProductWidgetType.VerticalCollection:
                    itemVm = CreateCollection(groupId, productId, parameter);
                    break;
                case ProductWidgetType.MinMax:
                    itemVm = CreateMinMax(parameter);
                    break;
                case ProductWidgetType.Picker:
                    itemVm = CreatePicker(parameter);
                    break;
                case ProductWidgetType.OneSelection:
                case ProductWidgetType.MultiSelection:
                    itemVm = CreateSelection(parameter);
                    break;
                case ProductWidgetType.Switch:
                    itemVm = CreateSwitch(parameter);
                    break;
                case ProductWidgetType.MultilineText:
                    itemVm = CreateMultilineText(parameter);
                    break;
                case ProductWidgetType.Transition:
                    itemVm = CreateTransitionItem(groupId, productId, parameter);
                    break;
            }

            if (itemVm != null)
                itemVm.Initialize();

            return itemVm;
        }

        #endregion

        #region Protected

        #region Basic

        protected virtual IImagesProductsPciVm SetupImagesItem(List<Image> images)
        {
            IImagesProductsPciVm itemVm = null;

            if (!images.IsNullOrEmpty())
            {
                itemVm = new ImagesProductsPciVm(images);
                itemVm.Initialize();
            }

            return itemVm;
        }

        protected virtual IShortInfoProductsPciVm SetupShortInfoItem(Product model)
        {
            IShortInfoProductsPciVm itemVm = null;

            itemVm = new ShortInfoProductsPciVm(model);
            itemVm.Initialize();

            return itemVm;
        }

        #endregion

        #region Details

        #region Collection

        protected virtual IBaseCollectionPciVm CreateCollection(string groupId, string productId, ProductParameter parameter)
                => parameter.WidgetType == ProductWidgetType.HorizontalCollection ?
                         (IBaseCollectionPciVm)CreateHorizontalCollection(groupId, productId, parameter) :
                         (IBaseCollectionPciVm)CreateVerticalCollection(parameter);

        protected virtual IBaseCollectionPciVm CreateHorizontalCollection(string groupId, string productId, ProductParameter parameter)
        {
            switch (parameter.DataType)
            {
                case ProductDataType.Products:
                    return (IBaseCollectionPciVm)new HorizontalProductsCollectionPciVm(groupId, productId, parameter);
                case ProductDataType.ShopsAvailability_Count:
                case ProductDataType.ShopsAvailability_Indicator:
                    return (IBaseCollectionPciVm)new HorizontalShopsCollectionPciVm(groupId, productId, parameter);
                default:
                    return new HorizontalCollectionPciVm(parameter);
            }
        }
                    

        protected virtual IVerticalCollectionPciVm CreateVerticalCollection(ProductParameter parameter) => new VerticalCollectionPciVm(parameter);

        #endregion

        #region MinMax

        protected virtual IBaseMinMaxPciVm CreateMinMax(ProductParameter parameter)
        {
            return parameter.DataType == ProductDataType.Number ?
                            (IBaseMinMaxPciVm)CreateNumberMinMax(parameter) :
                            (IBaseMinMaxPciVm)CreateDateMinMax(parameter);
        }

        protected virtual INumberMinMaxPciVm CreateNumberMinMax(ProductParameter parameter)
        {
            return new NumberMinMaxPciVm(parameter);
        }

        protected virtual IDateMinMaxPciVm CreateDateMinMax(ProductParameter parameter)
        {
            return new DateMinMaxPciVm(parameter);
        }

        #endregion

        #region Picker

        protected virtual IPickerPciVm CreatePicker(ProductParameter parameter)
        {
            return new PickerPciVm(parameter);
        }

        #endregion

        #region Collection

        protected virtual IBaseSelectionPciVm CreateSelection(ProductParameter parameter)
        {
            return parameter.WidgetType == ProductWidgetType.OneSelection ?
                         (IBaseSelectionPciVm)CreateOneSelection(parameter) :
                         (IBaseSelectionPciVm)CreateMultiSelection(parameter);
        }

        protected virtual IOneSelectionPciVm CreateOneSelection(ProductParameter parameter)
        {
            return new OneSelectionPciVm(parameter);
        }

        protected virtual IMultiSelectionPciVm CreateMultiSelection(ProductParameter parameter)
        {
            return new MultiSelectionPciVm(parameter);
        }

        #endregion

        #region Switch

        protected virtual ISwitchPciVm CreateSwitch(ProductParameter parameter)
        {
            return new SwitchPciVm(parameter);
        }

        #endregion

        #region MultilineText

        protected virtual IMultilinePciVm CreateMultilineText(ProductParameter parameter)
        {
            return new MultilinePciVm(parameter);
        }

        #endregion

        #region TransitionItem

        protected virtual ITransitionPciVm CreateTransitionItem(string productGroupId, string productId, ProductParameter parameter)
        {
            return new TransitionPciVm(productGroupId, productId, parameter);
        }

        #endregion

        #endregion

        #endregion

        #region IProductCardVmService implementation

        public async Task<Product> LoadProductShortInfo(string groupId, string productId)
        {
            var product = new Product();

            try
            {
                product = await ProductService.LoadProductShortInfo(groupId, productId);
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return product;
        }

        public Task<ObservableCollection<IProductBasicItemVM>> LoadBasicProductCardItems(Product model)
        {
            return Task.Run(() =>
            {
                var dataSource = new ObservableCollection<IProductBasicItemVM>();

                try
                {
                    if (!model.ImageUrls.IsNullOrEmpty())
                        dataSource.Add(SetupImagesItem(model.ImageUrls));

                    dataSource.Add(SetupShortInfoItem(model));
                }
                catch (Exception ex)
                {
                    MvxTrace.TaggedTrace(MvxTraceLevel.Warning, this.GetType().FullName, ex.BuildAllMessagesAndStackTrace());
                }

                return dataSource;
            });
        }

        public async Task<ObservableCollection<IProductDetailsItemVM>> LoadDetailsProductCardItems(string groupId, string productId)
        {
            var dataSource = new ObservableCollection<IProductDetailsItemVM> ();

            try
            {
                ProductDetails details = null;

                if (!_cachedDetails.TryGetValue($"{groupId}{productId}", out details))
                {
                    details = await ProductService.LoadProductDetails(groupId, productId);

                    if (details != null)
                        _cachedDetails.Add($"{groupId}{productId}", details);
                }

                dataSource = new ObservableCollection<IProductDetailsItemVM>(details.Parameters.Select(x => SetupParametersItem(groupId, productId, x)).Where(y => y != null));
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return dataSource;
        }

        public async Task<Product> ReloadProductByParameters(string groupId, string productId, IEnumerable<ApplyedProductParameter> applyedParameters)
        {
            Product newProduct = null;

            try
            {
                newProduct = await ProductService.ReloadProductByParameter(groupId, productId, applyedParameters);
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return newProduct;
        }

        public IMvxViewModel LoadBasketBlock()
        {
            var config = ConfigService.Config;

            try
            {
                if (config.Basket?.AddToCart != null)
                {
                    var assembly = Assembly.Load(new AssemblyName(config.Basket.AddToCart.AssemblyName));

                    var basketType = assembly.GetType(config.Basket.AddToCart.TypeName);

                    object basketInstance = null;

                    if (basketType.GetTypeInfo().IsInterface)
                    {
                        var viewModelType = Mvx.Resolve<IViewModelLookupService>().Resolve(basketType);
                        basketInstance = Activator.CreateInstance(viewModelType);
                    }
                    else
                        basketInstance = Activator.CreateInstance(basketType);

                    return _basketBlockViewModel ?? (_basketBlockViewModel = basketInstance as IMvxViewModel);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return null;
        }

        #endregion
    }
}
