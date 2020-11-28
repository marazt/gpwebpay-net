﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GPWebpayNet.Sdk.Enums;
using GPWebpayNet.Sdk.Exceptions;
using GPWebpayNet.Sdk.Models;

namespace GPWebpayNet.Sdk.Services
{
    /// <summary>
    /// Payment request transformer helper.
    /// </summary>
    /// <seealso cref="GPWebpayNet.Sdk.Services.IPaymentRequestTransformer" />
    public class PaymentRequestTransformer : IPaymentRequestTransformer
    {
        private const int MaxLength = 255;

        /// <summary>
        /// Gets the parameters for digest calculation.
        /// </summary>
        /// <param name="paymentRequest">The payment request.</param>
        /// <returns>
        /// Collection of kvp in exact form described in GPWebpay documentation.
        /// </returns>
        public IList<KeyValuePair<string, string>> GetParametersForDigestCalculation(PaymentRequest paymentRequest)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("MERCHANTNUMBER", paymentRequest.MerchantNumber),
                new KeyValuePair<string, string>("OPERATION", "CREATE_ORDER"),
                new KeyValuePair<string, string>("ORDERNUMBER", paymentRequest.OrderNumber.ToString()),
                new KeyValuePair<string, string>("AMOUNT",
                    paymentRequest.Amount.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("CURRENCY", ((int) paymentRequest.Currency).ToString()),
                new KeyValuePair<string, string>("DEPOSITFLAG", paymentRequest.DepositFlag.ToString())
            };

            if (paymentRequest.MerOrderNumber != null)
            {
                parameters.Add(new KeyValuePair<string, string>("MERORDERNUM", paymentRequest.MerOrderNumber));
            }

            parameters.Add(new KeyValuePair<string, string>("URL", paymentRequest.Url));

            if (paymentRequest.Description != null)
            {
                parameters.Add(new KeyValuePair<string, string>("DESCRIPTION", TransformToAscii(paymentRequest.Description, "DESCRIPTION")));
            }

            if (paymentRequest.MD != null)
            {
                parameters.Add(new KeyValuePair<string, string>("MD", TransformToAscii(paymentRequest.MD, "MD")));
            }

            if (paymentRequest.PaymentMethod != PaymentMethodEnum.NotSet)
            {
                parameters.Add(new KeyValuePair<string, string>("PAYMETHOD",
                    Extensions.GetEnumValueName(paymentRequest.PaymentMethod)));
            }

            if (paymentRequest.DisabledPaymentMethod != PaymentMethodEnum.NotSet)
            {
                parameters.Add(new KeyValuePair<string, string>("DISABLEPAYMETHOD",
                    Extensions.GetEnumValueName(paymentRequest.DisabledPaymentMethod)));
            }

            if (paymentRequest.PaymentMethods != null && paymentRequest.PaymentMethods.Length > 0)
            {
                parameters.Add(new KeyValuePair<string, string>("PAYMETHODS",
                    string.Join(",", paymentRequest.PaymentMethods.Select(Extensions.GetEnumValueName))));
            }

            if (paymentRequest.Email != null)
            {
                parameters.Add(new KeyValuePair<string, string>("EMAIL", paymentRequest.Email));
            }

            if (paymentRequest.ReferenceNumber != null)
            {
                parameters.Add(new KeyValuePair<string, string>("REFERENCENUMBER", paymentRequest.ReferenceNumber));
            }

            if (paymentRequest.AddInfo != null)
            {
                parameters.Add(new KeyValuePair<string, string>("ADDINFO", paymentRequest.AddInfo.OuterXml));
            }

            return parameters;
        }

        private static string TransformToAscii (string data, string parameterName)
        {
            var asciiData = data.Trim().GetInASCII();
            if (System.Text.Encoding.ASCII.GetByteCount(asciiData) > MaxLength)
            {
                throw new InvalidPaymentRequestDataException($"The value of parameter {parameterName} must be at most {MaxLength} bytes.", null);
            }
            return asciiData;
        }
    }
}
