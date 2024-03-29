﻿using AppRopio.Base.Core;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Feedback.Core;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using FFImageLoading.Cross;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.ReviewDetails
{
    public partial class ReviewDetailsViewController : CommonViewController<IReviewDetailsViewModel>
    {
		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

		public ReviewDetailsViewController() : base("ReviewDetailsViewController", null)
        {
        }

		#region Protected

		#region InitializationControls

		protected virtual void SetupProductTitle(UILabel productTitleLabel)
		{
			productTitleLabel.SetupStyle(ThemeConfig.ReviewDetails.ProductTitle);
		}

		protected virtual void SetupDate(UILabel dateLabel)
		{
			dateLabel.SetupStyle(ThemeConfig.ReviewDetails.Date);
		}

		protected virtual void SetupText(UILabel textLabel)
		{
			textLabel.SetupStyle(ThemeConfig.ReviewDetails.Text);
		}

		protected virtual void SetupEditButton(UIButton editButton)
		{
			editButton.SetupStyle(ThemeConfig.ReviewDetails.EditButton);
            editButton.WithTitleForAllStates(LocalizationService.GetLocalizableString(FeedbackConstants.RESX_NAME, "ReviewDetails_Edit"));
		}

		protected virtual void SetupDeleteButton(UIButton deleteButton)
		{
			deleteButton.SetupStyle(ThemeConfig.ReviewDetails.DeleteButton);
            deleteButton.WithTitleForAllStates(LocalizationService.GetLocalizableString(FeedbackConstants.RESX_NAME, "ReviewDetails_Delete"));
		}

		protected virtual void SetupScoreView(UIView containerView)
		{
			var scoreView = Mvx.IoCProvider.Resolve<IMvxIosViewCreator>().CreateView(ViewModel.ScoreViewModel) as UIView;
			if (scoreView != null)
			{
				scoreView.ChangeFrame(x: 0, y: containerView.Frame.Height / 2 - 10, h: 20);
				containerView.AddSubview(scoreView);
			}
		}

		#endregion

		#region BindingControls

		protected virtual void BindDate(UILabel date, MvxFluentBindingDescriptionSet<ReviewDetailsViewController, IReviewDetailsViewModel> set)
		{
			var converter = new DateToStringConverter();
            converter.Culture = AppSettings.SettingsCulture;

			set.Bind(date).To(vm => vm.Date).WithConversion(converter, "dd MMMM yyyy");
		}

		protected virtual void BindProductTitle(UILabel productTitle, MvxFluentBindingDescriptionSet<ReviewDetailsViewController, IReviewDetailsViewModel> set)
		{
			set.Bind(productTitle).To(vm => vm.ProductTitle);
		}

		protected virtual void BindImage(UIImageView image, MvxFluentBindingDescriptionSet<ReviewDetailsViewController, IReviewDetailsViewModel> set)
		{
            if (image is MvxCachedImageView imageView)
            {
                imageView.LoadingPlaceholderImagePath = $"res:{ThemeConfig.ReviewDetails.ProductImage.Path}";
                imageView.ErrorPlaceholderImagePath = $"res:{ThemeConfig.ReviewDetails.ProductImage.Path}";

                set.Bind(imageView).For(i => i.ImagePath).To(vm => vm.ProductImageUrl);
            }
		}

		protected virtual void BindText(UILabel text, MvxFluentBindingDescriptionSet<ReviewDetailsViewController, IReviewDetailsViewModel> set)
		{
			set.Bind(text).To(vm => vm.Text);
		}

		protected virtual void BindButtonsView(UIView buttonsView, MvxFluentBindingDescriptionSet<ReviewDetailsViewController, IReviewDetailsViewModel> set)
		{
			set.Bind(buttonsView).For("Visibility").To(vm => vm.CanEdit).WithConversion("Visibility");
		}

		protected virtual void BindEditButton(UIButton editButton, MvxFluentBindingDescriptionSet<ReviewDetailsViewController, IReviewDetailsViewModel> set)
		{
			set.Bind(editButton).To(vm => vm.EditReviewCommand);
		}

		protected virtual void BindDeleteButton(UIButton deleteButton, MvxFluentBindingDescriptionSet<ReviewDetailsViewController, IReviewDetailsViewModel> set)
		{
			set.Bind(deleteButton).To(vm => vm.DeleteReviewCommand);
		}

		protected virtual void BindProductDetails(UIView productDetails, MvxFluentBindingDescriptionSet<ReviewDetailsViewController, IReviewDetailsViewModel> set)
		{
			set.Bind(productDetails).To(vm => vm.ProductDetailsCommand).For("Tap");
		}

		#endregion

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
            Title = LocalizationService.GetLocalizableString(FeedbackConstants.RESX_NAME, "ReviewDetails_Title");

			SetupProductTitle(ProductTitleLabel);
			SetupDate(DateLabel);
			SetupText(TextLabel);
			SetupScoreView(ScoreContainerView);

			SetupEditButton(EditButton);
			SetupDeleteButton(DeleteButton);
		}

		protected override void BindControls()
		{
			var bindingSet = this.CreateBindingSet<ReviewDetailsViewController, IReviewDetailsViewModel>();

			BindDate(DateLabel, bindingSet);
			BindProductTitle(ProductTitleLabel, bindingSet);
			BindImage(ProductImageView, bindingSet);
			BindText(TextLabel, bindingSet);
			BindButtonsView(ButtonsView, bindingSet);
			BindProductDetails(ProductDetailsView, bindingSet);
			BindEditButton(EditButton, bindingSet);
			BindDeleteButton(DeleteButton, bindingSet);

			bindingSet.Apply();
		}

		#endregion

		#endregion
	}
}