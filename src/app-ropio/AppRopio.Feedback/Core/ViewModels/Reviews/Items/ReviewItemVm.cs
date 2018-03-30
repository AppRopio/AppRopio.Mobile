using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Feedback.Responses;

namespace AppRopio.Feedback.Core.ViewModels.Reviews.Items
{
    public class ReviewItemVm : BaseViewModel, IReviewItemVm
    {
		public string Id { get; set; }

        public string ProductTitle { get; set; }

        public string ProductImageUrl { get; set; }

        public string Author { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }

        public int? Score { get; set; }

        public ReviewItemVm(Review model)
        {
            Id = model.Id;
            ProductTitle = model.ProductTitle;
            ProductImageUrl = model.ProductImageUrl;
            Author = model.Author;
            Date = model.Date;
            Text = model.Text;
            Score = model.Rating?.Value;
        }
    }
}