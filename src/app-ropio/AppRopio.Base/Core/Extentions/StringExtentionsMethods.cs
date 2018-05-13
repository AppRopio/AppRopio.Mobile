
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System
{
    public static class StringExtentionsMethods
    {
        public static bool IsNullOrEmtpy(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static bool IsMail(this string self)
        {
            return self.Contains("@") && self.Contains(".");
        }

        public static string Rouble(this string str)
        {
            return string.Format("{0} \u20BD", str);
        }

        public static string ToFirstCharUppercase(this string self)
        {
            return $"{char.ToUpperInvariant(self[0])}{self.Substring(1)}";
        }

        public static string ToFirstCharLowercase(this string self)
        {
            return $"{char.ToLowerInvariant(self[0])}{self.Substring(1)}";
        }

        public static string StringPrice(object obj)
        {
            var str = obj.ToString().Replace(" ", "");

            double decValue = 0;

            if (double.TryParse(str, NumberStyles.Any, CultureInfo.CurrentCulture, out decValue) ||
                double.TryParse(str, NumberStyles.Any, CultureInfo.CurrentUICulture, out decValue) ||
                double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out decValue))
            {
                return Math.Abs(decValue - Math.Floor(decValue)) > 0 ?
                           "{0:# ### ##0.#0}" :
                           "{0:# ### ##0}";
            }

            return string.Empty;
        }

        public static string StringPostfixCase(this int val, string one, string two, string five)
        {
            int t = (val % 100 > 20) ? val % 10 : val % 20;

            switch (t)
            {
                case 1: return one;
                case 2: case 3: case 4: return two;
                default: return five;
            }
        }

        public static string StringDate(object obj)
        {
            return string.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
        }

        public static Dictionary<string, string> ToParamsDictionary(this string paramsString)
        {
            return paramsString
                .Split('&')
                .ToDictionary(
                    x => x.Split('=')[0],
                    x => x.Split('=')[1]
                );
        }
    }
}
