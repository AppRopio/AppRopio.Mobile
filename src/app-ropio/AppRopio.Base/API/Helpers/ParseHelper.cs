using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using AppRopio.Base.API.Models;

namespace AppRopio.Base.API.Helpers
{
    public static class ParseHelper
    {
        public static async Task<ParseResult<T>> Parse<T>(this RequestResult result, MediaTypeFormat messageTypeFormat, Func<string, ParseResult<T>> customDeserializer = null) where T : class
        {
            if (result.Exception != null)
                return new ParseResult<T> { Successful = false, Error = result.Exception.Message };

            if (result.ResponseCode == HttpStatusCode.NoContent)
                return new ParseResult<T> { Successful = true };

            var contentAsString = await result.ResponseContent.ReadAsStringAsync();

            if (customDeserializer != null)
                return customDeserializer(contentAsString);

            switch (messageTypeFormat)
            {
                case MediaTypeFormat.Json:
                    return TryParseJson<T>(contentAsString);
                case MediaTypeFormat.Xml:
                    return TryParseXml<T>(contentAsString);
                default:
                    return TryParseJson<T>(contentAsString);
            }

        }

        public static ParseResult<T> TryParseJson<T>(string source) where T : class
        {
            try
            {
                return new ParseResult<T>()
                {
                    Successful = true,
                    ParsedObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(source)
                };
            }
            catch (Exception ex)
            {
                return new ParseResult<T>()
                {
                    Successful = false,
                    Error = ex.Message
                };
            }
        }

        public static ParseResult<T> TryParseXml<T>(string source, XmlAttributeOverrides overrides) where T : class
        {
            var serializer = new XmlSerializer(typeof(T), overrides);
            return TryParseXml<T>(source, serializer);
        }

        public static ParseResult<T> TryParseXml<T>(string source, XmlRootAttribute root) where T : class
        {
            var serializer = new XmlSerializer(typeof(T), root);
            return TryParseXml<T>(source, serializer);
        }

        public static ParseResult<T> TryParseXml<T>(string source) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            return TryParseXml<T>(source, serializer);
        }

        public static ParseResult<T> TryParseXml<T>(string source, XmlSerializer serializer) where T : class
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(source)))
                {
                    return new ParseResult<T>()
                    {
                        Successful = true,
                        ParsedObject = (T)serializer.Deserialize(reader)
                    };
                }
            }
            catch (Exception ex)
            {
                return new ParseResult<T>()
                {
                    Successful = false,
                    Error = ex.Message
                };
            }
        }
    }
}
