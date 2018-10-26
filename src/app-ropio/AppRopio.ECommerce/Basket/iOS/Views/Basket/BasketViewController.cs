
using System.Linq;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Basket.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Basket
{
    public partial class BasketViewController : CommonViewController<IBasketViewModel>
    {
        protected Models.Basket BasketTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Basket; } }

        public BasketViewController()
            : base("BasketViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Basket_Title");

            RegisterKeyboardActions = true;


            SetupTableView(_tableView);
            SetupNextButton(_nextButton);
            SetupEmptyView(_emptyView, _emptyImage, _emptyTitle, _epmtyText, _goToButton);
            SetupBottomView(_bottomView);
            ResolveAndSetupLoyalty(_loyaltyWrapper);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<BasketViewController, IBasketViewModel>();

            BindTableView(_tableView, set);
            BindNextButton(_nextButton, set);
            BindEmptyView(_emptyView, _goToButton, set);
            BindBottomView(_bottomView, set);

            if (_loyaltyWrapper.Subviews.Any())
                BindLoyaltyWrapper(_loyaltyWrapper, set);

            set.Apply();
        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();
        }

        #endregion

        #region InitializationControls

        protected virtual void SetupBottomView(UIView view)
        {
            view.SetupStyle(BasketTheme.BottomView);
        }

        protected virtual void SetupTableView(UITableView tableView)
        {
            tableView.TableHeaderView = new UIView();
            tableView.TableFooterView = new UIView();

            tableView.RowHeight = BasketTheme.Cell.Size.Height ?? 120;
            tableView.SeparatorColor = (UIColor)Theme.ColorPalette.Separator;
        }

        protected virtual void SetupNextButton(UIButton nextButton)
        {
            nextButton.SetupStyle(BasketTheme.NextButton);
		}

		protected virtual void SetupEmptyView(UIView emptyView, UIImageView emptyImage, UILabel emptyTitle, UILabel emptyText, UIButton goToButton)
		{
            emptyView.Hidden = true;
            emptyView.BackgroundColor = BasketTheme.EmptyView.Background.ToUIColor();

            emptyImage.SetupStyle(BasketTheme.EmptyView.Image);
            emptyTitle.SetupStyle(BasketTheme.EmptyView.Title);
            emptyText.SetupStyle(BasketTheme.EmptyView.Text);

            goToButton.SetupStyle(BasketTheme.EmptyView.CatalogButton);

            emptyTitle.Text = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Basket_EmptyTitle");
            emptyText.Text = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Basket_EmptyText");
            goToButton.WithTitleForAllStates(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Basket_EmptyButton"));
		}

        protected virtual void ResolveAndSetupLoyalty(UIView loyaltyWrapper)
        {
            var config = Mvx.Resolve<IBasketConfigService>().Config;
            if (config.Loyalty != null && Mvx.Resolve<IViewLookupService>().IsRegistered(config.Loyalty.TypeName))
            {
                var loyaltyView = ViewModel.LoyaltyVm == null ? null : Mvx.Resolve<IMvxIosViewCreator>().CreateView(ViewModel.LoyaltyVm) as UIView;
                if (loyaltyView != null)
                {
                    loyaltyWrapper.Hidden = false;
                    loyaltyWrapper.AddSubviewWithFill(loyaltyView);
                    View.UpdateConstraints();
                }
            }
        }

        #endregion

        #region BindingControls

        protected virtual BasketTableSource SetupTableViewDataSource(UITableView tableView)
        {
            return new BasketTableSource(tableView, BasketCell.Key, BasketCell.Key);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<BasketViewController, IBasketViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(s => s.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
            set.Bind(dataSource).For(s => s.DeleteItemCommand).To(vm => vm.DeleteItemCommand);

            set.Bind(tableView).For("Visibility").To(vm => vm.Items.Count).WithConversion("Visibility");

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual void BindNextButton(UIButton nextButton, MvxFluentBindingDescriptionSet<BasketViewController, IBasketViewModel> set)
        {
            set.Bind(nextButton)
               .For("Title")
               .To(vm => vm.Amount)
               .WithConversion(
                   "StringFormat", 
                   new StringFormatParameter { StringFormat = (arg) => 
                {
                    var str = $"{LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Basket_OrderBy")} {((decimal)arg).ToString(AppSettings.CurrencyFormat, AppSettings.SettingsCulture.NumberFormat)}";
                    return BasketTheme.NextButton.UppercaseTitle ? str.ToUpperInvariant() : str;
                } 
            });
            
            set.Bind(nextButton).To(vm => vm.NextCommand);
            set.Bind(nextButton).For(s => s.Enabled).To(vm => vm.CanGoNext);

            set.Bind(nextButton).For("Visibility").To(vm => vm.Items.Count).WithConversion("Visibility");
        }

        protected virtual void BindBottomView(UIView bottomView, MvxFluentBindingDescriptionSet<BasketViewController, IBasketViewModel> set)
        {
            set.Bind(bottomView).For("Visibility").To(vm => vm.Items.Count).WithConversion("Visibility");
        }

		protected virtual void BindEmptyView(UIView emptyView, UIButton goToButton, MvxFluentBindingDescriptionSet<BasketViewController, IBasketViewModel> set)
		{
            set.Bind(emptyView).For("Visibility").To(vm => vm.IsEmpty).WithConversion("Visibility");
            set.Bind(goToButton).To(vm => vm.CatalogCommand);
        }

        protected virtual void BindLoyaltyWrapper(UIView loyaltyWrapper, MvxFluentBindingDescriptionSet<BasketViewController, IBasketViewModel> set)
        {
            set.Bind(loyaltyWrapper).For("Visibility").To(vm => vm.Items.Count).WithConversion("Visibility");
        }

        #endregion
    }
}

