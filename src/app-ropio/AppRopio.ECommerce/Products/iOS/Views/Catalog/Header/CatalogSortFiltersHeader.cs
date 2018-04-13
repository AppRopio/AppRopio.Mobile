using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Header;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using UIKit;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.ECommerce.Products.Core;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog.Header
{
    public class CatalogSortFiltersHeader : MvxCollectionReusableView, IMvxIosView<CatalogSortFiltersHeaderVM>
    {
        #region Fields

        private UIButton _sortButton;

        private UIButton _filtersButton;

        #endregion

        #region Properties

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        protected bool IsBinded { get; set; }

        public override NSString ReuseIdentifier
        {
            get
            {
                return new NSString(Mvx.Resolve<IProductConfigService>().Config.Header?.TypeName ?? nameof(CatalogSortFiltersHeader));
            }
        }

        public MvxViewModelRequest _request;
        public MvxViewModelRequest Request
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;
                ViewModel = (CatalogSortFiltersHeaderVM)((value as MvxViewModelInstanceRequest).ViewModelInstance);
                DataContext = ViewModel;

                Mvx.Resolve<IMvxLog>().Trace("CatalogSortFiltersHeader Request is set");
            }
        }

        public CatalogSortFiltersHeaderVM ViewModel { get; set; }
        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (CatalogSortFiltersHeaderVM)value;

                Mvx.Resolve<IMvxLog>().Trace("CatalogSortFiltersHeader ViewModel is set");
            }
        }

        #endregion

        #region Constructor

        public CatalogSortFiltersHeader()
        {
            Initialize();
            this.DelayBind(OnBindContent);

            Mvx.Resolve<IMvxLog>().Trace("CatalogSortFiltersHeader created");
        }

        public CatalogSortFiltersHeader(IntPtr handle)
            : base(handle)
        {
            Initialize();
            this.DelayBind(OnBindContent);

            Mvx.Resolve<IMvxLog>().Trace("CatalogSortFiltersHeader created with handle");
        }

        #endregion

        #region Private

        private void Initialize()
        {
            UserInteractionEnabled = true;
            
            SetupFiltersButton(
                _filtersButton = new UIButton(UIButtonType.Custom)
                .WithFrame(8, 8, (Frame.Width - 16 / 2) - 4, Frame.Height - 16)
                .WithTitleForAllStates(Mvx.Resolve<ILocalizationService>().GetLocalizableString(ProductsConstants.RESX_NAME, "Catalog_Filters"))
            );

            _filtersButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _filtersButton.AddConstraint(NSLayoutConstraint.Create(_filtersButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1, ((DeviceInfo.ScreenWidth - 16) / 2) - 4));

            SetupSortButton(
                _sortButton = new UIButton(UIButtonType.Custom)
                .WithFrame((Frame.Width / 2) + 4, 8, (Frame.Width - 16 / 2) - 4, Frame.Height - 16)
                .WithTitleForAllStates(Mvx.Resolve<ILocalizationService>().GetLocalizableString(ProductsConstants.RESX_NAME, "Catalog_Sort"))
            );

            _sortButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _sortButton.AddConstraint(NSLayoutConstraint.Create(_sortButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1, ((DeviceInfo.ScreenWidth - 16) / 2) - 4));

            AddSubviews(_filtersButton, _sortButton);

            this.AddConstraints(new[]
            {
                NSLayoutConstraint.Create(_filtersButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, 4),
                NSLayoutConstraint.Create(_filtersButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, -4),
                NSLayoutConstraint.Create(_filtersButton, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, this, NSLayoutAttribute.Leading, 1, 8),

                NSLayoutConstraint.Create(_sortButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, 4),
                NSLayoutConstraint.Create(_sortButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, -4),
                NSLayoutConstraint.Create(_sortButton, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, this, NSLayoutAttribute.Trailing, 1, -8),
            });
        }

        private void OnBindContent()
        {
            if (IsBinded)
                return;

            IsBinded = true;

            var set = this.CreateBindingSet<CatalogSortFiltersHeader, CatalogSortFiltersHeaderVM>();

            BindSortButton(_sortButton, set);
            BindFiltersButton(_filtersButton, set);

            set.Apply();
        }

        #endregion

        #region Protected

        #region InitializationControls

        protected virtual void SetupSortButton(UIButton sortButton)
        {
            sortButton.SetupStyle(ThemeConfig.Products.SortButton);
        }

        protected virtual void SetupFiltersButton(UIButton filtersButton)
        {
            filtersButton.SetupStyle(ThemeConfig.Products.FiltersButton);
        }

        #endregion

        #region BindingControls

        protected virtual void BindSortButton(UIButton sortButton, MvxFluentBindingDescriptionSet<CatalogSortFiltersHeader, CatalogSortFiltersHeaderVM> set)
        {
            set.Bind(sortButton).To(sb => sb.SortCommand);
        }

        protected virtual void BindFiltersButton(UIButton filtersButton, MvxFluentBindingDescriptionSet<CatalogSortFiltersHeader, CatalogSortFiltersHeaderVM> set)
        {
            set.Bind(filtersButton).To(sb => sb.FiltersCommand);
        }

        #endregion

        #endregion
    }
}
