using System.Threading.Tasks;
using AppRopio.Models.Auth.Responses;

namespace AppRopio.Base.Auth.Core.Services
{
	public interface ISessionService
	{
		bool Alive { get; }

		Task<bool> StartByToken(string token);

		User GetUser();

		Task<bool> UpdateUserData();

		Task<bool> ChangeUserData(User user);

		string Token { get; }

		void Finish();
	}
}
