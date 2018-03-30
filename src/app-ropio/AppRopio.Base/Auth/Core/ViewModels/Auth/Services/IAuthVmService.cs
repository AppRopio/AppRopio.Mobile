using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.Auth.Core.Models.OAuth;

namespace AppRopio.Base.Auth.Core.ViewModels.Auth.Services
{
	public interface IAuthVmService
	{
		Task SignInTo(OAuthType socialType, string cancelledError, string error, CancellationTokenSource cts);
	}
}
