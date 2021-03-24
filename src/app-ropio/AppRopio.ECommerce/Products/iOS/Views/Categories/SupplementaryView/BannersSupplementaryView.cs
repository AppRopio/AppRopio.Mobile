using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Disabled;
using AppRopio.ECommerce.Products.iOS.Views.Categories.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.SupplementaryView
{
    public class BannersSupplementaryView : MvxCollectionReusableView
    {
        #region Fields

        public static readonly string ReuseIdentifierString_Header = "BannersSupplementaryView_HEADER";
        public static readonly string ReuseIdentifierString_Footer = "BannersSupplementaryView_FOOTER";

        private UICollectionView _collectionView;

        #endregion

        #region Constructor

        public BannersSupplementaryView()
        {
            Initialize();
            this.DelayBind(OnBindContent);
        }

        public BannersSupplementaryView(IntPtr handle)
            : base(handle)
        {
            Initialize();
            this.DelayBind(OnBindContent);
        }

        #endregion

        #region Private

        private void Initialize()
        {
            var width = UIScreen.MainScreen.Bounds.Width;
            var height = width * 9 / 16;

            _collectionView = new UICollectionView(Bounds, new UICollectionViewFlowLayout
            {
                ItemSize = Bounds.Size,
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                MinimumLineSpacing = 0,
                MinimumInteritemSpacing = 0,
                SectionInset = UIEdgeInsets.Zero
            });
            _collectionView.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
            _collectionView.PagingEnabled = true;
            _collectionView.ShowsHorizontalScrollIndicator = false;
            _collectionView.ShowsVerticalScrollIndicator = false;
            _collectionView.RegisterNibForCell(BannerCell.Nib, BannerCell.Key);

            AddSubview(_collectionView);
        }

        private void OnBindContent()
        {
            var dataSource = new MvxCollectionViewSource(_collectionView, BannerCell.Key);

            var set = this.CreateBindingSet<BannersSupplementaryView, IDCategoriesViewModel>();

            if (ReuseIdentifier == ReuseIdentifierString_Header)
                set.Bind(dataSource).To(vm => vm.TopBanners);
            else if (ReuseIdentifier == ReuseIdentifierString_Footer)
                set.Bind(dataSource).To(vm => vm.BottomBanners);
            
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.BannerSelectionChangedCommand);

            set.Apply();

            _collectionView.Source = dataSource;
            _collectionView.ReloadData();
        }

        #endregion
    }
}
