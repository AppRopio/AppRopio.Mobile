using System;
using System.Threading;
using System.Threading.Tasks;
namespace AppRopio.Base.API.Services
{
    public interface IPushService
    {
        Task RegisterDevice(string pushToken, CancellationToken cancellationToken);
    }
}
