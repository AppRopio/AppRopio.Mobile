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
using FFImageLoading.Cross;

namespace AppRopio.Feedback.iOS.Views.MyReviews.Cell
{
    public partial class MyReviewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("MyReviewCell");
        public static readonly UINib Nib;

		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

        static MyReviewCell()
        {
            Nib = UINib.FromName("MyReviewCell", NSBundle.MainBundle);
        }

        protected MyReviewCell(IntPtr handle) : base(handle)
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
            SetupDate(DateLabel);
            SetupProductTitle(ProductTitleLabel);
            SetupReviewText(ReviewTextLabel);
            SetupBadge();
        }

		protected virtual void SetupDate(UILabel dateLabel)
		{
            dateLabel.SetupStyle(ThemeConfig.MyReviewCell.Date);
		}

		protected virtual void SetupProductTitle(UILabel productTitleLabel)
		{
            productTitleLabel.SetupStyle(ThemeConfig.MyReviewCell.Title);
		}

		protected virtual void SetupReviewText(UILabel reviewTextLabel)
		{
            reviewTextLabel.SetupStyle(ThemeConfig.MyReviewCell.Text);
		}

		protected virtual void SetupBadge()
		{
			BadgeLabel.SetupStyle(ThemeConfig.ReviewCell.Badge);
			BadgeView.BackgroundColor = ThemeConfig.MyReviewCell.Badge.Background.ToUIColor();
            BadgeView.Layer.CornerRadius = (float)ThemeConfig.MyReviewCell.Badge.Layer.CornerRadius;
		}

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var bindingSet = this.CreateBindingSet<MyReviewCell, IReviewItemVm>();

            BindText(ReviewTextLabel, bindingSet);
            BindTitle(ProductTitleLabel, bindingSet);
            BindDate(DateLabel, bindingSet);
            BindImage(ProductImageView, bindingSet);
            BindScore(BadgeLabel, BadgeView, bindingSet);

            bindingSet.Apply();
        }

		protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<MyReviewCell, IReviewItemVm> set)
		{
            set.Bind(title).To(vm => vm.ProductTitle);
		}

		protected virtual void BindDate(UILabel date, MvxFluentBindingDescriptionSet<MyReviewCell, IReviewItemVm> set)
		{
			var converter = new DateToStringConverter();
            converter.Culture = AppSettings.SettingsCulture;
			set.Bind(date).To(vm => vm.Date).WithConversion(converter, "dd MMMM yyyy");
		}

		protected virtual void BindText(UILabel text, MvxFluentBindingDescriptionSet<MyReviewCell, IReviewItemVm> set)
		{
            set.Bind(text).To(vm => vm.Text);
		}

		protected virtual void BindImage(UIImageView image, MvxFluentBindingDescriptionSet<MyReviewCell, IReviewItemVm> set)
		{
            if (image is MvxCachedImageView imageView)
            {
                imageView.LoadingPlaceholderImagePath = $"res:{ThemeConfig.MyReviewCell.ProductImage.Path}";
                imageView.ErrorPlaceholderImagePath = $"res:{ThemeConfig.MyReviewCell.ProductImage.Path}";

                set.Bind(imageView).For(i => i.ImagePath).To(vm => vm.ProductImageUrl);
            }
        }

		protected virtual void BindScore(UILabel score, UIView scoreContainer, MvxFluentBindingDescriptionSet<MyReviewCell, IReviewItemVm> set)
		{
			set.Bind(score).To(vm => vm.Score);
            set.Bind(scoreContainer).For("Visibility").To(vm => vm.Score).WithConversion("Visibility");
		}

        #endregion
    }
}
