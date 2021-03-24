using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Feedback.Core.ViewModels.Items;
using AppRopio.Feedback.Core.ViewModels.Reviews.Items;
using AppRopio.Models.Feedback.Responses;
using MvvmCross.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.Reviews
{
    public interface IReviewsViewModel : ILoadMoreViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        IMvxCommand ReviewCommand { get; }

        MvxObservableCollection<IReviewItemVm> Reviews { get; }

        bool CanPostReview { get; }
    }
}