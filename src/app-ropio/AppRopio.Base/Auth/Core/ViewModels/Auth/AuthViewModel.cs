using System.Windows.Input;
using AppRopio.Base.Auth.Core.Models.OAuth;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Auth.Core.ViewModels.Auth.Services;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Base.Auth.Core.ViewModels.Auth
{
    public class AuthViewModel : BaseViewModel, IAuthViewModel
	{
		#region Fields

		protected IAuthNavigationVmService NavigationVmService { get { return Mvx.Resolve<IAuthNavigationVmService>(); } }

		#endregion

		#region Commands

		private ICommand _navigateToSignInCommand;
		public ICommand NavigateToSignInCommand
		{
			get
			{
				return _navigateToSignInCommand ?? (_navigateToSignInCommand = new MvxCommand(NavigateToLoginExecute));
			}
		}

		private ICommand _navigateToSignUpCommand;
		public ICommand NavigateToSignUpCommand
		{
			get
			{
				return _navigateToSignUpCommand ?? (_navigateToSignUpCommand = new MvxCommand(NavigateToRegistrationExecute));
			}
		}

		private ICommand _skipAuthCommand;
		public ICommand SkipAuthCommand
		{
			get
			{
				return _skipAuthCommand ?? (_skipAuthCommand = new MvxCommand(SkipAuthExecute));
			}
		}

		private ICommand _vkCommand;
		public ICommand VkCommand
		{
			get
			{
				return _vkCommand ?? (_vkCommand = new MvxCommand(VkExecute));
			}
		}

		private ICommand _facebookCommand;
		public ICommand FacebookCommand
		{
			get
			{
				return _facebookCommand ?? (_facebookCommand = new MvxCommand(FacebookExecute));
			}
		}


		#endregion

		#region Services

		protected IAuthVmService SocialService { get { return Mvx.Resolve<IAuthVmService>(); } }

		#endregion

		#region Constructor

		public AuthViewModel()
		{
			VmNavigationType = NavigationType.ClearAndPush;
		}

		#endregion

		#region Protected

		protected virtual async void VkExecute()
		{
			Loading = true;

			await SocialService.SignInTo(OAuthType.VK, "Вы отменили процедуру авторизации с помощью VK", "Не удалось авторизоваться с помощью VK", OnUnbindCTS);

			Loading = false;
		}

		protected virtual async void FacebookExecute()
		{
			Loading = true;

			await SocialService.SignInTo(OAuthType.Facebook, "Вы отменили процедуру авторизации с помощью Facebook", "Не удалось авторизоваться с помощью Facebook", OnUnbindCTS);

			Loading = false;
		}

		protected virtual void NavigateToLoginExecute()
		{
			NavigationVmService.NavigateToSignIn(new BaseBundle(NavigationType.Push));
		}

		protected virtual void NavigateToRegistrationExecute()
		{
			NavigationVmService.NavigateToSignUp(new BaseBundle(NavigationType.Push));
		}

		protected virtual void SkipAuthExecute()
		{
			if (VmNavigationType == NavigationType.ClearAndPush || VmNavigationType == NavigationType.DoubleClearAndPush)
                ChangePresentation(new Base.Core.PresentationHints.NavigateToDefaultViewModelHint());
			else
				Close(this);
		}

		public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

			if (parameters.Data.Count>0)
			{
				var navigationBundle = parameters.ReadAs<BaseBundle>();
				this.InitFromBundle(navigationBundle);
			}
		}

		protected virtual void InitFromBundle(BaseBundle parameters)
		{
			VmNavigationType = parameters.NavigationType;
		}

		#endregion

		#region Public

		public override void Unbind()
		{
			base.Unbind();
		}

		#endregion
	}
}
