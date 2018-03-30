using System;
namespace AppRopio.Feedback.Core.ViewModels.Items.Text
{
    public interface ITextItemVm : IReviewParameterItemVm
    {
        int MinLength { get; set; }

        int MaxLength { get; set;}

        string Text { get; set; }
    }
}