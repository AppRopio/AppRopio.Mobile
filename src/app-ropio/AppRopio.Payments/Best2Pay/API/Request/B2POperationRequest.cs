using System.Collections.Generic;
using AppRopio.Payments.Best2Pay.API.Extentions;

namespace AppRopio.Payments.Best2Pay.API
{
    public class B2POperationRequest : IB2PRequest
    {
        public int ID = -1;
        public int operation = -1;
        public bool get_token = false;

        public string RequestString(int sector, string password, string forwardURL)
        {
            string signature = "" + sector + ID + operation + password;
            signature = Parameters.SIGN(signature);

            string data = "";
            data += "&sector=" + sector;
            if (ID != -1) data += "&id=" + ID;
            if (operation != -1) data += "&operation=" + operation;
            data += "&get_token=" + ((get_token) ? 1 : 0);
            if (signature.Length != 0) data += "&signature=" + signature;

            return data.Substring(1);
        }

        public Dictionary<string, string> RequestData(int sector, string password, string forwardURL)
        {
            string signature = "" + sector + ID + operation + password;
            signature = Parameters.SIGN(signature);

            var data = new Dictionary<string, string>();

            data.Add("sector", sector.ToString());

            if (ID != -1) data.Add("id", ID.ToString());

            if (operation != -1) data.Add("operation", operation.ToString());

            data.Add("get_token", ((get_token) ? "1" : "0"));

            if (signature.Length != 0) data.Add("signature", signature);

            return data;
        }

        public string Path()
        {
            return "/webapi/Operation";
        }

        public IB2PResponse ResponseForRequest()
        {
            return new B2POperationResponse();
        }
    }
}
