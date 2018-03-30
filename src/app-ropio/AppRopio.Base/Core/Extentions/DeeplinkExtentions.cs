using System;
using System.Collections.Generic;
using System.Linq;

namespace AppRopio.Base.Core.Extentions
{
    public static class DeeplinkExtentions
    {
        public static Tuple<string, List<string>, Dictionary<string, string>> ParseDeeplink(this string deeplink)
        {
            deeplink = deeplink
                .Replace("%3A", ":")
                .Replace("%2F", "/")
                .Replace("%3D", "=")
                .Replace("%26", "&");

            var urlScheme = deeplink.Substring(0, deeplink.IndexOf("://", StringComparison.Ordinal));

            var deeplinkParamsAndRouting = deeplink.Substring(deeplink.IndexOf("://", StringComparison.Ordinal) + 3);

            var routing = deeplinkParamsAndRouting.Contains("/") ? deeplinkParamsAndRouting.Split('/').ToList() : new List<string>();

            var paramsString = routing.IsNullOrEmpty() ?
                                      deeplinkParamsAndRouting :
                                      routing.Last();

            var urlParameters = paramsString.ToParamsDictionary();

            return new Tuple<string, List<string>, Dictionary<string, string>>(urlScheme, routing, urlParameters);
        }

        public static void ParseDeeplink(this string deeplink, out string urlScheme, out List<string> routing, out Dictionary<string, string> urlParameters)
        {
            deeplink = deeplink
                .Replace("%3A", ":")
                .Replace("%2F", "/")
                .Replace("%3D", "=")
                .Replace("%26", "&");

            urlScheme = deeplink.Substring(0, deeplink.IndexOf("://", StringComparison.Ordinal));

            var deeplinkParamsAndRouting = deeplink.Substring(deeplink.IndexOf("://", StringComparison.Ordinal) + 3);

            routing = deeplinkParamsAndRouting.Contains("/") ? deeplinkParamsAndRouting.Split('/').ToList() : new List<string>();

            var paramsString = routing.IsNullOrEmpty() ?
                                      deeplinkParamsAndRouting :
                                      routing.Last();

            if (!routing.IsNullOrEmpty())
                routing.RemoveAt(routing.Count - 1);

            urlParameters = paramsString.ToParamsDictionary();
        }
    }
}
