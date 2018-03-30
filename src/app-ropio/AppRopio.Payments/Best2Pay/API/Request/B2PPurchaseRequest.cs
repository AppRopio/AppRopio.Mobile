using System.Collections.Generic;
using AppRopio.Payments.Best2Pay.API.Extentions;

namespace AppRopio.Payments.Best2Pay.API
{
    public class B2PPurchaseRequest : IB2PRequest
    {
        public int ID = -1;
        public bool get_token = false;

        public string RequestString(int sector, string password, string forwardURL)
        {
            string signature = "" + sector + ID + password;
            signature = Parameters.SIGN(signature);

            string data = "";
            if (ID != -1) data += "&id=" + ID;
            if (sector != -1) data += "&sector=" + sector;
            data += "&get_token=" + (get_token ? "1" : "0");
            if (signature.Length != 0) data += "&signature=" + signature;

            return data.Substring(1);
        }

        public Dictionary<string, string> RequestData(int sector, string password, string forwardURL)
        {
            string signature = "" + sector + ID + password;
            signature = Parameters.SIGN(signature);
    
            var data = new Dictionary<string, string>();

            if (ID != -1) data.Add("id", ID.ToString());

            if (sector != -1) data.Add("sector", sector.ToString());

            data.Add("get_token", (get_token ? "1" : "0"));

            if (signature.Length != 0) data.Add("signature", signature);
    
            return data;
        }

        public string Path()
        {
            return "/webapi/Purchase";
        }

        public IB2PResponse ResponseForRequest()
        {
            return new B2POperationResponse();
        }

    }
}
