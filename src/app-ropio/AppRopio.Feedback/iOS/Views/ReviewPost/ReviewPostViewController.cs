using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Feedback.Core.ViewModels.ReviewPost;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using AppRopio.Feedback.iOS.Views.ReviewPost.Cell.Score;
using AppRopio.Feedback.iOS.Views.ReviewPost.Cell.Text;
using AppRopio.Feedback.iOS.Views.ReviewPost.Cell.TotalScore;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Feedback.Core;

namespace AppRopio.Feedback.iOS.Views.ReviewPost
{
    public partial class ReviewPostViewController : CommonViewController<IReviewPostViewModel>
    {
        private UIButton _accessoryButton;

		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

		public ReviewPostViewController() : base("ReviewPostViewController", null)
        {
        }

		#region Private

		private void RegisterCells(UITableView tableView)
		{
			tableView.RegisterNibForCellReuse(TotalScoreCell.Nib, TotalScoreCell.Key);
			tableView.RegisterNibForCellReuse(ScoreCell.Nib, ScoreCell.Key);
			tableView.RegisterNibForCellReuse(TextCell.Nib, TextCell.Key);
		}

		#endregion

		#region Protected

		#region InitializationControls

		protected virtual void SetupTableView(UITableView tableView)
		{
			RegisterCells(tableView);

			tableView.RowHeight = UITableView.AutomaticDimension;
			tableView.EstimatedRowHeight = 100;
			tableView.TableFooterView = new UIView();
		}

		protected virtual void SetupPostButton(UIButton postButton)
		{
            postButton.SetupStyle(ThemeConfig.ReviewPost.PostButton);
            postButton.WithTitleForAllStates(LocalizationService.GetLocalizableString(FeedbackConstants.RESX_NAME, "Review_Send"));
		}

		protected virtual void SetupAccessoryButton(UIButton accessoryNextButton)
		{
            accessoryNextButton.SetTitle(LocalizationService.GetLocalizableString(FeedbackConstants.RESX_NAME, "Review_Done"), UIControlState.Normal);
			accessoryNextButton.ChangeFrame(w: DeviceInfo.ScreenWidth, h: 44);
			accessoryNextButton.SetupStyle(ThemeConfig.ReviewPost.AccessoryNextButton);
		}

		#endregion

		#region BindingControls

		protected virtual void BindTable(UITableView tableView, MvxFluentBindingDescriptionSet<ReviewPostViewController, IReviewPostViewModel> set)
		{
			var dataSource = SetupTableViewDataSource(tableView);

			tableView.Source = dataSource;

			set.Bind(dataSource).To(vm => vm.ReviewItems);
		}

		protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
		{
			return new ReviewPostTableViewSource(tableView)
			{
				FieldInputAccessoryView = _accessoryButton,
				DeselectAutomatically = true
			};
		}

		protected virtual void BindPostButton(UIButton post, MvxFluentBindingDescriptionSet<ReviewPostViewController, IReviewPostViewModel> set)
		{
			set.Bind(post).To(vm => vm.PostCommand);
			set.Bind(post).For(v => v.Enabled).To(vm => vm.CanPostReview);
		}

		protected virtual void BindAccessoryButton(UIButton accessoryNextButton, MvxFluentBindingDescriptionSet<ReviewPostViewController, IReviewPostViewModel> set)
		{
			accessoryNextButton.TouchUpInside += (sender, e) => View.EndEditing(true);
		}

		#endregion

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
            Title = LocalizationService.GetLocalizableString(FeedbackConstants.RESX_NAME, "Review_Title");

			RegisterKeyboardActions = true;

			SetupTableView(TableView);
			SetupPostButton(PostButton);
			SetupAccessoryButton(_accessoryButton ?? (_accessoryButton = new UIButton()));
		}

		protected override void BindControls()
		{
			var bindingSet = this.CreateBindingSet<ReviewPostViewController, IReviewPostViewModel>();

			BindTable(TableView, bindingSet);
			BindPostButton(PostButton, bindingSet);
			BindAccessoryButton(_accessoryButton, bindingSet);

			bindingSet.Apply();
		}

		#endregion

		#endregion
	}
}