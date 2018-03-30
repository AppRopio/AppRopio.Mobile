using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Vertical;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Images;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Multiline;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.OneSelection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.ShortInfo;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Products;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Vertical;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Images;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MinMax;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MinMax.Date;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MinMax.Number;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MultilineText;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Picker;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.MultiSelection;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.OneSelection;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.ShortInfo;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Switch;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Transition;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.ViewSources;
using AppRopio.Models.Products.Responses;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard
{
    public partial class ProductCardViewController : ProductCardViewController<IProductCardViewModel>
    {
        protected override UITableView TableView => _tableView;

        public ProductCardViewController()
        {

        }

        protected ProductCardViewController(IntPtr handle)
            : base(handle)
        {

        }

        protected ProductCardViewController(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();

            base.CleanUp();
        }
    }

    public abstract class ProductCardViewController<T> : CommonViewController<T>
        where T : class, IProductCardViewModel
    {
        private MvxSubscriptionToken _subscriptionToken;

        private UIBarButtonItem _markBarButton;
        private UIBarButtonItem _shareBarButton;

        private UIButton _markButton;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        protected IMvxMessenger MessengerService { get { return Mvx.Resolve<IMvxMessenger>(); } }

        protected abstract UITableView TableView { get; }

        public ProductCardViewController()
            : base("ProductCardViewController", null)
        {
            RegisterKeyboardActions = true;
        }

        protected ProductCardViewController(IntPtr handle)
            : base(handle)
        {

        }

        protected ProductCardViewController(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        #region Private

        private void RegisterCells(UITableView tableView)
        {
            var cellsMap = CreateCellsMap();

            foreach (var item in cellsMap)
                tableView.RegisterNibForCellReuse(item.Value, item.Key);
        }

        #endregion

        #region Protected

        protected virtual void OnProductDetailsReloadMessageRecieved(ProductDetailsReloadWhenValueChangedMessage msg)
        {
            (TableView.Source as ProductCardTableViewSource)?.ClearCache();

            TableView.BeginUpdates();
            TableView.ReloadData();
            TableView.EndUpdates();

            var selectedIndexPath = (TableView.Source as ProductCardTableViewSource).IndexPathForSelectedRow;
            if (selectedIndexPath != null)
                NSOperationQueue.MainQueue.AddOperation(() => TableView.ScrollToRow(selectedIndexPath, UITableViewScrollPosition.Middle, true));
        }

        #region InitializationControls

        protected void SetupBasketView(UITableView tableView)
        {
            var config = Mvx.Resolve<IProductConfigService>().Config;
            if (config.Basket?.AddToCart != null && Mvx.Resolve<IViewLookupService>().IsRegistered(config.Basket?.AddToCart.TypeName))
            {
                var basketView = ViewModel.BasketBlockViewModel == null ? null : Mvx.Resolve<IMvxIosViewCreator>().CreateView(ViewModel.BasketBlockViewModel) as UIView;
                if (basketView != null)
                {
                    if (tableView != null)
                        tableView.ContentInset = new UIEdgeInsets(0, 0, basketView.Frame.Height, 0);
                    
                    basketView.ChangeFrame(y: DeviceInfo.ScreenHeight - basketView.Frame.Height);

                    View.AddSubview(basketView);
                }
            }
        }

        protected virtual UIBarButtonItem SetupShareButton()
        {
            var barButton = new UIBarButtonItem(UIImage.FromFile(ThemeConfig.ProductDetails.ShareButton.Image.Path), UIBarButtonItemStyle.Plain, null);

            return barButton;
        }

        protected virtual UIBarButtonItem SetupMarkButton(UIButton button)
        {
            button.SetupStyle(ThemeConfig.ProductDetails.MarkButton);

            return new UIBarButtonItem(button);
        }

        protected virtual void SetupTableView(UITableView tableView)
        {
            RegisterCells(tableView);
        }

        protected virtual Dictionary<NSString, UINib> CreateCellsMap()
        {
            return new Dictionary<NSString, UINib>
            {
                [ImagesCell.Key] = ImagesCell.Nib,
                [ShortInfoCell.Key] = ShortInfoCell.Nib,

                [PDHorizontalShopsCollectionCell.Key] = PDHorizontalShopsCollectionCell.Nib,
                [PDHorizontalProductsCollectionCell.Key] = PDHorizontalProductsCollectionCell.Nib,
                [PDHorizontalCollectionCell.Key] = PDHorizontalCollectionCell.Nib,
                [PDVerticalCollectionCell.Key] = PDVerticalCollectionCell.Nib,
                [PDDateMinMaxCell.Key] = PDDateMinMaxCell.Nib,
                [PDNumberMinMaxCell.Key] = PDNumberMinMaxCell.Nib,
                [PDPickerCell.Key] = PDPickerCell.Nib,
                [PDMultiSelectionCell.Key] = PDMultiSelectionCell.Nib,
                [PDOneSelectionCell.Key] = PDOneSelectionCell.Nib,
                [PDSwitchCell.Key] = PDSwitchCell.Nib,
                [MultilineTextCell.Key] = MultilineTextCell.Nib,
                [TransitionCell.Key] = TransitionCell.Nib
            };
        }

        #endregion

        #region BindingControls

        protected virtual void BindMarkButton(UIButton markButton, MvxFluentBindingDescriptionSet<ProductCardViewController<T>, IProductCardViewModel> set)
        {
            set.Bind(markButton).To(vm => vm.MarkCommand);
            set.Bind(markButton)
               .For(mb => mb.Selected)
               .To(vm => vm.Marked);
        }

        protected virtual void BindShareButton(UIBarButtonItem shareButton, MvxFluentBindingDescriptionSet<ProductCardViewController<T>, IProductCardViewModel> set)
        {
            set.Bind(shareButton).To(vm => vm.ShareCommand);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<ProductCardViewController<T>, IProductCardViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            var dataSource = new ProductCardTableViewSource(tableView)
            {
                WidgetHeight = CalculateWidgetHeight
            };
            return dataSource;
        }

        protected virtual float CalculateWidgetHeight(object item, ProductWidgetType? widgetType, ProductDataType? dataType)
        {
            float itemHeight = 0;

            if (widgetType == null)
            {
                if (item is IImagesProductsPciVm imagesItem && !imagesItem.ImagesUrls.IsNullOrEmpty())
                    itemHeight = DeviceInfo.ScreenWidth;

                if (item is IShortInfoProductsPciVm)
                    itemHeight = ShortInfoCell.INFO_HEIGHT;
            }
            else
            {
                switch (widgetType)
                {
                    case ProductWidgetType.HorizontalCollection:
                        {
                            if (dataType == ProductDataType.Products)
                            {
                                if (ThemeConfig.Products.CollectionType == Models.CollectionType.Grid)
                                {
                                    var collectionItemWidth = ThemeConfig.Products.ProductCell.Size.Height ?? (DeviceInfo.ScreenWidth - PDHorizontalProductsCollectionCell.DEFAULT_INSET - (PDHorizontalProductsCollectionCell.DEFAULT_INSET / 2)) / 2;

                                    itemHeight = PDHorizontalProductsCollectionCell.HEIGHT + PDHorizontalProductsCollectionCell.DEFAULT_INSET + (int)(collectionItemWidth * 1.69f);
                                }
                                else
                                {
                                    var height = ThemeConfig.Products.ProductCell.Size.Height ?? 146;

                                    itemHeight = PDHorizontalProductsCollectionCell.HEIGHT + PDHorizontalProductsCollectionCell.DEFAULT_INSET + height;
                                }
                            }
                            else if (dataType == ProductDataType.ShopsAvailability_Count)
                                itemHeight = PDHorizontalShopsCollectionCell.HEIGHT + PDHorizontalShopsCollectionCell.COUNT_HEIGHT + PDHorizontalShopsCollectionCell.DEFAULT_INSET;
                            else if (dataType == ProductDataType.ShopsAvailability_Indicator)
                                itemHeight = PDHorizontalShopsCollectionCell.HEIGHT + PDHorizontalShopsCollectionCell.INDICATOR_HEIGHT + PDHorizontalShopsCollectionCell.DEFAULT_INSET;
                            else
                                itemHeight = PDHorizontalCollectionCell.HORIZONTAL_COLLECTION_HEIGHT;

                            break;
                        }
                    case ProductWidgetType.VerticalCollection:
                        {
                            if (item is IVerticalCollectionPciVm verticalItemVm)
                            {
                                itemHeight = verticalItemVm.Values.IsNullOrEmpty() ?
                                                            0 :
                                                            ((verticalItemVm.Values.Count / 4) + (verticalItemVm.Values.Count % 4 == 0 ? 0 : 1)) * PDVerticalCollectionCell.VERTICAL_COLLECTION_ITEM_HEIGHT;
                                itemHeight += PDVerticalCollectionCell.VERTICAL_COLLECTION_BOTTOM_INSET;
                            }

                            itemHeight = PDVerticalCollectionCell.VERTICAL_COLLECTION_TITLE_HEIGHT + itemHeight;

                            break;
                        }
                    case ProductWidgetType.MinMax:
                        {
                            itemHeight = BaseMinMaxCell.MIN_MAX_HEIGHT;

                            break;
                        }
                    case ProductWidgetType.Picker:
                        {
                            itemHeight = PDPickerCell.PICKER_TITLE_HEIGHT;

                            if (item is IPickerPciVm pickerItemVm && pickerItemVm.Selected)
                                itemHeight += PDPickerCell.PICKER_CONTENT_HEIGHT;

                            break;
                        }
                    case ProductWidgetType.OneSelection:
                        {
                            itemHeight = PDOneSelectionCell.ONE_SELECTION_HEIGHT;

                            if (item is IOneSelectionPciVm oneItemVm && !oneItemVm.ValueName.IsNullOrEmtpy())
                                itemHeight += PDOneSelectionCell.ONE_SELECTION_CONTENT_HEIGHT;

                            break;
                        }
                    case ProductWidgetType.MultiSelection:
                        {
                            itemHeight = PDMultiSelectionCell.MULTY_SELECTION_TITLE_HEIGHT;

                            if (item is IMultiSelectionPciVm multiItemVm && !multiItemVm.Items.IsNullOrEmpty())
                                itemHeight += PDMultiSelectionCell.MULTY_SELECTION_CONTENT_HEIGHT;

                            break;
                        }
                    case ProductWidgetType.Switch:
                        {
                            itemHeight = PDSwitchCell.SWITCH_HEIGHT;

                            break;
                        }
                    case ProductWidgetType.MultilineText:
                        {
                            if (item is IMultilinePciVm multilineItemVm)
                                itemHeight = MultilineTextCell.GetHeightForContent(multilineItemVm.Text);

                            break;
                        }
                    case ProductWidgetType.Transition:
                        {
                            itemHeight = TransitionCell.TRANSITION_HEIGHT;

                            break;
                        }
                }
            }

            return itemHeight;
        }

        #endregion

        #region CommonViewController implementation

        protected override void SetupLoading()
        {
            base.SetupLoading();

            LoadingView.ChangeFrame(y: -64, h: LoadingView.Frame.Height + 64);
        }

        protected override void InitializeControls()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;

            _shareBarButton = SetupShareButton();
            _markBarButton = SetupMarkButton(_markButton = new UIButton(UIButtonType.Custom).WithFrame(0, 0, 44, 44));

            SetupBasketView(TableView);

            SetupTableView(TableView);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<ProductCardViewController<T>, IProductCardViewModel>();

            if (_shareBarButton != null)
                BindShareButton(_shareBarButton, set);

            if (_markBarButton != null)
                BindMarkButton(_markButton, set);

            BindTableView(TableView, set);

            set.Apply();

            if (ViewModel.MarkEnabled && _shareBarButton != null && _markBarButton != null)
                NavigationItem.SetRightBarButtonItems(new[] { _shareBarButton, _markBarButton }, false);
            else if (_shareBarButton != null)
                NavigationItem.SetRightBarButtonItems(new[] { _shareBarButton }, false);

            _subscriptionToken = MessengerService.Subscribe<ProductDetailsReloadWhenValueChangedMessage>(OnProductDetailsReloadMessageRecieved);
        }

        protected override void CleanUp()
        {
            if (_subscriptionToken != null)
            {
                MessengerService.Unsubscribe<ProductDetailsReloadWhenValueChangedMessage>(_subscriptionToken);
                _subscriptionToken?.Dispose();
                _subscriptionToken = null;
            }

            if (_markButton != null)
            {
                _markButton.Dispose();
                _markButton = null;
            }
        }

        #endregion

        #endregion

        #region Public

        public override void ViewWillAppear(bool animated)
        {
            if (NavigationController != null)
            {
                NavigationController.SetTranparentNavBar(true);
                //if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || !Theme.ControlPalette.NavigationBar.PrefersLargeTitles)
                //{
                //    NavigationController.NavigationBar.BackgroundColor = null;
                //    NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
                //    NavigationController.NavigationBar.ShadowImage = new UIImage();
                //    NavigationController.NavigationBar.Translucent = true;
                //}
                //else if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                //{
                //    NavigationController.NavigationBar.ShadowImage = new UIImage();
                //    NavigationController.NavigationBar.BackgroundColor = null;
                //    NavigationController.NavigationBar.Translucent = true;
                //    NavigationController.NavigationBar.Opaque = true;
                //}
            }

            base.ViewWillAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            if (NavigationController != null)
                NavigationController.NavigationBar.SetupStyle(Theme.ControlPalette.NavigationBar);
            else if (Mvx.Resolve<IMvxIosViewPresenter>() is MvxIosViewPresenter presenter)
                presenter.MasterNavigationController?.SetTranparentNavBar(false);

            base.ViewDidDisappear(animated);
        }

        #endregion
    }
}

