using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Information.API.Services;
using AppRopio.Base.Information.Core.Services;
using AppRopio.Models.Information.Enums;
using AppRopio.Models.Information.Responses;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.Information.Core.ViewModels.Information.Services
{
    public class InformationVmService : BaseVmService, IInformationVmService
    {
		#region Services

		protected IInformationService ApiService { get { return Mvx.IoCProvider.Resolve<IInformationService>(); } }

		#endregion

		public async Task<MvxObservableCollection<Article>> LoadArticles()
		{
			MvxObservableCollection<Article> dataSource = null;

			try
			{
                var articles = await ApiService.GetArticles();

                dataSource = new MvxObservableCollection<Article>(articles);
			}
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}

			return dataSource;
		}

        public async Task HandleSelection(Article article)
        {
            try
            {
                var info = await ApiService.GetArticle(article.Id);

                var navigationService = Mvx.IoCProvider.Resolve<IInformationNavigationVmService>();

                switch (article.Type)
                {
                    case ArticleType.Html:
                        navigationService.NavigateToWebContent(new BaseWebContentBundle(NavigationType.Push, article.Title, info.Content));
                        break;

                    case ArticleType.Text:
                        navigationService.NavigateToTextContent(new BaseTextContentBundle(article.Title, info.Content, NavigationType.Push));
						break;

                    case ArticleType.Url:
                        navigationService.NavigateToUrl(new BaseWebContentBundle(NavigationType.Push, article.Title, null, info.Content));
						break;
                }
            }
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}
        }
	}
}