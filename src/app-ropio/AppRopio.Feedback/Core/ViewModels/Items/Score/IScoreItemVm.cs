using System;
namespace AppRopio.Feedback.Core.ViewModels.Items.Score
{
    public interface IScoreItemVm : IReviewParameterItemVm
    {
        int MaxValue { get; set; }

        int Value { get; set; }
    }
}