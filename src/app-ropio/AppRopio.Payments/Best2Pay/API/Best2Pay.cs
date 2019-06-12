using System;
using System.Collections.Generic;
using AppRopio.Payments.Best2Pay.API.Extentions;

namespace AppRopio.Payments.Best2Pay.API
{
    public class Best2Pay
    {
        private string _password;
        private int _sector;

        private string _forwardUrl;
        private string _forwardVerifyURL;

        public bool IsTest { get; set; }

        public Best2Pay(int sector, string password, string forwardUrl, string forwardVerifyUrl, bool test)
        {
            _password = password;
            _sector = sector;
            _forwardUrl = forwardUrl;
            _forwardVerifyURL = forwardVerifyUrl;

#if DEBUG
            IsTest = true;
#else
            IsTest = test;
#endif
        }

        private string Encode(string to)
        {
            string requestString = System.Net.WebUtility.UrlEncode(to);
            requestString = requestString.Replace("%3D", "=");
            requestString = requestString.Replace("%26", "&");
            return requestString;
        }

        private string UrlForRequest()
        {
            return ((this.IsTest) ? @"https://test.best2pay.net" : @"https://pay.best2pay.net");
        }

        private string GetRequest(IB2PRequest request, bool isVerifyCase)
        {
            string requestString = request.RequestString(_sector, _password, _forwardUrl);
            requestString = Encode(requestString);

            return (!isVerifyCase) ? (UrlForRequest() + request.Path() + "?" + requestString) : (request.Path());

            //if (!isVerifyCase)
            //    _webBrowser.LoadRequest(new NSUrlRequest(new NSUrl(url)));//_webBrowser.Navigate(new Uri(url));
            //else
            //{
            //    B2P3DSecureRequest dsrequest = (B2P3DSecureRequest)request;
            //    string navStr = "<html><head><title>Faceless</title><script type=\"text/javascript\">function submitForm() {document.forms[0].submit();}</script></head><body onload=\"submitForm();\"><form method=\"POST\" action=\"";
            //    navStr += dsrequest.ACSUrl;
            //    navStr += "\"><input type=\"hidden\" name=\"PaReq\" value=\"";
            //    navStr += encoder(dsrequest.PaReq);
            //    navStr += "\"><input type=\"hidden\" name=\"TermUrl\" value=\"";
            //    navStr += dsrequest.TermUrl;
            //    navStr += "\"><input type=\"hidden\" name=\"MD\" value=\"";
            //    navStr += encoder(dsrequest.MD);
            //    navStr += "\"></form></body></html>";
            //    _webBrowser.LoadHtmlString(navStr, null);
            //    _webBrowser.LoadRequest(new NSUrlRequest(new NSUrl(url)));
            //}
        }

        private async void PostRequest(IB2PRequest request, Action<IB2PResponse, B2PError> completion)
        {
            B2PError error = null;
            IB2PResponse response = null;
            string responseString = "";

            var data = request.RequestData(_sector, _password, _forwardUrl);

            string url = this.UrlForRequest() + request.Path();

            try
            {
                responseString = await Parameters.POST(url, data);
            }
            catch (Exception ex)
            {
                error = new B2PError(-1, ex.Message);
            }

            if (error == null)
            {
                if (responseString == "ok")
                {
                    B2POperationResponse changeRecResponse = (B2POperationResponse)request.ResponseForRequest();
                    changeRecResponse.message = responseString;
                    response = changeRecResponse;
                }
                else
                {
                    string rootName = "";
                    try
                    {
                        rootName = Parameters.getRootName(responseString);
                    }
                    catch (Exception e)
                    {
                        error = new B2PError(-1, e.Message);
                    }

                    if (error == null)
                    {
                        Dictionary<string, string> responseDictionary = Parameters.dictionaryForXMLString(responseString);
                        if (rootName == "error")
                        {
                            int code = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(responseDictionary, "code"));
                            error = new B2PError(code, Parameters.stringFromParsedDictionary(responseDictionary, "description"));
                        }

                        if (error == null)
                        {
                            bool isSignatureOK = true;

                            if (Parameters.stringFromParsedDictionary(responseDictionary, "signature") != "")
                            {
                                string signature = Parameters.stringFromParsedDictionary(responseDictionary, "signature");
                                isSignatureOK = Parameters.checkSignature(signature, responseString, _password);
                            }
                            if (isSignatureOK)
                            {
                                response = request.ResponseForRequest();
                                response.fillFromDictionary(responseDictionary);

                                if (response.GetType() == typeof(B2POrderResponse))
                                {
                                    B2POrderResponse orderResponse = (B2POrderResponse)response;
                                    List<string> operationsString = Parameters.cutOperationsInArrayFromResponseString(responseString);
                                    foreach (string operationString in operationsString)
                                    {
                                        Dictionary<string, string> operationDictionary = Parameters.dictionaryForXMLString(operationString);
                                        B2POperationResponse operationResponse = new B2POperationResponse();
                                        operationResponse.fillFromDictionary(operationDictionary);
                                        orderResponse.operations.Add(operationResponse);
                                    }
                                }

                            }
                            else
                                error = new B2PError(109, "Invalid signature");
                        }
                    }
                }
            }

            completion(response, error);
        }

        public void RegisterOrder(B2PRegisterRequest request, Action<B2PRegisterResponse, B2PError> completion)
        {
            PostRequest(request, (response, error) =>
                {
                    completion((B2PRegisterResponse)response, error);
                });
        }

        public void Operation(B2POperationRequest request, Action<B2POperationResponse, B2PError> completion)
        {
            PostRequest(request, (response, error) =>
                {
                    completion((B2POperationResponse)response, error);
                });
        }

        public string PurchaseURL(B2PPurchaseRequest request)
        {
            return GetRequest(request, false);
        }

        public string EpaymentURL(B2PEpaymentRequest request)
        {
            return GetRequest(request, false);
        }
    }
}

