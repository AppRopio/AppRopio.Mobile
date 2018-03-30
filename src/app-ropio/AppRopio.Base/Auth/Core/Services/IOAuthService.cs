using System.Threading.Tasks;
using AppRopio.Base.Auth.Core.Models.OAuth;

namespace AppRopio.Base.Auth.Core.Services
{
	public interface IOAuthService
    {
        Task<string> SignInTo(OAuthType socialType);
    }
}
