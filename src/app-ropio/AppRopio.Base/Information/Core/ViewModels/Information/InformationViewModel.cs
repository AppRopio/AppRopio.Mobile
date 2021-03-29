using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Information.Core.ViewModels.Information.Services;
using AppRopio.Models.Information.Responses;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Commands;

namespace AppRopio.Base.Information.Core.ViewModels.Information
{
    public class InformationViewModel : BaseViewModel, IInformationViewModel
    {
		protected MvxObservableCollection<Article> _articles;

		public MvxObservableCollection<Article> Articles
		{
            get { return _articles; }
			protected set
			{
                SetProperty(ref _articles, value);
			}
		}

		#region Services

		protected IInformationVmService VmService { get { return Mvx.IoCProvider.Resolve<IInformationVmService>(); } }

		#endregion

		#region Commands

		private IMvxCommand _selectionChangedCommand;

		public IMvxCommand SelectionChangedCommand
		{
			get
			{
				return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<Article>(OnArticleSelected));
			}
		}

		#endregion


		#region Init

		public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<BaseBundle>();
			this.InitFromBundle(navigationBundle);
		}

		protected virtual void InitFromBundle(BaseBundle parameters)
		{
			VmNavigationType = parameters.NavigationType == NavigationType.None ?
															NavigationType.ClearAndPush :
															parameters.NavigationType;
		}

		#endregion

		public override Task Initialize()
		{
            return LoadContent();
		}

		protected virtual async Task LoadContent()
		{
            Loading = true;

			var dataSource = await VmService.LoadArticles();

			InvokeOnMainThread(() => Articles = dataSource);

            Loading = false;
		}

        protected virtual async void OnArticleSelected(Article article)
        {
            Loading = true;

            await VmService.HandleSelection(article);

            Loading = false;
        }
	}
}