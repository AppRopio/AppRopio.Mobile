using System;
namespace AppRopio.Base.Core.Services.Log
{
    public interface ILogService
    {
        byte[] CachedLog();

        void Write(byte[] data);

        byte[] Read();
    }
}
 