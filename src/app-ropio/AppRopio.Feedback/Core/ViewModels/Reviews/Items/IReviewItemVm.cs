using System;
using AppRopio.Base.Core.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.Reviews.Items
{
    public interface IReviewItemVm : IBaseViewModel
    {
		string Id { get; set; }

		string ProductTitle { get; set; }

		string ProductImageUrl { get; set; }

		string Author { get; set; }

		DateTime Date { get; set; }

		string Text { get; set; }

		int? Score { get; set; }
    }
}