using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Feedback.Responses;

namespace AppRopio.Feedback.Core.ViewModels.ReviewDetails
{
    public class ScoreViewModel : BaseViewModel, IScoreViewModel
    {
        private ReviewRating _score;

        public ReviewRating Score
        {
            get { return _score; }
            set { SetProperty(ref _score, value); }
        }
    }
}