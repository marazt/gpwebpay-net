using System.Collections.Generic;
using GPWebpayNet.Sdk.Models;

namespace GPWebpayNet.Sdk.Services
{
    /// <summary>
    /// Payment request transformer helper.
    /// </summary>
    public interface IPaymentRequestTransformer
    {
        /// <summary>
        /// Gets the parameters for digest calculation.
        /// </summary>
        /// <param name="paymentRequest">The payment request.</param>
        /// <returns>Collection of kvp in exact form described in GPWebpay documentation.</returns>
        IList<KeyValuePair<string, string>> GetParametersForDigestCalculation(PaymentRequest paymentRequest);
    }
}