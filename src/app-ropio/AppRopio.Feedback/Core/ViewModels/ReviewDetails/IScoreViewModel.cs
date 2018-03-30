using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Feedback.Responses;

namespace AppRopio.Feedback.Core.ViewModels.ReviewDetails
{
    public interface IScoreViewModel : IBaseViewModel
    {
        ReviewRating Score { get; set; }
    }
}