using System;
using AppRopio.Base.iOS;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Horizontal;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal.Cells;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal
{
    public partial class HorizontalCollectionCell : MvxTableViewCell
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public const float HORIZONTAL_COLLECTION_HEIGHT = 98;

        public static readonly NSString Key = new NSString("HorizontalCollectionCell");
        public static readonly UINib Nib;

        static HorizontalCollectionCell()
        {
            Nib = UINib.FromName("HorizontalCollectionCell", NSBundle.MainBundle);
        }

        protected HorizontalCollectionCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupName(_name);

            SetupCollectionView(_collectionView);

            _bottonSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.Title);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            collectionView.ContentInset = new UIEdgeInsets(0, 16, 0, 16);
            collectionView.ShowsHorizontalScrollIndicator = false;

            collectionView.RegisterNibForCell(HorizontalColorCell.Nib, HorizontalColorCell.Key);
            collectionView.RegisterNibForCell(HorizontalTextCell.Nib, HorizontalTextCell.Key);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<HorizontalCollectionCell, IHorizontalCollectionFiVm>();

            BindName(_name, set);
            BindCollectionView(_collectionView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<HorizontalCollectionCell, IHorizontalCollectionFiVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<HorizontalCollectionCell, IHorizontalCollectionFiVm> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);
            collectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
        }

        protected virtual MvxCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new FiltersHorizontalCollectionViewSource(collectionView);
        }

        #endregion
    }
}
