using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Feedback.Responses;
using MvvmCross.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.ReviewDetails
{
    public interface IReviewDetailsViewModel : IBaseViewModel
    {
        IMvxCommand ProductDetailsCommand { get; }

        IMvxCommand EditReviewCommand { get; }

        IMvxCommand DeleteReviewCommand { get; }

        string ProductTitle { get; }

        string ProductImageUrl { get; }

        DateTime Date { get; }

        string Text { get; }

        IScoreViewModel ScoreViewModel { get; }

        bool CanEdit { get; }
    }
}