using System.Xml;
using GPWebpayNet.Sdk.Models;
using Microsoft.AspNetCore.Http;

namespace GPWebpayNet.Sdk.Services
{
    /// <summary>
    /// Payment response transformer helper.
    /// </summary>
    /// <seealso cref="GPWebpayNet.Sdk.Services.IPaymentResponseTransformer" />
    public class PaymentResponseTransformer : IPaymentResponseTransformer
    {
        /// <summary>
        /// Gets the payment response instance from query arguments.
        /// </summary>
        /// <param name="queryArgs">The query arguments.</param>
        /// <returns>
        /// PaymentResponse instance.
        /// </returns>
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
                OrderNumber = ulong.Parse(queryArgs["ORDERNUMBER"]),
                MerOrderNumber = queryArgs.ContainsKey("MERORDERNUM") ? uint.Parse(queryArgs["MERORDERNUM"]) : 0,
                MD = queryArgs["MD"],
                PRCode = uint.Parse(queryArgs["PRCODE"]),
                SRCode = uint.Parse(queryArgs["SRCODE"]),
                ResultText = queryArgs["RESULTTEXT"],
                UserParam1 = queryArgs["USERPARAM1"],
                AddInfo = addInfo,
                Digest = queryArgs["DIGEST"],
                Digest1 = queryArgs["DIGEST1"]
            };
        }

        /// <summary>
        /// Gets the parameter string from PaymentResponse instance.
        /// Parameter string must have exact form described in GPWebpay documentation.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <returns>
        /// PaymentResponse representted as a string message.
        /// </returns>
        public string GetParameterString(PaymentResponse paymentResponse)
        {
            var merOrderNumber = paymentResponse.MerOrderNumber == 0 ? string.Empty : $"{paymentResponse.MerOrderNumber}|";
            var md = paymentResponse.MD == null ? string.Empty : $"{paymentResponse.MD}|";
            var resultText = paymentResponse.ResultText == null ? string.Empty : $"|{paymentResponse.ResultText}";
            var userParam1 = paymentResponse.UserParam1 == null ? string.Empty : $"|{paymentResponse.UserParam1}";
            var addInfo = paymentResponse.AddInfo == null ? string.Empty : $"|{paymentResponse.AddInfo.OuterXml}";
            return $"{paymentResponse.Operation}|{paymentResponse.OrderNumber}|{merOrderNumber}{md}{paymentResponse.PRCode}|{paymentResponse.SRCode}{resultText}{userParam1}{addInfo}";
        }
    }
}