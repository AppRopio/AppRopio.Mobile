using System;
using System.Collections.Generic;

namespace AppRopio.Payments.Best2Pay.API
{
    public interface IB2PResponse
    {
        void fillFromDictionary(Dictionary<string, string> dictionary);
    }
}

