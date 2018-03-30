using System;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.Feedback.Core.ViewModels.MyReviews;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.MyReviews
{
    public partial class MyReviewsViewController : CommonViewController<IMyReviewsViewModel>
    {
		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

		public MyReviewsViewController()
            : base("MyReviewsViewController", null)
        {
        }

		#region Protected

		#region InitializationControls

		protected virtual void SetupTableView(UITableView tableView)
		{
			tableView.RegisterNibForCellReuse(Cell.MyReviewCell.Nib, Cell.MyReviewCell.Key);

			tableView.RowHeight = (nfloat)ThemeConfig.MyReviewCell.Size.Height;
			tableView.SeparatorColor = ThemeConfig.MyReviewCell.SeparatorColor.ToUIColor();
			tableView.TableFooterView = new UIView();
		}

		#endregion

		#region BindingControls

		protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<MyReviewsViewController, IMyReviewsViewModel> set)
		{
			var dataSource = SetupTableViewDataSource(tableView);

			tableView.Source = dataSource;

			set.Bind(dataSource).To(vm => vm.Reviews);
			set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
			set.Bind(dataSource).For(ds => ds.LoadMoreCommand).To(vm => vm.LoadMoreCommand);

			tableView.ReloadData();
		}

		protected virtual BaseTableViewSource SetupTableViewDataSource(UITableView tableView)
		{
			return new BaseTableViewSource(tableView, Cell.MyReviewCell.Key)
			{
				DeselectAutomatically = true
			};
		}

		#endregion

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
			Title = "Мои отзывы";

			SetupTableView(TableView);
		}

		protected override void BindControls()
		{
			var bindingSet = this.CreateBindingSet<MyReviewsViewController, IMyReviewsViewModel>();

			BindTableView(TableView, bindingSet);

			bindingSet.Apply();
		}

		#endregion

		#endregion
	}
}