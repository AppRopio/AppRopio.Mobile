using System.Collections.Generic;
using AppRopio.Payments.Best2Pay.API.Extentions;

namespace AppRopio.Payments.Best2Pay.API
{
    public class B2PRegisterRequest : IB2PRequest
    {
        public int amount = -1;
        public int currency = -1;
        public string reference = @"";
        public string description = @"";
        public string email = @"";
        public string phone = @"";
        public string bank_name = @"";
        public string address = @"";
        public string city = @"";
        public string region = @"";
        public string post_code = @"";
        public string country = @"";
        public int recurring_period = -1;
        public int error_period = -1;
        public int error_number = -1;
        public string deviceID = @"";

        public string RequestString(int sector, string password, string forwardURL)
        {
            string signature = "" + sector + amount + currency + password;
            signature = Parameters.SIGN(signature);

            string data = "";
            if (amount != -1) data += "&amount=" + amount;
            if (currency != -1) data += "&currency=" + currency;
            if (reference.Length != 0) data += "&reference=" + reference;
            if (description.Length != 0) data += "&description=" + description;
            data += "&sector=" + sector;
            data += "&url=" + forwardURL;
            if (!string.IsNullOrEmpty(email) && email.Length != 0) data += "&email=" + email;
            if (!string.IsNullOrEmpty(phone) && phone.Length != 0) data += "&phone=" + phone;
            if (signature.Length != 0) data += "&signature=" + signature;
            if (bank_name.Length != 0) data += "&bank_name=" + bank_name;
            if (address.Length != 0) data += "&address=" + address;
            if (city.Length != 0) data += "&city=" + city;
            if (region.Length != 0) data += "&region=" + region;
            if (post_code.Length != 0) data += "&post_code=" + post_code;
            if (country.Length != 0) data += "&country=" + country;
            if (recurring_period != -1) data += "&recurring_period=" + recurring_period;
            if (error_period != -1) data += "&error_period=" + error_period;
            if (error_number != -1) data += "&error_number=" + error_number;

            if (deviceID.Length != 0) data += "&device_id=" + deviceID;

            string dataString = data.Substring(1);
            return dataString;
        }

        public Dictionary<string, string> RequestData(int sector, string password, string forwardURL)
        {
            string signature = "" + sector + amount + currency + password;
            signature = Parameters.SIGN(signature);

            var data = new Dictionary<string, string>();

            if (amount != -1) data.Add("amount", amount.ToString());

            if (currency != -1) data.Add("currency", currency.ToString());

            if (reference.Length != 0) data.Add("reference", reference);

            if (description.Length != 0) data.Add("description", description);

            data.Add("sector", sector.ToString());
            data.Add("url", forwardURL);

            if (!string.IsNullOrEmpty(email) && email.Length != 0) data.Add("email", email);

            if (!string.IsNullOrEmpty(phone) && phone.Length != 0) data.Add("phone", phone);

            if (signature.Length != 0) data.Add("signature", signature);

            if (bank_name.Length != 0) data.Add("bank_name", bank_name);

            if (address.Length != 0) data.Add("address", address);

            if (city.Length != 0) data.Add("city", city);

            if (region.Length != 0) data.Add("region", region);

            if (post_code.Length != 0) data.Add("post_code", post_code);

            if (country.Length != 0) data.Add("country", country);

            if (recurring_period != -1) data.Add("recurring_period", recurring_period.ToString());

            if (error_period != -1) data.Add("error_period", error_period.ToString());

            if (error_number != -1) data.Add("error_number", error_number.ToString());

            if (deviceID.Length != 0) data.Add("device_id", deviceID);

            return data;
        }

        public string Path()
        {
            return "/webapi/Register";
        }

        public IB2PResponse ResponseForRequest()
        {
            return new B2PRegisterResponse();
        }
    }
}

