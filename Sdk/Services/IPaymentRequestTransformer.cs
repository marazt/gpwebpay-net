using System.Collections.Generic;
using GPWebpayNet.Sdk.Models;

namespace GPWebpayNet.Sdk.Services
{
    public interface IPaymentRequestTransformer
    {
        IList<KeyValuePair<string, string>> GetParametersForDigestCalculation(PaymentRequest paymentRequest);
    }
}