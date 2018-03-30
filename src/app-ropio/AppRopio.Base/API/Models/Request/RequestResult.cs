using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace AppRopio.Base.API.Models
{
    public class RequestResult
    {
        public string AbsoluteUrl { get; internal set; }

        public bool Succeeded { get; internal set; }

        public Exception Exception { get; internal set; }

        public bool RequestCanceled { get; internal set; }

        public RequestCancelReason RequestCancelReason { get; internal set; }

        public HttpStatusCode ResponseCode { get; internal set; }

        public HttpContent ResponseContent { get; internal set; }

        public Dictionary<string, IEnumerable<string>> ResponseHeaders { get; internal set; }

		public Dictionary<string, IEnumerable<string>> Headers { get; internal set; }
    }
}
