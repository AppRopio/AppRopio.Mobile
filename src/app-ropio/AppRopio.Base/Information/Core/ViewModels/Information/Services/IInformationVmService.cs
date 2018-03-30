using System;
using System.Threading.Tasks;
using AppRopio.Models.Information.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Information.Core.ViewModels.Information.Services
{
    public interface IInformationVmService
    {
        Task<MvxObservableCollection<Article>> LoadArticles();

        Task HandleSelection(Article article);
    }
}