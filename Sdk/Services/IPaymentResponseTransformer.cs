using GPWebpayNet.Sdk.Models;
using Microsoft.AspNetCore.Http;

namespace GPWebpayNet.Sdk.Services
{
    public interface IPaymentResponseTransformer
    {
        PaymentResponse GetPaymentResponse(IQueryCollection queryArgs);
        string GetParameterString(PaymentResponse paymentResponse);
    }
}