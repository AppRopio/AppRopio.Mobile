using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Models;

namespace AppRopio.Base.API.Services
{
    public interface IConnectionService : IDisposable
    {
        Dictionary<string, string> Headers { get; }

        Task<RequestResult> GET(string url, CancellationToken? token = null, Action<HttpRequestMessage> requestTuneAction = null);
        Task<RequestResult> POST(string url, HttpContent postData, CancellationToken? token = null, Action<HttpRequestMessage> requestTuneAction = null);
        Task<RequestResult> PUT(string url, HttpContent postData, CancellationToken? token = null, Action<HttpRequestMessage> requestTuneAction = null);
        Task<RequestResult> DELETE(string url, CancellationToken? token = null, HttpContent postData = null, Action<HttpRequestMessage> requestTuneAction = null);
    }
}
