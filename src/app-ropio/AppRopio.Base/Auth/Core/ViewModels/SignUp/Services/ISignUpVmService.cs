using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using AppRopio.Models.Auth.Responses;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp.Services
{
	public interface ISignUpVmService
	{
		Task<RegistrationResponse> SignUp(List<ISignUpItemBaseViewModel> fields, CancellationTokenSource cts);
	}
}
