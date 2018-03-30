using System.Threading;
using System.Threading.Tasks;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.New.Services
{
	public interface IPasswordNewVmService
	{
		Task<bool> SetNewPassword(string password, CancellationTokenSource cts);
	}
}
