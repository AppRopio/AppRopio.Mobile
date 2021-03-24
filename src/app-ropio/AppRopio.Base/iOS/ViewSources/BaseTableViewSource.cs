using System;
using System.Windows.Input;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding;
using UIKit;

namespace AppRopio.Base.iOS.ViewSources
{
    public class BaseTableViewSource : MvxSimpleTableViewSource
    {
        #region Fields

        #endregion

        #region Commands

        public ICommand LoadMoreCommand { get; set; }

        #endregion

        #region Properties

        protected int FromBottomCellStartLoadingIndex { get; set; } = 5;

        #endregion

        #region Constructors

        public BaseTableViewSource(IntPtr handle)
            : base(handle)
        {
            
        }

        public BaseTableViewSource(UITableView tableView, string nibName, string cellIdentifier = null, NSBundle bundle = null)
            : base(tableView, nibName, cellIdentifier, bundle)
        {
            
        }

        public BaseTableViewSource(UITableView tableView, Type cellType, string cellIdentifier = null)
            : base(tableView, cellType, cellIdentifier)
        {
            
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        #endregion

        #region Public

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            if ((indexPath.Row >= (ItemsSource.Count() - FromBottomCellStartLoadingIndex)) &&
                LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
            {
                LoadMoreCommand.Execute(null);
            }
        }

        #endregion
    }
}
