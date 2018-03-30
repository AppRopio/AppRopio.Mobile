using System;
namespace AppRopio.Analytics.Firebase.Core.Services
{
    public interface IFirebaseService
    {
        void TrackScreen(string screenName);

        void TrackEvent(string category, string action, string label, object model);

        void TrackECommerce(decimal fullPrice, string orderId, string currency);

        void TrackException(string message, bool isFatal);
   }
}
