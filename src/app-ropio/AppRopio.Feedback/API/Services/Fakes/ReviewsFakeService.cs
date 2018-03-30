using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Models.Feedback.Responses;

namespace AppRopio.Feedback.API.Services.Fakes
{
    public class ReviewsFakeService : IReviewsService
    {
        private List<Review> _reviews = new List<Review>()
        {
            new Review()
            {
                Id = "1",
                Author = "Иван Петров",
                Date = new DateTime(2016, 9, 22),
                Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = new ReviewRating { Value = 5 },
                ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
            },
			new Review()
			{
				Id = "4",
				Author = "Константин Константинопольский",
				Date = new DateTime(2016, 9, 22),
				Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = null,
                ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
			},
			new Review()
			{
				Id = "3",
				Author = "Василиса Премудрая",
				Date = new DateTime(2016, 9, 22),
				Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = new ReviewRating { Value = 1 },
                ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
			},
						new Review()
			{
				Id = "2",
				Author = "Иван Петров",
				Date = new DateTime(2016, 9, 22),
				Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = new ReviewRating { Value = 5 },
				ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
			},
			new Review()
			{
				Id = "5",
				Author = "Константин Константинопольский",
				Date = new DateTime(2016, 9, 22),
				Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = new ReviewRating { Value = 4 },
				ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
			},
			new Review()
			{
				Id = "6",
				Author = "Василиса Премудрая",
				Date = new DateTime(2016, 9, 22),
				Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = new ReviewRating { Value = 1 },
				ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
			}
        };

		private List<Review> _myReviews = new List<Review>()
		{
			new Review()
			{
				Id = "1",
                ProductTitle = "Пиджак цвета и оттенка тёмно-синий или коричневый",
				Date = new DateTime(2016, 9, 22),
				Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = new ReviewRating { Value = 5 },
                ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
			},
			new Review()
			{
				Id = "2",
                ProductTitle = "Юбка коричневая",
				Date = new DateTime(2016, 9, 22),
				Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = new ReviewRating { Value = 4 },
                ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
            },
			new Review()
			{
				Id = "3",
                ProductTitle = "Платье в горошек",
				Date = new DateTime(2016, 9, 22),
				Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = new ReviewRating { Value = 1 },
                ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
            },
            new Review()
            {
                Id = "4",
                ProductTitle = "Юбка коричневая",
                Date = new DateTime(2016, 9, 22),
                Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = null,
                ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
            }
		};

        public async Task<List<Review>> GetReviews(string productGroupId, string productId, int count, int offset)
		{
            await Task.Delay(500);

            var result = new List<Review>();
            for (int i = 0; i < 20; i++)
                result.AddRange(_reviews);
            return result.Skip(offset).Take(count).ToList();
		}

		public async Task<List<Review>> GetMyReviews(int count, int offset)
		{
			await Task.Delay(500);

			var result = new List<Review>();
			for (int i = 0; i < 20; i++)
				result.AddRange(_myReviews);

            return result.Skip(offset).Take(count).ToList();
		}

        public async Task<ReviewDetails> GetReviewDetails(string reviewId)
        {
            await Task.Delay(500);

            return new ReviewDetails()
            {
                Id = reviewId,
                Date = new DateTime(2016, 9, 22),
                Author = "Константин Константинопольский",
                Text = "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он… Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он… Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…",
                Rating = reviewId == "4" ? null : new ReviewRating()
                {
                    Value = new Random(Environment.TickCount).Next(0, 6),
                    Max = 5
                },
                ProductId = "1",
                ProductTitle = "Пиджак цвета и оттенка тёмно-синий или коричневый",
                ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg",
                Editable = reviewId == "1"
            };
        }

        public async Task<List<ReviewParameter>> GetReviewApplication(string reviewId, string productGroupId, string productId)
		{
			await Task.Delay(500);

            return new List<ReviewParameter>()
            {
                new ReviewParameter()
                {
                    Id = "1",
                    WidgetType = ReviewWidgetType.TotalScore,
                    Title = "Общее впечатление:",
                    MinValue = 1,
                    MaxValue = 5,
                    Value = "4"
                },
				new ReviewParameter()
				{
					Id = "2",
                    WidgetType = ReviewWidgetType.Score,
                    Title = "Надежность:",
					MinValue = 1,
					MaxValue = 5,
                    Value = "2"
				},
				new ReviewParameter()
				{
					Id = "3",
					WidgetType = ReviewWidgetType.Score,
                    Title = "Функциональность:",
					MinValue = 1,
					MaxValue = 5,
                    Value = "3"
				},
				new ReviewParameter()
				{
					Id = "4",
                    WidgetType = ReviewWidgetType.Text,
                    MinValue = 3,
                    MaxValue = 100,
                    Value = "Юбка идет размер в размер. Симатичная за счет сочного цвета и модногов этом сезоне тропического принта. Смотрится очень достойно! Юбка без застежки, просто на широкой резинке, что еще лучше на мой взгляд, т.к. молния в такого типа ткани, как правило, деформируется в процессе носки"
				}
            };
		}

        public async Task PostReview(string reviewId, string productGroupId, string productId, List<ReviewParameter> parameters)
        {
			await Task.Delay(500);
        }

		public async Task DeleteReview(string reviewId)
		{
			await Task.Delay(500);
		}
    }
}