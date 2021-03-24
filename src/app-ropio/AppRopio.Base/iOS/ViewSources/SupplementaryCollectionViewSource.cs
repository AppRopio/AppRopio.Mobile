using System;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using MvvmCross.Platform.Logging;
using UIKit;

namespace AppRopio.Base.iOS.ViewSources
{
    public class SupplementaryCollectionViewSource : BaseCollectionViewSource
    {
        IMvxLog mvxLog;

        protected readonly object _headerDataContext;
        protected readonly object _footerDataContext;

        public string HeaderReuseID { get; set; }
        public string FooterReuseID { get; set; }

        public SupplementaryCollectionViewSource(UICollectionView collectionView, NSString cellIdentifier, object headerVm = null, object footerVm = null)
            : base(collectionView, cellIdentifier)
        {
            _footerDataContext = footerVm;
            _headerDataContext = headerVm;

            mvxLog = Mvx.Resolve<IMvxLog>();
        }

        protected virtual void SetupReusableView(UICollectionReusableView reusableView, NSString elementKind, NSIndexPath indexPath, object mvxViewModel)
        {
            Mvx.Resolve<IMvxLog>().Trace($"SetupReusableView started {elementKind} {indexPath}");

            if (reusableView is IMvxBindable mvxIosView)
            {
                Mvx.Resolve<IMvxLog>().Trace($"SetupReusableView in process {elementKind} {indexPath}");
                mvxIosView.DataContext = mvxViewModel;
            }
            else
                Mvx.Resolve<IMvxLog>().Warn($"SetupReusableView is not IMvxBindable {elementKind} {indexPath}");

            Mvx.Resolve<IMvxLog>().Trace($"SetupReusableView ended {elementKind} {indexPath}");
        }

        [Export("collectionView:layout:referenceSizeForHeaderInSection:"), CompilerGenerated]
        public virtual CGSize GetReferenceSizeForHeader(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return HeaderReuseID.IsNullOrEmpty() ? CGSize.Empty : new CGSize(collectionView.Bounds.Width, 44);
        }

        [Export("collectionView:layout:referenceSizeForFooterInSection:"), CompilerGenerated]
        public virtual CGSize GetReferenceSizeForFooter(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return FooterReuseID.IsNullOrEmpty() ? CGSize.Empty : new CGSize(collectionView.Bounds.Width, 44);
        }

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            mvxLog.Trace("SupplementaryCollectionViewSource GetViewForSupplementaryElement started");

            if (elementKind == UICollectionElementKindSectionKey.Header && !HeaderReuseID.IsNullOrEmtpy())
            {
                mvxLog.Trace("SupplementaryCollectionViewSource GetViewForSupplementaryElement HEADER resolving started");

                if (_headerDataContext != null)
                {
                    var headerView = collectionView.DequeueReusableSupplementaryView(elementKind, HeaderReuseID, indexPath);
                    mvxLog.Trace("SupplementaryCollectionViewSource GetViewForSupplementaryElement HEADER in process");
                    SetupReusableView(headerView, elementKind, indexPath, _headerDataContext);
                    return headerView;
                }

                mvxLog.Warn("SupplementaryCollectionViewSource GetViewForSupplementaryElement _headerDataContext is NULL");

                mvxLog.Trace("SupplementaryCollectionViewSource GetViewForSupplementaryElement HEADER resolving ended");
            }
            else
                mvxLog.Warn("SupplementaryCollectionViewSource GetViewForSupplementaryElement HEADER ID NULL");

            if (elementKind == UICollectionElementKindSectionKey.Footer && !FooterReuseID.IsNullOrEmtpy())
            {
                mvxLog.Trace("SupplementaryCollectionViewSource GetViewForSupplementaryElement FOOTER resolving started");

                if (_footerDataContext != null)
                {
                    var footerView = collectionView.DequeueReusableSupplementaryView(elementKind, FooterReuseID, indexPath);
                    mvxLog.Trace("SupplementaryCollectionViewSource GetViewForSupplementaryElement FOOTER in process");
                    SetupReusableView(footerView, elementKind, indexPath, _headerDataContext);
                    return footerView;
                }

                mvxLog.Warn("SupplementaryCollectionViewSource GetViewForSupplementaryElement _footerDataContext is NULL");

                mvxLog.Trace("SupplementaryCollectionViewSource GetViewForSupplementaryElement FOOTER resolving ended");

            }
            else
                mvxLog.Warn("SupplementaryCollectionViewSource GetViewForSupplementaryElement FOOTER ID NULL");

            mvxLog.Trace("SupplementaryCollectionViewSource GetViewForSupplementaryElement ended");

            return null;
        }
    }
}
