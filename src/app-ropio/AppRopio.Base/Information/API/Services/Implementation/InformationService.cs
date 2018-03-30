using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Information.Responses;

namespace AppRopio.Base.Information.API.Services.Implementation
{
    public class InformationService : BaseService, IInformationService
    {
		protected string INFORMATION_URL = "information";
        protected string INFORMATION_ARTICLE_URL = "information/article";

		public async Task<List<Article>> GetArticles()
		{
			return await Get<List<Article>>(INFORMATION_URL);
		}

		public async Task<ArticleInfo> GetArticle(string id)
		{
            return await Get<ArticleInfo>($"{INFORMATION_ARTICLE_URL}?id={id}");
		}
    }
}