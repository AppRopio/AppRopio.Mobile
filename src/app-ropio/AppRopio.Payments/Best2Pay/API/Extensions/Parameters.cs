using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using MvvmCross.Platform;
using System.Net.Http;
using AppRopio.Base.API.Services;
using System.Xml.Serialization;

namespace AppRopio.Payments.Best2Pay.API.Extentions
{
    public static class Parameters
    {
        private static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<Stream>();
            request.BeginGetRequestStream(ar =>
            {
                Stream requestStream = request.EndGetRequestStream(ar);
                taskComplete.TrySetResult(requestStream);
            }, request);
            return taskComplete.Task;
        }

        private static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();
            request.BeginGetResponse(asyncResponse =>
            {
                try
                {
                    HttpWebRequest responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
                    HttpWebResponse someResponse = (HttpWebResponse)responseRequest.EndGetResponse(asyncResponse);
                    taskComplete.TrySetResult(someResponse);
                }
                catch (WebException webExc)
                {
                    HttpWebResponse failedResponse = (HttpWebResponse)webExc.Response;
                    taskComplete.TrySetResult(failedResponse);
                }
            }, request);
            return taskComplete.Task;
        }

        public static async Task<string> POST(string Url, Dictionary<string, string> Data)
        {
            return await Post(Url, new FormUrlEncodedContent(Data));
        }

        public static string SIGN(string unsignedString)
        {
            unsignedString = MD5Core.GetHashString(Encoding.UTF8.GetBytes(unsignedString)).ToLower();
            unsignedString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(unsignedString));
            return unsignedString;
        }

        public static string stringFromParsedDictionary(Dictionary<string, string> dictionary, string key)
        {
            string value;
            return (dictionary.TryGetValue(key, out value)) ? value : "";
        }


        public static bool checkSignature(string signature, string response, string password)
        {
            string modifiedString = Regex.Replace(response, "(<signature>[^>]+</signature>[\r\n]*)|(<[^>]+>[\r\n]*)", "");
            modifiedString += password;
            modifiedString = Parameters.SIGN(modifiedString);

            bool isEqual = (modifiedString == signature);
            return isEqual;
        }

        public static Dictionary<string, string> dictionaryWithQueryString(string queryString)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] pairs = queryString.Split('&');

            for (int i = 0; i < pairs.Length; i++)
            {
                string[] elements = pairs[i].Split('=');
                if (elements.Length == 2)
                {
                    string key = elements[0];
                    string value = elements[1];
                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }

        public static int ConvertToInt(string value)
        {
            int number;
            bool result = Int32.TryParse(value, out number);
            return (result) ? number : 0;
        }

        public static string getRootName(string data)
        {
            return (XDocument.Parse(data)).Root.Name.LocalName;
        }

        public static Dictionary<string, string> dictionaryForXMLString(string data)
        {
            XDocument doc = XDocument.Parse(data);
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

            foreach (XElement element in doc.Root.Elements())
            {
                int keyInt = 0;
                string keyName = element.Name.LocalName;

                while (dataDictionary.ContainsKey(keyName))
                    keyName = element.Name.LocalName + "_" + keyInt++;

                dataDictionary.Add(keyName, element.Value);
            }
            return dataDictionary;
        }

        public static List<string> cutOperationsInArrayFromResponseString(string responseString)
        {
            List<string> ret = new List<string>();

            string tag = "operation";

            while (true)
            {
                int fIncl = responseString.IndexOf("<" + tag + ">");
                int lIncl = responseString.IndexOf("</" + tag + ">") + tag.Length + 3;
                if (fIncl < 0) break;
                string operationString = responseString.Substring(fIncl, (lIncl - fIncl));
                ret.Add(operationString);
                responseString = responseString.Substring(lIncl - 1);
            }

            return ret;
        }

		internal static async Task<string> Post(string url, HttpContent postContent)
		{
			var uri = new Uri($"{url}");

			var httpClient = new HttpClient();

			HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, postContent);
			var response = await responseMessage.Content.ReadAsStringAsync();
            return response;
		}
    }
}

