using System;

namespace AppRopio.Base.API.Models
{
    public enum RequestCancelReason
    {
        None = 0,

        ConnectionFailed = 1,
        Timeout = 2,
        Manual = 3
    }
}

