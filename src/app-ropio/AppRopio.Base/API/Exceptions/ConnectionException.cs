using System;
using AppRopio.Base.API.Models;
namespace AppRopio.Base.API.Exceptions
{
    public class ConnectionException : Exception
    {
        public RequestResult RequestResult { get; private set; }
        
        public ConnectionException(RequestResult result)
        {
            RequestResult = result;
        }
    }
}
