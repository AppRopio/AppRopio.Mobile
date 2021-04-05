using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.PresentationHints;
using AppRopio.Base.Core.ViewModels;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Auth.Core.ViewModels.Thanks
{
	public class ThanksViewModel : BaseViewModel, IThanksViewModel
	{
		public ICommand StartCommand
		{
			get
			{
				return new MvxAsyncCommand(OnStartCommandExecute);
			}
		}

		#region Services

		protected new IAuthNavigationVmService NavigationVmService { get { return Mvx.IoCProvider.Resolve<IAuthNavigationVmService>(); } }

		#endregion

		public ThanksViewModel()
		{
			VmNavigationType = Base.Core.Models.Navigation.NavigationType.PresentModal;
		}

		#region protected

		public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

			var navigationBundle = parameters.ReadAs<BaseBundle>();
			this.InitFromBundle(navigationBundle);
		}

		protected virtual void InitFromBundle(BaseBundle parameters)
		{
			VmNavigationType = parameters.NavigationType;
		}

		protected virtual async Task OnStartCommandExecute()
		{
			await NavigationVmService.ChangePresentation(new NavigateToDefaultViewModelHint());
		}

		#endregion
	}
}
