using System;
using System.Windows.Input;
using AppRopio.Base.Auth.Core.ViewModels._base;
using AppRopio.Base.Auth.Core.ViewModels.Password.New.Services;
using AppRopio.Base.Core.PresentationHints;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.New
{
	public class PasswordNewViewModel : AuthBaseViewModel, IPasswordNewViewModel
	{
		#region Commands

		private ICommand _doneCommand;
		public ICommand DoneCommand
		{
			get
			{
				return _doneCommand ?? (_doneCommand = new MvxCommand(OnDoneExecute));
			}
		}

		#endregion

		#region Properties

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

		private string _passwordConfirm;
		public string PasswordConfirm
		{
			get
			{
				return _passwordConfirm;
			}
			set
			{
				_passwordConfirm = value;
				RaisePropertyChanged(() => PasswordConfirm);
				CheckPropertiesData();
			}
		}

		#endregion

		#region Services

		private IPasswordNewVmService _passwordNewVmService;
		protected IPasswordNewVmService PasswordNewVmService { get { return _passwordNewVmService ?? (_passwordNewVmService = Mvx.Resolve<IPasswordNewVmService>()); } }

		#endregion

		#region Protected

		protected override bool IsViewModelPropertiesValid()
		{
			return !Password.IsNullOrEmtpy() && Password == PasswordConfirm;
		}

		protected virtual async void OnDoneExecute()
		{
			if (IsViewModelPropertiesValid())
			{
				Loading = true;
				if (await PasswordNewVmService.SetNewPassword(Password, OnUnbindCTS))
				{
					ChangePresentation(new NavigateToDefaultViewModelHint());
				}
				Loading = false;
			}
		}

		#endregion
	}
}
