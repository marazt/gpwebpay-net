using System.Xml;
using GPWebpayNet.Sdk.Models;
using Microsoft.AspNetCore.Http;

namespace GPWebpayNet.Sdk.Services
{
    public class PaymentResponseTransformer : IPaymentResponseTransformer
    {
        public PaymentResponse GetPaymentResponse(IQueryCollection queryArgs)
        {
            XmlElement addInfo = null;

            if ((string)queryArgs["ADDINFO"] != null)
            {
                var doc = new XmlDocument();
                doc.LoadXml(queryArgs["ADDINFO"]);
                addInfo = doc.DocumentElement;
            }
           
            
            return new PaymentResponse
            {
                Operation = queryArgs["OPERATION"],
                OrderNumber = int.Parse(queryArgs["ORDERNUMBER"]),
                MerOrderNumber = queryArgs.ContainsKey("MERORDERNUM") ? int.Parse(queryArgs["MERORDERNUM"]) : 0,
                PRCode = int.Parse(queryArgs["PRCODE"]),
                SRCode = int.Parse(queryArgs["SRCODE"]),
                ResultText = queryArgs["RESULTTEXT"],
                UserParam1 = queryArgs["USERPARAM1"],
                AddInfo = addInfo,
                Digest = queryArgs["DIGEST"],
                Digest1 = queryArgs["DIGEST1"]
            };
        }
        
        public string GetParameterString(PaymentResponse paymentResponse)
        {
            var merOrderNumber = paymentResponse.MerOrderNumber == 0 ? string.Empty : $"{paymentResponse.MerOrderNumber}|";
            var resultText = paymentResponse.ResultText == null ? string.Empty : $"|{paymentResponse.ResultText}";
            var userParam1 = paymentResponse.UserParam1 == null ? string.Empty : $"|{paymentResponse.UserParam1}";
            var addInfo = paymentResponse.AddInfo == null ? string.Empty : $"|{paymentResponse.AddInfo.OuterXml}";
            return $"{paymentResponse.Operation}|{paymentResponse.OrderNumber}|{merOrderNumber}{paymentResponse.PRCode}|{paymentResponse.SRCode}{resultText}{userParam1}{addInfo}";
        }
    }
}