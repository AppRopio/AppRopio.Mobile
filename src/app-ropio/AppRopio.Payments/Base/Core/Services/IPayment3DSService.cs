using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppRopio.Payments.Core.Services
{
	public interface IPayment3DSService
	{
        void SetWebView(object webView);
        
		Task<Dictionary<string, string>> Process3DS(string url, string redirectUrl, Dictionary<string, string> parameters);
	}
}