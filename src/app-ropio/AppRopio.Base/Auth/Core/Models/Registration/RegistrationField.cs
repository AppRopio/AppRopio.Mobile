using System;
using AppRopio.Models.Auth.Enums;
using System.Collections.Generic;
namespace AppRopio.Base.Auth.Core.Models.Registration
{
	public class RegistrationField
	{
		public string Id { get; set; }

		public RegistrationFieldType Type { get; set; }

		public bool Required { get; set; } = true;

		public DateTime? MinDate { get; set; }

		public DateTime? MaxDate { get; set; }		
	}
}
