using GPWebpayNet.Sdk.Models;
using Microsoft.AspNetCore.Http;

namespace GPWebpayNet.Sdk.Services
{
    /// <summary>
    /// Payment response transformer helper.
    /// </summary>
    public interface IPaymentResponseTransformer
    {
        /// <summary>
        /// Gets the payment response instace from query arguments.
        /// </summary>
        /// <param name="queryArgs">The query arguments.</param>
        /// <returns>PaymentResponse instance.</returns>
        PaymentResponse GetPaymentResponse(IQueryCollection queryArgs);

        /// <summary>
        /// Gets the parameter string from PaymentResponse instance.
        /// Parameter string must have exact form described in GPWebpay documentation.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <returns>PaymentResponse representted as a string message.</returns>
        string GetParameterString(PaymentResponse paymentResponse);
    }
}