using System;
using System.Windows.Input;
using AppRopio.Base.Auth.Core.Formatters;
using AppRopio.Base.Auth.Core.ViewModels._base;
using AppRopio.Base.Auth.Core.ViewModels.SignIn.Services;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.PresentationHints;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Base.Auth.Core.ViewModels.SignIn
{
	public class SignInViewModel : AuthBaseViewModel, ISignInViewModel
	{
		#region Commands

		private ICommand _recoveryPassCommand;
		public ICommand RecoveryPassCommand
		{
			get
			{
				return _recoveryPassCommand ?? (_recoveryPassCommand = new MvxCommand(OnRecoveryPassExecute));
			}
		}

		private ICommand _signInCommand;
		public ICommand SignInCommand
		{
			get
			{
				return _signInCommand ?? (_signInCommand = new MvxCommand(SignInExecute));
			}
		}

		private ICommand _skipCommand;
		public ICommand SkipCommand
		{
			get
			{
				return _skipCommand ?? (_skipCommand = new MvxCommand(SkipExecute));
			}
		}

		#endregion

		#region Properties

		private bool _dataValid;
		public bool DataValid
		{
			get
			{
				return _dataValid;
			}
			set
			{
				_dataValid = value;
				RaisePropertyChanged(() => DataValid);
			}
		}

		private string _identity;
		public string Identity
		{
			get
			{
				return _identity;
			}
			set
			{
				_identity = value;
				RaisePropertyChanged(() => Identity);
				CheckPropertiesData();
			}
		}

		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				RaisePropertyChanged(() => Password);
				CheckPropertiesData();
			}
		}

		#endregion

		#region Services

		private ISignInVmService _signInService;
		protected ISignInVmService SignInService { get { return _signInService ?? (_signInService = Mvx.Resolve<ISignInVmService>()); } }

		#endregion

		#region Constructor

		public SignInViewModel()
		{
			VmNavigationType = Base.Core.Models.Navigation.NavigationType.Push;
		}

		#endregion

		#region Protected

		protected override bool IsViewModelPropertiesValid()
		{
			return !Password.IsNullOrEmtpy()
							&&
							  (
                                (!Identity.IsNullOrEmpty() && Identity.IsMail() && Config.IdentifyUserByEmail)
								  || (PhoneNumberFormatter.IsValid(Identity) && !Config.IdentifyUserByEmail)
							  );
		}

		protected async virtual void SignInExecute()
		{
			Loading = true;

			if (await SignInService.SignIn(Identity, Password, OnUnbindCTS))
				InvokeOnMainThread(() => ChangePresentation(new NavigateToDefaultViewModelHint()));

			Loading = false;
		}

		protected virtual void SkipExecute()
		{
			Close(this);
		}

		protected virtual void OnRecoveryPassExecute()
		{
			NavigationVmService.NavigateToResetPassword(new BaseBundle(NavigationType.Push));
		}

		#endregion

	}
}
