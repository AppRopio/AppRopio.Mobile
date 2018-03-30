using System;
using System.Collections.Generic;

namespace AppRopio.Payments.Best2Pay.API
{
    public interface IB2PRequest
    {
        string RequestString(int sector, string password, string forwardURL);

        Dictionary<string, string> RequestData(int sector, string password, string forwardURL);

        string Path();

        IB2PResponse ResponseForRequest();
    }
}

