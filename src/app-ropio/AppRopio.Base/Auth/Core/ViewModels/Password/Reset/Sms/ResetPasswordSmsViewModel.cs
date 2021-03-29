using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Auth.Core.Formatters;
using AppRopio.Base.Auth.Core.Models.Bundle;
using AppRopio.Base.Auth.Core.ViewModels._base;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms.Services;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms
{
    public class ResetPasswordSmsViewModel : AuthBaseViewModel, IResetPasswordSmsViewModel
	{
		#region Fields

		private string _phoneNumber;

		#endregion

		#region Commands

		private ICommand _validateCodeCmd;
		public ICommand ValidateCodeCmd
		{
			get
			{
				return _validateCodeCmd ?? (_validateCodeCmd = new MvxAsyncCommand(OnValidateCodeExecute));
			}
		}

		private ICommand _resendCodeCmd;
		public ICommand ResendCodeCmd
		{
			get
			{
				return _resendCodeCmd ?? (_resendCodeCmd = new MvxAsyncCommand(OnResendExecute));
			}
		}

		#endregion

		#region Properties

		private string _verifyCode;
		public string VerifyCode
		{
			get
			{
				return _verifyCode;
			}
			set
			{
				_verifyCode = value;
				RaisePropertyChanged(() => VerifyCode);
				CheckPropertiesData();
			}
		}

		public string DescriptionWithPhone
		{
			get
			{
				return GetDescriptionWithPhone();
			}

		}

		#endregion

		#region Services

		private IResetPasswordSmsVmService _resetSmsVmService;
		protected IResetPasswordSmsVmService ResetSmsVmService { get { return _resetSmsVmService ?? (_resetSmsVmService = Mvx.IoCProvider.Resolve<IResetPasswordSmsVmService>()); } }

		#endregion

		#region Constructor

		public ResetPasswordSmsViewModel()
		{
			VmNavigationType = Base.Core.Models.Navigation.NavigationType.Push;
		}

		#endregion

		#region Private

		private string GetDescriptionWithPhone()
		{
			string phone = null;
			if (!_phoneNumber.IsNullOrEmtpy())
			{
				if (_phoneNumber.Length > 10)
				{
					var cleanPhone = PhoneNumberFormatter.GetCleanStrWithoutFormat(_phoneNumber, false);
					phone = $"+7 {cleanPhone.Substring(1, 3)}...{cleanPhone.Substring(cleanPhone.Length - 2, 2)}";
				}
				else
					phone = _phoneNumber;
			}
			if (phone != null)
                return $"{LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_CodeFromSms")}\n{LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_SentToPhone")} {phone}";
			else
                return LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_CodeFromSms");
		}

		#endregion

		#region Protected

		protected override bool IsViewModelPropertiesValid()
		{
			return !VerifyCode.IsNullOrEmtpy();
		}

		protected virtual async Task OnValidateCodeExecute()
		{
			Loading = true;
			if (await ResetSmsVmService.VerifyCode(VerifyCode, OnUnbindCTS))
			{
				NavigationVmService.NavigateToNewPassword(new BaseBundle(NavigationType.Push));
			}

			Loading = false;

		}

		protected virtual async Task OnResendExecute()
		{
			Loading = true;
			await ResetSmsVmService.ResendCode(OnUnbindCTS);
			Loading = false;
		}

		public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

			var resetSmsBundle = parameters.ReadAs<ResetSmsBundle>();
			this.InitFromBundle(resetSmsBundle);
		}

		protected virtual void InitFromBundle(ResetSmsBundle parameters)
		{
			VmNavigationType = parameters.NavigationType;
			_phoneNumber = parameters.PhoneNumber;
		}

		#endregion

		#region Public


		#endregion
	}
}
