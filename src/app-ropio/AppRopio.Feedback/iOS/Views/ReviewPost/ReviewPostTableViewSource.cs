using System;
using AppRopio.Feedback.Core.ViewModels.Items;
using AppRopio.Feedback.iOS.Views.ReviewPost.Cell.Score;
using AppRopio.Feedback.iOS.Views.ReviewPost.Cell.Text;
using AppRopio.Feedback.iOS.Views.ReviewPost.Cell.TotalScore;
using AppRopio.Models.Feedback.Responses;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.ReviewPost
{
	public class ReviewPostTableViewSource : MvxStandardTableViewSource
	{
        public UIView FieldInputAccessoryView { get; set; }

		#region Constructor

		public ReviewPostTableViewSource(UITableView tableView)
			: base(tableView)
		{
		}

		#endregion

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
		{
			var itemVm = item as IReviewParameterItemVm;

            switch (itemVm.WidgetType)
			{
                case ReviewWidgetType.Score:
					return tableView.DequeueReusableCell(ScoreCell.Key, indexPath);

                case ReviewWidgetType.TotalScore:
                    return tableView.DequeueReusableCell(TotalScoreCell.Key, indexPath);

                case ReviewWidgetType.Text:
                    var cell = (TextCell)tableView.DequeueReusableCell(TextCell.Key, indexPath);
                    cell.FieldInputAccessoryView = FieldInputAccessoryView;
                    return cell;
			}

			return null;
		}
	}
}
