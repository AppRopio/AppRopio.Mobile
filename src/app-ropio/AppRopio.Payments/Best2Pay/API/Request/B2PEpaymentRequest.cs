using System.Collections.Generic;
using AppRopio.Payments.Best2Pay.API.Extentions;

namespace AppRopio.Payments.Best2Pay.API
{
    public class B2PEpaymentRequest : IB2PRequest
    {
        public int ID = -1;
        public string firstname;
        public string lastname;
        public string email;
        public bool wm;
        public bool ym;
        public bool qiwi;

        public string RequestString(int sector, string password, string forwardURL)
        {
            string signature = "" + sector + ID + password;
            signature = Parameters.SIGN(signature);

            string data = "";
            data += "&sector=" + sector;
            if (ID != -1) data += "&id=" + ID;
            if (!string.IsNullOrEmpty(firstname) && firstname.Length != 0) data += "&firstname=" + firstname;
            if (!string.IsNullOrEmpty(lastname) && lastname.Length != 0) data += "&lastname=" + lastname;
            if (email.Length != 0) data += "&email=" + email;
            data += "&wm=" + (wm ? "1" : "0");
            data += "&ym=" + (ym ? "1" : "0");
            data += "&qiwi=" + (qiwi ? "1" : "0");
            data += "&signature=" + signature;

            return data.Substring(1);
        }

        public Dictionary<string, string> RequestData(int sector, string password, string forwardURL)
        {
            string signature = "" + sector + ID + password;
            signature = Parameters.SIGN(signature);
    
            var data = new Dictionary<string, string>();

            data.Add("sector", sector.ToString());

            if (ID != -1) data.Add("id", ID.ToString());


            if (!string.IsNullOrEmpty(firstname) && firstname.Length != 0) data.Add("firstname", firstname);

            if (!string.IsNullOrEmpty(lastname) && lastname.Length != 0) data.Add("lastname", lastname);

            if (email.Length != 0) data.Add("email", email);

            data.Add("wm", (wm ? "1" : "0"));

            data.Add("ym", (ym ? "1" : "0"));

            data.Add("qiwi", (qiwi ? "1" : "0"));

            data.Add("signature", signature);
    
            return data;
        }

        public string Path()
        {
            return "/webapi/Epayment";
        }

        public IB2PResponse ResponseForRequest()
        {
            return new B2POperationResponse();
        }
    }
}
