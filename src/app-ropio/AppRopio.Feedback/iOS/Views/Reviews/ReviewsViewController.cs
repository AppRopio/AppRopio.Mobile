using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.Feedback.Core.ViewModels.Reviews;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.Reviews
{
    public partial class ReviewsViewController : CommonViewController<IReviewsViewModel>
    {
		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

		public ReviewsViewController() : base("ReviewsViewController", null)
        {
        }

		#region Protected

		#region InitializationControls

		protected virtual void SetupTableView(UITableView tableView)
		{
			tableView.RegisterNibForCellReuse(Cell.ReviewCell.Nib, Cell.ReviewCell.Key);

			tableView.RowHeight = (nfloat)ThemeConfig.ReviewCell.Size.Height;
			tableView.TableFooterView = new UIView();
		}

		protected virtual void SetupReviewButton(UIButton reviewButton)
		{
			reviewButton.SetupStyle(ThemeConfig.ReviewButton);
		}

		#endregion

		#region BindingControls

		protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<ReviewsViewController, IReviewsViewModel> set)
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
			return new BaseTableViewSource(tableView, Cell.ReviewCell.Key)
			{
				DeselectAutomatically = true
			};
		}

		protected virtual void BindReviewButton(UIButton review, MvxFluentBindingDescriptionSet<ReviewsViewController, IReviewsViewModel> set)
		{
			set.Bind(review).For("Visibility").To(vm => vm.CanPostReview).WithConversion("Visibility");
			set.Bind(review).To(vm => vm.ReviewCommand);
		}

		#endregion

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
			Title = "Отзывы";

			SetupTableView(TableView);
			SetupReviewButton(ReviewButton);
		}

		protected override void BindControls()
		{
			var bindingSet = this.CreateBindingSet<ReviewsViewController, IReviewsViewModel>();

            BindTableView(TableView, bindingSet);
            BindReviewButton(ReviewButton, bindingSet);

			bindingSet.Apply();
		}

		#endregion

		#endregion
	}
}