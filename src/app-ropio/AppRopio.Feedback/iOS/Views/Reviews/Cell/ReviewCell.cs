using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Feedback.Core.ViewModels.Reviews.Items;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Converters;

namespace AppRopio.Feedback.iOS.Views.Reviews.Cell
{
    public partial class ReviewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ReviewCell");
        public static readonly UINib Nib;

		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

		static ReviewCell()
        {
            Nib = UINib.FromName("ReviewCell", NSBundle.MainBundle);
        }

        protected ReviewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupBadge();
            SetupAuthor(AuthorLabel);
            SetupDate(DateLabel);
            SetupText(ContentLabel);
        }

		protected virtual void SetupBadge()
		{
            BadgeLabel.SetupStyle(ThemeConfig.ReviewCell.Badge);
            BadgeView.BackgroundColor = ThemeConfig.ReviewCell.Badge.Background.ToUIColor();
			BadgeView.Layer.CornerRadius = (float)ThemeConfig.ReviewCell.Badge.Layer.CornerRadius;
		}

        protected virtual void SetupAuthor(UILabel authorLabel)
		{
            authorLabel.SetupStyle(ThemeConfig.ReviewCell.Author);
		}

		protected virtual void SetupDate(UILabel dateLabel)
		{
            dateLabel.SetupStyle(ThemeConfig.ReviewCell.Date);
		}

		protected virtual void SetupText(UILabel textLabel)
		{
            textLabel.SetupStyle(ThemeConfig.ReviewCell.Text);
		}

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var bindingSet = this.CreateBindingSet<ReviewCell, IReviewItemVm>();

            BindAuthor(AuthorLabel, bindingSet);
            BindDate(DateLabel, bindingSet);
            BindText(ContentLabel, bindingSet);
            BindScore(BadgeLabel, BadgeView, bindingSet);

            bindingSet.Apply();
        }

		protected virtual void BindAuthor(UILabel author, MvxFluentBindingDescriptionSet<ReviewCell, IReviewItemVm> set)
		{
            set.Bind(author).To(vm => vm.Author);
		}

		protected virtual void BindDate(UILabel date, MvxFluentBindingDescriptionSet<ReviewCell, IReviewItemVm> set)
		{
            var converter = new DateToStringConverter();
            converter.Culture = AppSettings.SettingsCulture;
            set.Bind(date).To(vm => vm.Date).WithConversion(converter, "dd MMMM yyyy");
		}

		protected virtual void BindText(UILabel text, MvxFluentBindingDescriptionSet<ReviewCell, IReviewItemVm> set)
		{
            set.Bind(text).To(vm => vm.Text);
        }

		protected virtual void BindScore(UILabel score, UIView scoreContainer, MvxFluentBindingDescriptionSet<ReviewCell, IReviewItemVm> set)
		{
            set.Bind(score).To(vm => vm.Score);
            set.Bind(scoreContainer).For("Visibility").To(vm => vm.Score).WithConversion("Visibility");
		}

        #endregion
    }
}