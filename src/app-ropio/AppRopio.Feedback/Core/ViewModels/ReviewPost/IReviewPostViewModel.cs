using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Feedback.Core.ViewModels.Items;
using MvvmCross.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.ReviewPost
{
    public interface IReviewPostViewModel : IBaseViewModel
    {
        IMvxCommand PostCommand { get; }

        MvxObservableCollection<IReviewParameterItemVm> ReviewItems { get; }

        bool CanPostReview { get; }
    }
}