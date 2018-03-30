using System;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using Foundation;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items;
using System.Windows.Input;
namespace AppRopio.ECommerce.Basket.iOS.Views.Basket
{
    public class BasketTableSource : MvxSimpleTableViewSource
    {
        public ICommand DeleteItemCommand { get; set; }

        public BasketTableSource(UITableView tableView, string nibName, string cellIdentifier = null, NSBundle bundle = null)
            : base(tableView, nibName, cellIdentifier, bundle)
        {
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            if (editingStyle == UITableViewCellEditingStyle.Delete && DeleteItemCommand != null)
            {
                var item = GetItemAt(indexPath) as IBasketItemVM;
                if (item != null && DeleteItemCommand.CanExecute(item))
                    DeleteItemCommand.Execute(item);
            }
        }
    }
}
