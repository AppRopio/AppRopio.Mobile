using System.Threading;
using System.Threading.Tasks;

namespace AppRopio.Base.Auth.Core.ViewModels.SignIn.Services
{
	public interface ISignInVmService
	{
		Task<bool> SignIn(string identifier, string password, CancellationTokenSource cts);
	}
}
