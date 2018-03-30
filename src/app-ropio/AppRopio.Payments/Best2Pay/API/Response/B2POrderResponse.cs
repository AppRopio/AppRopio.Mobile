using System.Collections.Generic;
using System.Linq;
using AppRopio.Payments.Best2Pay.API.Extentions;

namespace AppRopio.Payments.Best2Pay.API
{
    public class B2POrderResponse : IB2PResponse
    {
        public int order_id = 0;
        public string state = "";
        public bool inprogress = false;
        public string date = "";
        public int amount = 0;
        public int currency = 0;
        public string email = "";
        public string phone = "";
        public string reference = "";
        public string description = "";
        public string url = "";
        public List<B2POperationResponse> operations = new List<B2POperationResponse>();

        public void fillFromDictionary(Dictionary<string, string> dictionary)
        {
            order_id = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "id"));
    
            state = Parameters.stringFromParsedDictionary(dictionary, "state");
            inprogress = !Parameters.stringFromParsedDictionary(dictionary, "inprogress").Contains("0");
            date = Parameters.stringFromParsedDictionary(dictionary, "date");
            amount = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "amount"));
            currency = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "currency"));
            email = Parameters.stringFromParsedDictionary(dictionary, "email");
            phone = Parameters.stringFromParsedDictionary(dictionary, "phone");
            reference = Parameters.stringFromParsedDictionary(dictionary, "reference");
            description = Parameters.stringFromParsedDictionary(dictionary, "description");
            url = Parameters.stringFromParsedDictionary(dictionary, "url");

        }


    }
}
