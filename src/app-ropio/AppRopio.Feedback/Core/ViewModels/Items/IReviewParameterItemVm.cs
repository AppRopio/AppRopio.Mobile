using System;
using AppRopio.Models.Feedback.Responses;

namespace AppRopio.Feedback.Core.ViewModels.Items
{
    public interface IReviewParameterItemVm
    {
		string Id { get; set; }

		ReviewWidgetType WidgetType { get; set; }

		string Title { get; set; }
    }
}