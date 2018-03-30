using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Information.Enums;
using AppRopio.Models.Information.Responses;
using Newtonsoft.Json;

namespace AppRopio.Base.Information.API.Services.Fakes
{
    public class FakeInformationService : IInformationService
    {
        private List<Article> _articles = new List<Article>()
        {
            new Article() { Id = "1", Title = "О компании", Type = ArticleType.Text},
            new Article() { Id = "2", Title = "Условия доставки", Type = ArticleType.Html},
            new Article() { Id = "3", Title = "Оферта", Type = ArticleType.Url}
        };

        private Dictionary<string, ArticleInfo> _fullArticles = new Dictionary<string, ArticleInfo>()
        {
            ["1"] = new ArticleInfo() { Content = "Hello world!" },
            ["2"] = new ArticleInfo() { Content = "<html><body><h1>Hello world!</h1><body></html>" },
            ["3"] = new ArticleInfo() { Content = "http://google.com" }
        };

		public async Task<List<Article>> GetArticles()
		{
            await Task.Delay(500);

            return _articles;
		}

        public async Task<ArticleInfo> GetArticle(string id)
        {
			await Task.Delay(500);

            return _fullArticles[id];
        }
    }
}