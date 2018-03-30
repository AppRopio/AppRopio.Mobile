using System;
namespace AppRopio.Feedback.Core.ViewModels.Items.TotalScore
{
    public interface ITotalScoreItemVm : IReviewParameterItemVm
    {
		int MaxValue { get; set; }

        int Value { get; set; }
    }
}