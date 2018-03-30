using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Feedback.Responses;

namespace AppRopio.Feedback.Core.ViewModels.Items
{
    public class ReviewParameterItemVm : BaseViewModel, IReviewParameterItemVm
    {
		public string Id { get; set; }

		public ReviewWidgetType WidgetType { get; set; }

		public string Title { get; set; }
    }
}