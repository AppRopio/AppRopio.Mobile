using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Information.Responses;

namespace AppRopio.Base.Information.API.Services
{
    public interface IInformationService
    {
        Task<List<Article>> GetArticles();

        Task<ArticleInfo> GetArticle(string id);
    }
}