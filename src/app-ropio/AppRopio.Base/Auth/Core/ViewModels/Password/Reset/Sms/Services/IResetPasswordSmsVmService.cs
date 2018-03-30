using System.Threading;
using System.Threading.Tasks;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms.Services
{
	public interface IResetPasswordSmsVmService
	{
		Task<bool> VerifyCode(string code, CancellationTokenSource cts);

		Task ResendCode(CancellationTokenSource cts);
	}
}
