using System.Net.Http;
using System.Threading.Tasks;
using GPWebpayNet.Sdk.Models;
using Microsoft.AspNetCore.Http;

namespace GPWebpayNet.Sdk.Services
{
    public interface IClientService
    {
        Task<string> PostRequestAsync(
            HttpClient client,
            string url,
            PaymentRequest paymentRequest,
            string privateCert,
            string privateCertPassword,
            string publicCert,
            string publicCertPassword);

        string GenerateGPWebPayRedirectUrl(
            string url,
            PaymentRequest paymentRequest,
            string privateCert,
            string privateCertPassword,
            string publicCert,
            string publicCertPassword);

        void ProcessGPWebPayResponse(
            PaymentResponse paymentResponse,
            string merchantNumber,
            string certificate,
            string certificatePassword
        );
        
        PaymentResponse ProcessGPWebPayResponse(
            IQueryCollection queryArgs,
            string merchantNumber,
            string publicCert,
            string publicCertPassword
        );
    }
}