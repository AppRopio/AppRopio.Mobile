using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Models.Base.Requests;

namespace AppRopio.Base.API.Services.Implementations
{
    public class PushService : BaseService, IPushService
    {
        protected string URL = "push/register";
        
        #region IPushService implementation

        public async Task RegisterDevice(string pushToken, CancellationToken cancellationToken)
        {
            await Post(URL, ToStringContent(new PushRegisterRequest { PushToken = pushToken }), cancellationToken: cancellationToken);
        }

        #endregion
    }
}
