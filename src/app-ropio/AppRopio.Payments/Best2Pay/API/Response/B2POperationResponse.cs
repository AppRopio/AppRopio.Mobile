using System.Collections.Generic;
using AppRopio.Payments.Best2Pay.API.Extentions;

namespace AppRopio.Payments.Best2Pay.API
{
    public class B2POperationResponse : IB2PResponse
    {
        public int order_id = 0;
        public string order_state = "";
        public string reference = "";
        public int ID = 0;
        public string date = "";
        public string type = "";
        public string state = "";
        public int reason_code = 0;
        public string message = "";
        public string name = "";
        public string pan = "";
        public string email = "";
        public int amount = 0;
        public int currency = 0;
        public string approval_code = "";
        public string token = "";

        public void fillFromDictionary(Dictionary<string, string> dictionary)
        {
            ID = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "operation"));
            if (ID <= 0)
            {
                ID = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "id"));
                order_id = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "order_id"));
            }
            else order_id = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "id"));

            order_state = Parameters.stringFromParsedDictionary(dictionary, "order_state");
            reference = Parameters.stringFromParsedDictionary(dictionary, "reference");
            date = Parameters.stringFromParsedDictionary(dictionary, "date");
            type = Parameters.stringFromParsedDictionary(dictionary, "type");
            state = Parameters.stringFromParsedDictionary(dictionary, "state");
            reason_code = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "reason_code"));
            message = Parameters.stringFromParsedDictionary(dictionary, "message");
            name = Parameters.stringFromParsedDictionary(dictionary, "name");
            pan = Parameters.stringFromParsedDictionary(dictionary, "pan");
            email = Parameters.stringFromParsedDictionary(dictionary, "email");
            amount = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "amount"));
            currency = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "currency"));
            approval_code = Parameters.stringFromParsedDictionary(dictionary, "approval_code");
            token = Parameters.stringFromParsedDictionary(dictionary, "token");
        }
    }
}
