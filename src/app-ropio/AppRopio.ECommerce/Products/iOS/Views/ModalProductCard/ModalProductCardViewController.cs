using System;
using System.Linq;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.ModalProductCard;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.ShortInfo;
using AppRopio.ECommerce.Products.iOS.Views.ModalProductCard.Cells.Collection.Horizontal.Products;
using AppRopio.ECommerce.Products.iOS.Views.ModalProductCard.Cells.Images;
using AppRopio.ECommerce.Products.iOS.Views.ModalProductCard.Cells.ShortInfo;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Products;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Images;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.ShortInfo;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ModalProductCard
{
    public partial class ModalProductCardViewController : ProductCardViewController<IModalProductCardViewModel>
    {
        private nfloat _previousScrollViewYOffset = -1;

        protected override UITableView TableView => _tableView;
        protected virtual UIButton CloseButton => _closeButton;

        public bool ClosingOnScrollDown { get; private set; }

        public ModalProductCardViewController()
            : base("ModalProductCardViewController", null)
        {
            DefinesPresentationContext = true;
            ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            TransitioningDelegate = new ViCoTransitioningDelegate();
        }

        #region Protected

        #region InitializeControls

        protected virtual void SetupCloseButton(UIButton closeButton)
        {
            _closeButtonTopConstraint.Constant = 20.If_iPhoneX(44);

            closeButton.SetupStyle(ThemeConfig.ProductDetails.CloseButton);
            closeButton.BackgroundColor = null;
            closeButton.Layer.ShadowOpacity = 0;
        }

        protected override void SetupTableView(UITableView tableView)
        {
            base.SetupTableView(tableView);

            tableView.ContentInset = new UIEdgeInsets(64, 0, tableView.ContentInset.Bottom, 0);
            tableView.BackgroundColor = UIColor.Clear;
        }

        protected override UIBarButtonItem SetupShareButton()
        {
            return null;
        }

        protected override UIBarButtonItem SetupMarkButton(UIButton button)
        {
            return null;
        }

        protected override void InitializeControls()
        {
            base.InitializeControls();

            SetupCloseButton(_closeButton);

            View.BackgroundColor = View.BackgroundColor.ColorWithAlpha(0.7f);
        }

        #endregion

        #region BindControls

        protected override System.Collections.Generic.Dictionary<Foundation.NSString, UINib> CreateCellsMap()
        {
            var cellMap = base.CreateCellsMap();

            cellMap[ShortInfoCell.Key] = ModalShortInfoCell.Nib;
            cellMap[ImagesCell.Key] = ModalImagesCell.Nib;
            cellMap[PDHorizontalProductsCollectionCell.Key] = ModalHorizontalProductsCollectionCell.Nib;

            return cellMap;
        }

        protected virtual void DataSource_OnScrolled(object sender, EventArgs e)
        {
            var scrollView = sender as UIScrollView;

            var scrollOffset = scrollView.ContentOffset.Y;

            if (_previousScrollViewYOffset == -1)
                _previousScrollViewYOffset = scrollOffset;

            var scrollDiff = scrollOffset - _previousScrollViewYOffset;
            var scrollHeight = scrollView.Frame.Height;
            var scrollContentSizeHeight = scrollView.ContentSize.Height + scrollView.ContentInset.Bottom;

            if (scrollOffset <= -(scrollView.ContentInset.Top)) // bounces top
            {
                if (scrollOffset < -(scrollView.ContentInset.Top + CloseButton.Frame.Height * 2))
                {
                    ClosingOnScrollDown = true;
                    CloseButton.SendActionForControlEvents(UIControlEvent.TouchUpInside);
                }

                var newConstant = 20.If_iPhoneX(44) - (scrollView.ContentInset.Top + scrollOffset);
                _closeButtonTopConstraint.Constant = newConstant;
            }
            else if ((scrollOffset + scrollHeight) >= scrollContentSizeHeight) // bounces bottom
            {
                //nothing
            }
            else
            {
                if (scrollOffset > -scrollView.ContentInset.Top && scrollOffset < 0)
                {
                    //show background and shadow
                    var background = ThemeConfig.ProductDetails.CloseButton.Background;
                    var layer = (Layer)ThemeConfig.ProductDetails.CloseButton.Layer.Clone();

                    var distance = scrollView.ContentInset.Top;

                    if (background != null)
                    {
                        var alpha = (float)Math.Min((distance + scrollOffset) / distance, 1.0f);
                        CloseButton.BackgroundColor = background.ToUIColor().ColorWithAlpha(alpha);
                    }

                    if (layer != null)
                    {
                        if (layer.Shadow != null)
                            layer.Shadow.Opacity = (float)Math.Min((distance + scrollOffset) / distance, layer.Shadow.Opacity);

                        CloseButton.Layer.SetupStyle(layer);
                    }
                }
                else if (scrollOffset < 0)
                {
                    //hide background and shadow
                    CloseButton.BackgroundColor = null;
                    CloseButton.Layer.ShadowOpacity = 0;
                }
                else
                {
                    CloseButton.BackgroundColor = ThemeConfig.ProductDetails.CloseButton.Background.ToUIColor();
                    CloseButton.Layer.SetupStyle(ThemeConfig.ProductDetails.CloseButton.Layer);
                }

                var minValue = 20.If_iPhoneX(44);
                _closeButtonTopConstraint.Constant = minValue;
            }

            _previousScrollViewYOffset = scrollOffset;
        }

        protected override MvvmCross.Platforms.Ios.Binding.Views.MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            var dataSource = base.SetupTableViewDataSource(tableView) as ScrolledEventsTableViewSource;
            dataSource.OnScrolled += DataSource_OnScrolled;
            return dataSource;
        }

        protected override float CalculateWidgetHeight(object item, AppRopio.Models.Products.Responses.ProductWidgetType? widgetType, AppRopio.Models.Products.Responses.ProductDataType? dataType)
        {
            if (widgetType == null && item is IShortInfoProductsPciVm shortItem)
                return shortItem.Badges.IsNullOrEmpty() ? ModalShortInfoCell.HEIGHT - ModalShortInfoCell.BADGES_HEIGHT : ModalShortInfoCell.HEIGHT;

            if (widgetType != null && widgetType == AppRopio.Models.Products.Responses.ProductWidgetType.HorizontalCollection && dataType == AppRopio.Models.Products.Responses.ProductDataType.Products)
            {
                var itemHeight = base.CalculateWidgetHeight(item, widgetType, dataType);
                itemHeight += PDHorizontalProductsCollectionCell.HEIGHT;

                return itemHeight;
            }

            return base.CalculateWidgetHeight(item, widgetType, dataType);
        }

        protected virtual void BindCloseButton(UIButton closeButton, MvxFluentBindingDescriptionSet<ModalProductCardViewController, IModalProductCardViewModel> set)
        {
            set.Bind(closeButton).To(vm => vm.CloseCommand);
        }

        protected override void BindControls()
        {
            base.BindControls();

            var set = this.CreateBindingSet<ModalProductCardViewController, IModalProductCardViewModel>();

            BindCloseButton(_closeButton, set);

            set.Apply();
        }

        #endregion

        protected override void CleanUp()
        {
            base.CleanUp();
        }

        #endregion

        #region Public

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController?.SetNavigationBarHidden(true, true);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            NavigationController?.SetNavigationBarHidden(true, false);
        }

        public override void ViewWillDisappear(bool animated)
        {
            NavigationController?.SetNavigationBarHidden(false, true);

            base.ViewWillDisappear(animated);
        }

        #endregion

        #region Transition

        private class ViCoTransitioningDelegate : UIViewControllerTransitioningDelegate
        {
            private const double ANIMATION_DURATION = 0.3;

            private const int VISUAL_EFFECT_TAG = 123456;

            public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
            {
                return new ShowTransitioning();
            }

            public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
            {
                return new HideTransitioning();
            }

            public class ShowTransitioning : UIViewControllerAnimatedTransitioning
            {
                public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
                {
                    var container = transitionContext.ContainerView;
                    var @to = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);

                    @to.View.Frame = UIScreen.MainScreen.Bounds;
                    @to.View.Alpha = 0;

                    var backgroundColor = Theme.ColorPalette.Background.ToUIColor();

                    var brightness = ((backgroundColor.CGColor.Components[0] * 299) + (backgroundColor.CGColor.Components[1] * 587) + (backgroundColor.CGColor.Components[2] * 114)) / 1000;

                    var visualEffectView = new UIVisualEffectView(UIBlurEffect.FromStyle(brightness > 0.5f ? UIBlurEffectStyle.ExtraLight : UIBlurEffectStyle.ExtraDark))
                    {
                        Frame = UIScreen.MainScreen.Bounds,
                        Tag = VISUAL_EFFECT_TAG,
                        Alpha = 0
                    };

                    container.AddSubviews(
                        visualEffectView,
                        @to.View
                    );

                    UIView.AnimateNotify(ANIMATION_DURATION, 0.0, UIViewAnimationOptions.CurveEaseIn, () =>
                    {
                        visualEffectView.Alpha = 1f;
                        @to.View.Alpha = 1f;
                    }, finished =>
                    {
                        transitionContext.CompleteTransition(true);
                    });
                }

                public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
                {
                    return ANIMATION_DURATION;
                }
            }

            public class HideTransitioning : UIViewControllerAnimatedTransitioning
            {
                public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
                {
                    var container = transitionContext.ContainerView;

                    var @from = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);

                    var visualEffectView = container.Subviews.FirstOrDefault(x => x.Tag == VISUAL_EFFECT_TAG);

                    UIView.Animate(ANIMATION_DURATION, 0.0, UIViewAnimationOptions.CurveEaseOut, () =>
                    {
                        if ((@from is ModalProductCardViewController viewController && viewController.ClosingOnScrollDown) ||
                            (@from is UINavigationController navigationController && navigationController.TopViewController is ModalProductCardViewController topViewController && topViewController.ClosingOnScrollDown))
                            @from.View.ChangeFrame(y: UIScreen.MainScreen.Bounds.Height);

                        @from.View.Alpha = 0;
                        visualEffectView.Alpha = 0;
                    }, () =>
                    {
                        visualEffectView.RemoveFromSuperview();
                        transitionContext.CompleteTransition(true);
                    });
                }

                public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
                {
                    return ANIMATION_DURATION;
                }
            }
        }

        #endregion
    }
}

