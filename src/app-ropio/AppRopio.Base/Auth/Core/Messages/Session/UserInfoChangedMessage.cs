using AppRopio.Models.Auth.Responses;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Auth.Core.Messages.Session
{
	public class UserInfoChangedMessage : MvxMessage
	{
		public User User { get; private set; }

		public UserInfoChangedMessage(object sender, User user) : base(sender)
		{
			this.User = user;
		}
	}
}
