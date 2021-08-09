using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.Base.Auth.Core.Models.Bundle
{
	public class ResetSmsBundle : BaseBundle
	{
		public string PhoneNumber { get; set; }

		public ResetSmsBundle()
		{

		}

		//public ResetSmsBundle(string phoneNumber, NavigationType navigationType)
		//	: base(new Dictionary<string, string>
		//	{
		//		{ nameof(PhoneNumber), phoneNumber }
		//	})
		//{
		//}

		public ResetSmsBundle(string phoneNumber, NavigationType navigationType)
					: base(navigationType, new Dictionary<string, string>
					{
						{ nameof(PhoneNumber), phoneNumber }
					})
		{
		}

	}
}

