using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Auth.Core.Messages.Session;
using AppRopio.Models.Auth.Responses;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Auth.Core.Services.Implementation
{
	public class SessionService : ISessionService
	{
		#region Fields

		private const string _headerBearerKey = "Bearer";

		private bool _alive;

		private string _token;

		#endregion

		#region Properties

		private User _currentUser;
		private User _CurrentUser
		{
			get
			{
				return _currentUser;
			}
			set
			{
				_alive = value != null;
				_currentUser = value;
			}
		}

		public string Token
		{
			get
			{
				return _token;
			}
		}

		public bool Alive
		{
			get
			{
				return _alive;
			}
		}

		#endregion

		#region Constructor

		public SessionService()
		{
            
		}

        #endregion

        #region Services

        private IMvxMessenger _messagerService;
        protected IMvxMessenger MessagerService => _messagerService ?? (_messagerService = Mvx.IoCProvider.Resolve<IMvxMessenger>());

        private IUserService _userService;
        protected IUserService UserService => _userService ?? (_userService = Mvx.IoCProvider.Resolve<IUserService>());

        private IConnectionService _connectionService;
        protected IConnectionService ConnectionService => _connectionService ?? (_connectionService = Mvx.IoCProvider.Resolve<IConnectionService>());

		#endregion

		#region Private

		private void AddTokenToHeader(string token)
		{
            if (ConnectionService.Headers.ContainsKey(_headerBearerKey))
                ConnectionService.Headers.Remove(_headerBearerKey);
            
            ConnectionService.Headers.Add(_headerBearerKey, token);
		}

		#endregion

		#region Public

		public async Task<bool> ChangeUserData(User user)
		{
			Dictionary<string, object> userFields = new Dictionary<string, object>();

			if (_CurrentUser.Email != user.Email)
				userFields.Add(nameof(_CurrentUser.Email), user.Email);

			if (_CurrentUser.Name != user.Name)
				userFields.Add(nameof(_CurrentUser.Name), user.Name);

			if (_CurrentUser.Phone != user.Phone)
				userFields.Add(nameof(_CurrentUser.Phone), user.Phone);

			if (_CurrentUser.PhotoUrl != user.PhotoUrl)
				userFields.Add(nameof(_CurrentUser.PhotoUrl), user.PhotoUrl);

			if (_CurrentUser.Surname != user.Surname)
				userFields.Add(nameof(_CurrentUser.Surname), user.Surname);

			try
			{
				await UserService.ChangeUserData(userFields);
				_CurrentUser = user;
				MessagerService.Publish<UserInfoChangedMessage>(new UserInfoChangedMessage(this, user));
				return true;
			}

			catch (Exception ex)
			{

			}
			return false;
		}

		public User GetUser()
		{
			return _CurrentUser;
		}

		public void Finish()
		{
			_CurrentUser = null;
		}

		public async Task<bool> StartByToken(string token)
		{
			try
			{
				_token = token;

				var result = await UpdateUserData();
				if (!result)
				{
					_token = null;
					return false;
				}

				AddTokenToHeader(token);

				MessagerService.Publish(new UserTypeChangedMessage(this, Models.UserType.UserType.Registered));

			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> UpdateUserData()
		{
			try
			{
				var user = await UserService.GetUserBy(_token);
				_CurrentUser = user;
				MessagerService.Publish<UserInfoChangedMessage>(new UserInfoChangedMessage(this, user));
			}

			catch (Exception ex)
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}
