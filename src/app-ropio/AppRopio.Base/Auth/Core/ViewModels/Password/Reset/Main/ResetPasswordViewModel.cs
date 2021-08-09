using System;
using System.Windows.Input;
using AppRopio.Base.Auth.Core.Formatters;
using AppRopio.Base.Auth.Core.Models.Bundle;
using AppRopio.Base.Auth.Core.ViewModels._base;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main.Services;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Commands;
using System.Threading.Tasks;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main
{
	public class ResetPasswordViewModel : AuthBaseViewModel, IResetPasswordViewModel
	{
		#region Commands

		private ICommand _forgotCmd;
		public ICommand ForgotCmd
		{
			get
			{
				return _forgotCmd ?? (_forgotCmd = new MvxAsyncCommand(OnForgotCmdExecute));
			}
		}

		private ICommand _closeCmd;
		public ICommand CloseCmd
		{
			get
			{
				return _closeCmd ?? (_closeCmd = new MvxAsyncCommand(OnCloseExecute));
			}
		}

		#endregion

		#region Properties

		private bool _successViewVisible;
		public bool SuccessViewVisible
		{
			get
			{
				return _successViewVisible;
			}
			set
			{
				if (value != _successViewVisible)
				{
					_successViewVisible = value;
					RaisePropertyChanged(() => SuccessViewVisible);
				}
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

		#endregion

		#region Services

		private IResetPasswordVmService _resetMainVmService;
		protected IResetPasswordVmService ResetMainVmService { get { return _resetMainVmService ?? (_resetMainVmService = Mvx.IoCProvider.Resolve<IResetPasswordVmService>()); } }

		#endregion

		#region Constructor

		public ResetPasswordViewModel()
		{
			_successViewVisible = false;
		}

		#endregion

		#region Protected

		protected override bool IsViewModelPropertiesValid()
		{
			return (Config.IdentifyUserByEmail && Identity.IsMail())
						 || (!Config.IdentifyUserByEmail && PhoneNumberFormatter.IsValid(Identity));
		}

		protected virtual async Task OnCloseExecute()
		{
			await NavigationVmService.Close(this);
		}

		protected virtual async Task OnForgotCmdExecute()
		{
			if (IsViewModelPropertiesValid())
			{
				Loading = true;

				if (await ResetMainVmService.ForgotPassword(Identity, OnUnbindCTS))
				{
					if (Config.IdentifyUserByEmail)
						SuccessViewVisible = true;
					else
					{
						var bundle = new ResetSmsBundle(Identity, Base.Core.Models.Navigation.NavigationType.Push);
						NavigationVmService.NavigateToResetSms(bundle);
					}
				}
				Loading = false;
			}
		}

		#endregion

	}
}
