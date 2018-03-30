using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;
namespace AppRopio.Base.Auth.iOS.Views.SignUp
{
	public delegate UITableViewCell SignUpCellFabric(UITableView tableView, Foundation.NSIndexPath indexPath, ISignUpItemBaseViewModel model);

	public class SignUpTableViewSource : MvxTableViewSource
	{
		private SignUpCellFabric cellFabric;

		public SignUpTableViewSource(UITableView tableView, SignUpCellFabric cellFabric) : base(tableView)
		{
			this.cellFabric = cellFabric;
		}

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
			return cellFabric(tableView, indexPath, item as ISignUpItemBaseViewModel);
		}


	}
}
