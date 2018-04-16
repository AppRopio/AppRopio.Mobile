using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Feedback.Responses;
using MvvmCross.Platform;

namespace AppRopio.Feedback.API.Services.Fakes
{
    public class ReviewsFakeService : IReviewsService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        private List<Review> _reviews;

        private List<Review> _myReviews;

        public ReviewsFakeService()
        {
            _reviews = new List<Review>()
            {
                new Review()
                {
                    Id = "1",
                    Author = IsRussianCulture ? "Иван Петров" : "User 1",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 1",
                    Rating = new ReviewRating { Value = 5 },
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                },
                new Review()
                {
                    Id = "4",
                    Author = IsRussianCulture ? "Константин Константинопольский" : "User 2",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 2",
                    Rating = null,
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                },
                new Review()
                {
                    Id = "3",
                    Author = IsRussianCulture ? "Василиса Премудрая" : "User 3",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 3",
                    Rating = new ReviewRating { Value = 1 },
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                },
                new Review()
                {
                    Id = "2",
                    Author = IsRussianCulture ? "Иван Петров" : "User 4",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 4",
                    Rating = new ReviewRating { Value = 5 },
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                },
                new Review()
                {
                    Id = "5",
                    Author = IsRussianCulture ? "Константин Константинопольский" : "User 5",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 5",
                    Rating = new ReviewRating { Value = 4 },
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                },
                new Review()
                {
                    Id = "6",
                    Author = IsRussianCulture ? "Василиса Премудрая" : "User 6",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 6",
                    Rating = new ReviewRating { Value = 1 },
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                }
            };

            _myReviews = new List<Review>()
            {
                new Review()
                {
                    Id = "1",
                    ProductTitle = IsRussianCulture ? "Пиджак цвета и оттенка тёмно-синий или коричневый" : "Product 1",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 1",
                    Rating = new ReviewRating { Value = 5 },
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                },
                new Review()
                {
                    Id = "2",
                    ProductTitle = IsRussianCulture ? "Юбка коричневая" : "Product 2",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 2",
                    Rating = new ReviewRating { Value = 4 },
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                },
                new Review()
                {
                    Id = "3",
                    ProductTitle = IsRussianCulture ?  "Платье в горошек" : "Product 3",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 3",
                    Rating = new ReviewRating { Value = 1 },
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                },
                new Review()
                {
                    Id = "4",
                    ProductTitle = IsRussianCulture ? "Юбка коричневая" : "Product 4",
                    Date = new DateTime(2016, 9, 22),
                    Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Text 4",
                    Rating = null,
                    ProductImageUrl = "http://wlooks.ru/images/article/orig/2016/08/chernaya-yubka-solnce-modnye-obrazy-32.jpg"
                }
            };
        }

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
                Author = IsRussianCulture ? "Константин Константинопольский" : "Author name",
                Text = IsRussianCulture ? "Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он… Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он… Замечательный пиджак! Очень стильная вещь, уютная и тёплая. Ношу его и не нарадуюсь, какой же он…" : "Review text",
                Rating = reviewId == "4" ? null : new ReviewRating()
                {
                    Value = new Random(Environment.TickCount).Next(0, 6),
                    Max = 5
                },
                ProductId = "1",
                ProductTitle = IsRussianCulture ? "Пиджак цвета и оттенка тёмно-синий или коричневый" : "Product title",
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
                    Title = IsRussianCulture ? "Общее впечатление:" : "Overall Impression",
                    MinValue = 1,
                    MaxValue = 5,
                    Value = "4"
                },
				new ReviewParameter()
				{
					Id = "2",
                    WidgetType = ReviewWidgetType.Score,
                    Title = IsRussianCulture ? "Надежность:" : "Reliability",
					MinValue = 1,
					MaxValue = 5,
                    Value = "2"
				},
				new ReviewParameter()
				{
					Id = "3",
					WidgetType = ReviewWidgetType.Score,
                    Title = IsRussianCulture ? "Функциональность:" : "Functionality",
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
                    Value = IsRussianCulture ? "Юбка идет размер в размер. Симатичная за счет сочного цвета и модногов этом сезоне тропического принта. Смотрится очень достойно! Юбка без застежки, просто на широкой резинке, что еще лучше на мой взгляд, т.к. молния в такого типа ткани, как правило, деформируется в процессе носки" : "The skirt goes size in size. Simatic due to the juicy color and fashionable in this season of tropical print. It looks very dignified! A skirt without fastening, just on a wide elastic band, which is even better in my opinion, tk. lightning in this type of fabric, as a rule, deforms in the process of socks"
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