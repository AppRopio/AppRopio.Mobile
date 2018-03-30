using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Auth.Responses;

namespace AppRopio.Base.Auth.API.Services.Implementation
{
	public class UserService : BaseService, IUserService
	{
		#region Fields

		protected const string USER_URL = "user/";
		protected const string CHANGE_URL = "user/change/";
		protected const string CHANGE_FIELDS_URL = "user/change_fields/";

		#endregion

		#region IUserService implementation

		public async Task ChangeUserData(Dictionary<string, object> fields)
		{
			await Post(CHANGE_URL, ToStringContent(fields));
		}

		public async Task ChangeUsersData(string fieldName, object data)
		{
			await Post(CHANGE_URL, ToStringContent(new KeyValuePair<string, object>(fieldName, data)));
		}

		public async Task<User> GetUserBy(string token)
		{
			return await Post<User>(CHANGE_URL, ToStringContent(new { token }));
		}

		#endregion
	}
}
