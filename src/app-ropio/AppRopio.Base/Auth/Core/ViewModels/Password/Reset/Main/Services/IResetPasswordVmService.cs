using System.Threading;
using System.Threading.Tasks;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main.Services
{
	public interface IResetPasswordVmService
	{
		Task<bool> ForgotPassword(string identifier, CancellationTokenSource cts);
	}
}
