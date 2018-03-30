using System;
using AppRopio.ECommerce.Marked.Core.Models;

namespace AppRopio.ECommerce.Marked.Core.Services
{
	public interface IMarkedConfigService
	{
		 MarkedConfig Config { get; }
	}
}