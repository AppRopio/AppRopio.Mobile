using AppRopio.Base.iOS.Views;
using UIKit;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment;
using MvvmCross.Platform;
using AppRopio.ECommerce.Basket.iOS.Services;
using System.Linq;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS;
using CoreAnimation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using System;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Payment
{
    public partial class PaymentViewController : CommonViewController<IPaymentViewModel>
    {
        protected Models.Payments PaymentsTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.Payments; } }

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

        public PaymentViewController() : base("PaymentViewController", null)
        {
            ModalPresentationStyle = UIModalPresentationStyle.Custom;
            TransitioningDelegate = new CustomTransitioningDelegate();
        }

        #region Private

        private void RecalculateContainerViewConstraints()
        {
            ContainerViewHeightConstraint.Constant = 50;

            if (ViewModel.Items != null && ViewModel.Items.Any())
                ContainerViewHeightConstraint.Constant += (ViewModel.Items.Count() * PaymentsTheme.Cell.Size.Height.Value) + 0.If_iPhoneX(30);

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

        #region CustomTransitioningDelegate

        private class CustomTransitioningDelegate : UIViewControllerTransitioningDelegate
        {
            private const double ANIMATION_DURATION = 0.2;

            public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
            {
                if (presented is PaymentViewController)
                    return new ShowTransitioning();
                return null;
            }

            public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
            {
                if (dismissed is PaymentViewController)
                {
                    var result = new HideSortTransitioning();
                    return result;
                }
                return null;
            }

            public class ShowTransitioning : UIViewControllerAnimatedTransitioning
            {
                public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
                {
                    var container = transitionContext.ContainerView;
                    var @to = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey) as PaymentViewController;

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
                    var @from = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey) as PaymentViewController;

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

        #region CommonViewController implementation

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<PaymentViewController, IPaymentViewModel>();

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

            SetupTitleLabel(_titleLabel);
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

        #region InitializationControls

        protected virtual void SetupTitleLabel(UILabel titleLabel)
        {
            titleLabel.Text = "Выбор оплаты";
            titleLabel.SetupStyle(PaymentsTheme.Title);
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
            tableView.RegisterNibForCellReuse(Cells.PaymentCell.Nib, Cells.PaymentCell.Key);
            tableView.RowHeight = (System.nfloat)PaymentsTheme.Cell.Size.Height;
        }

        protected virtual void SetupCancelButton(UIButton cancelBtn)
        {
            cancelBtn.SetupStyle(PaymentsTheme.CancelButton);
        }

        #endregion

        #region BindingControls

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<PaymentViewController, IPaymentViewModel> set)
        {
            var dataSource = SetupTableViewSource(tableView);

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewSource(UITableView tableView)
        {
            var dataSource = new MvxSimpleTableViewSource(tableView, Cells.PaymentCell.Key, Cells.PaymentCell.Key);

            return dataSource;
        }

        protected virtual void BindCancelButton(UIButton cancelBtn, MvxFluentBindingDescriptionSet<PaymentViewController, IPaymentViewModel> set)
        {
            set.Bind(cancelBtn).To(vm => vm.CancelCommand);
        }

        protected virtual void BindBackgroundView(UIView backgroundView, MvxFluentBindingDescriptionSet<PaymentViewController, IPaymentViewModel> set)
        {
            set.Bind(backgroundView).To(vm => vm.CancelCommand).For("Tap");
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

