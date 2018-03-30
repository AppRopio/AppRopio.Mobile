using System.Collections.Generic;
using AppRopio.Payments.Best2Pay.API.Extentions;

namespace AppRopio.Payments.Best2Pay.API
{
    public class B2PRegisterResponse : IB2PResponse
    {
        public int ID;
        public string state;
        public bool inprogress;
        public string date;
        public int amount;
        public int currency;
        public string email;
        public string phone;
        public string reference;
        public string description;
        public string url;

        public void fillFromDictionary(Dictionary<string, string> dictionary)
        {
            amount = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "amount"));
            currency = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "currency"));
            date = Parameters.stringFromParsedDictionary(dictionary, "date");
            description = Parameters.stringFromParsedDictionary(dictionary, "description");
            email = Parameters.stringFromParsedDictionary(dictionary, "email");
            ID = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dictionary, "id"));
            inprogress = !Parameters.stringFromParsedDictionary(dictionary, "inprogress").Contains("0");
            phone = Parameters.stringFromParsedDictionary(dictionary, "phone");
            reference = Parameters.stringFromParsedDictionary(dictionary, "reference");
            state = Parameters.stringFromParsedDictionary(dictionary, "state");
            url = Parameters.stringFromParsedDictionary(dictionary, "url");
        }
    }
}

