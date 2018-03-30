using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Feedback.Core.ViewModels.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.ReviewPost.Services
{
    public interface IReviewPostVmService
    {
        Task<MvxObservableCollection<IReviewParameterItemVm>> LoadReviewApplication(string reviewId, string productGroupId, string productId);

        Task<bool> PostReview(string reviewId, string productGroupId, string productId, List<IReviewParameterItemVm> reviewItems);
    }
}