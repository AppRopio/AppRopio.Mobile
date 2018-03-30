using AppRopio.Payments.Best2Pay.Core.Models;

namespace AppRopio.Payments.Best2Pay.Core.Services
{
    public interface IBest2PayConfigService
	{
        Best2PayConfig Config { get; }
	}
}