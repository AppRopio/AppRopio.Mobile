using System.Windows.Input;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items;
using Foundation;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Basket
{
    public class BasketTableSource : MvxSimpleTableViewSource
    {
        public ICommand DeleteItemCommand { get; set; }

        public BasketTableSource(UITableView tableView, string nibName, string cellIdentifier = null, NSBundle bundle = null)
            : base(tableView, nibName, cellIdentifier, bundle)
        {
            UseAnimations = true;
            RemoveAnimation = UITableViewRowAnimation.Left;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete;
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var action = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, Mvx.Resolve<ILocalizationService>().GetLocalizableString(BasketConstants.RESX_NAME, "Basket_DeleteItem"), (rowAction, inPath) => 
            {
                CommitEditingStyle(tableView, UITableViewCellEditingStyle.Delete, inPath);
            });

            return new [] { action };
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
