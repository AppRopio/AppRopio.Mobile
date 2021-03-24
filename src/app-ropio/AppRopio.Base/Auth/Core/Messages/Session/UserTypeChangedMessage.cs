using AppRopio.Base.Auth.Core.Models.UserType;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Auth.Core.Messages.Session
{
	public class UserTypeChangedMessage: MvxMessage
	{
		public UserType UserType { get; private set; }

		public UserTypeChangedMessage(object sender, UserType userType) : base(sender)
		{
			this.UserType = userType;
		}
	}
}
