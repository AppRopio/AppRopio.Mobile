using System;
using System.Linq;
using AppRopio.Base.Filters.Core.ViewModels.Sort;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using CoreAnimation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.Core.Services.Location;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Filters.Core;

namespace AppRopio.Base.Filters.iOS.Views.Sort
{
    public partial class SortViewController : CommonViewController<ISortViewModel>
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        protected NSLayoutConstraint ContainerViewTopConstraint { get { return _containerViewTopConstraint; } }
        protected NSLayoutConstraint ContainerViewHeightConstraint { get { return _containerViewHeightConstraint; } }

        private UIView _backgroundView;
        protected UIView BackgroundView
        {
            get { return _backgroundView; }
            set
            {
                if (value == null)
                    return;

                _backgroundView = value;

                View.InsertSubview(_backgroundView, 0);
            }
        }

        public SortViewController()
            : base("SortViewController", null)
        {
            ModalPresentationStyle = UIModalPresentationStyle.Custom;
            TransitioningDelegate = new SortTransitioningDelegate();
        }

        #region Private

        private void RecalculateContainerViewConstraints()
        {
            ContainerViewHeightConstraint.Constant = 50;

            if (ViewModel.Items != null && ViewModel.Items.Any())
                ContainerViewHeightConstraint.Constant += (ViewModel.Items.Count() * ThemeConfig.SortTypes.SortCell.Size.Height.Value) + 0.If_iPhoneX(30);

            ContainerViewTopConstraint.Constant = DeviceInfo.ScreenHeight - ContainerViewHeightConstraint.Constant;

            if (ContainerViewTopConstraint.Constant < 20)
            {
                ContainerViewTopConstraint.Constant = 20;
                ContainerViewHeightConstraint.Constant = DeviceInfo.ScreenHeight - 20;
                _tableView.ScrollEnabled = true;
            }

            UIView.Animate(0.3, () =>
            {
                View.LayoutIfNeeded();
            });
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == (nameof(ViewModel.Items)))
                RecalculateContainerViewConstraints();
        }

        #region SortTransitioningDelegate

        private class SortTransitioningDelegate : UIViewControllerTransitioningDelegate
        {
            private const double ANIMATION_DURATION = 0.2;

            public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
            {
                if (presented is SortViewController)
                    return new ShowSortTransitioning();
                return null;
            }

            public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
            {
                if (dismissed is SortViewController)
                {
                    var result = new HideSortTransitioning();
                    return result;
                }
                return null;
            }

            public class ShowSortTransitioning : UIViewControllerAnimatedTransitioning
            {
                public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
                {
                    var container = transitionContext.ContainerView;
                    var @to = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey) as SortViewController;

                    //@to.BackgroundView = new UIView(UIScreen.MainScreen.Bounds) { UserInteractionEnabled = true, BackgroundColor = UIColor.Black, Alpha = 0 };

                    @to.View.Frame = UIScreen.MainScreen.Bounds;

                    container.AddSubview(@to.View);

                    UIView.Animate(ANIMATION_DURATION, 0.0, UIViewAnimationOptions.CurveEaseInOut, () =>
                    {
                        @to.BackgroundView.Alpha = 0.6f;
                    }, () =>
                    {
                        transitionContext.CompleteTransition(true);
                    });
                }

                public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
                {
                    return ANIMATION_DURATION;
                }
            }

            public class HideSortTransitioning : UIViewControllerAnimatedTransitioning
            {
                public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
                {
                    var container = transitionContext.ContainerView;
                    var @from = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey) as SortViewController;

                    UIView.Animate(ANIMATION_DURATION, 0.0, UIViewAnimationOptions.CurveLinear, () =>
                    {
                        @from._containerView.ChangeFrame(y: UIScreen.MainScreen.Bounds.Height);
                        @from.BackgroundView.Alpha = 0;
                    }, () =>
                    {
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

        #endregion

        #region Protected

        #region InitializationControls

        protected virtual void SetupTitle(UILabel title)
        {
            title.Text = LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Sort_Title");
            title.SetupStyle(ThemeConfig.SortTypes.Title);
        }

        protected virtual void SetupContainerView(UIView containerView)
        {
            ContainerViewTopConstraint.Constant = UIScreen.MainScreen.Bounds.Height;

            containerView.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();

            var mask = UIBezierPath.FromRoundedRect(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height), UIRectCorner.TopLeft | UIRectCorner.TopRight, new CGSize(10, 10));
            var maskLayer = new CAShapeLayer { Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height), Path = mask.CGPath, StrokeColor = containerView.BackgroundColor.CGColor };

            containerView.Layer.Mask = maskLayer;
        }

        protected virtual void SetupTableView(UITableView tableView)
        {
            tableView.ScrollEnabled = false;
            tableView.SeparatorColor = Theme.ColorPalette.Separator.ToUIColor();
            tableView.RegisterNibForCellReuse(Cells.SortCell.Nib, Cells.SortCell.Key);
            tableView.RowHeight = ThemeConfig.SortTypes.SortCell.Size?.Height.Value ?? 60;
        }

        protected virtual void SetupCancelButton(UIButton cancelBtn)
        {
            cancelBtn.SetupStyle(ThemeConfig.SortTypes.CancelButton);
        }

        #endregion

        #region BindingControls

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<SortViewController, ISortViewModel> set)
        {
            var dataSource = SetupTableViewSource(tableView);

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewSource(UITableView tableView)
        {
            var dataSource = new MvxSimpleTableViewSource(tableView, Cells.SortCell.Key, Cells.SortCell.Key);

            return dataSource;
        }

        protected virtual void BindCancelButton(UIButton cancelBtn, MvxFluentBindingDescriptionSet<SortViewController, ISortViewModel> set)
        {
            set.Bind(cancelBtn).To(vm => vm.CancelCommand);
        }

        protected virtual void BindBackgroundView(UIView backgroundView, MvxFluentBindingDescriptionSet<SortViewController, ISortViewModel> set)
        {
            set.Bind(backgroundView).To(vm => vm.CancelCommand).For("Tap");
        }

        #endregion

        #region CommonViewController implementation

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<SortViewController, ISortViewModel>();

            BindTableView(_tableView, set);

            BindCancelButton(_cancelBtn, set);

            BindBackgroundView(_backgroundView, set);

            set.Apply();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override void InitializeControls()
        {
            View.BackgroundColor = UIColor.Clear;

            _titleSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();

            SetupTitle(_title);

            SetupContainerView(_containerView);

            SetupTableView(_tableView);

            SetupCancelButton(_cancelBtn);

            BackgroundView = new UIView(UIScreen.MainScreen.Bounds) { UserInteractionEnabled = true, BackgroundColor = UIColor.Black, Alpha = 0 };
        }

        protected override void CleanUp()
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            ReleaseDesignerOutlets();
        }

        #endregion

        #endregion

        #region Public

        public override void ViewDidAppear(bool animated)
        {
            RecalculateContainerViewConstraints();

            base.ViewDidAppear(animated);
        }

        #endregion

    }
}

