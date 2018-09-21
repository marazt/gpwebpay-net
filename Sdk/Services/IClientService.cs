using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GPWebpayNet.Sdk.Models;
using Microsoft.AspNetCore.Http;

namespace GPWebpayNet.Sdk.Services
{
    /// <summary>
    /// Main Sdk class representing client for communication.
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Create a request to GPWebpay Gateway and returns response as a string.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="paymentRequest">The payment request.</param>
        /// <param name="privateCert">The private cert.</param>
        /// <param name="privateCertPassword">The private cert password.</param>
        /// <param name="publicCert">The public cert.</param>
        /// <param name="publicCertPassword">The public cert password.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="keyStorageFlags">The key storage flags.</param>
        /// <returns>Response message as a string.</returns>
        Task<string> PostRequestAsync(
            HttpClient client,
            string url,
            PaymentRequest paymentRequest,
            string privateCert,
            string privateCertPassword,
            string publicCert,
            string publicCertPassword,
            int encoding = Encoding.DefaultEncoding,
            X509KeyStorageFlags keyStorageFlags = Encoding.DefaultKeyStorageFlags);

        
        /// <summary>
        /// Create a request to GPWebpay Gateway and returns response as a string.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="paymentRequest">The payment request.</param>
        /// <param name="privateCert">The private cert.</param>
        /// <param name="publicCert">The public cert.</param>
        /// <returns>Response message as a string.</returns>
        Task<string> PostRequestAsync(
            HttpClient client,
            string url,
            PaymentRequest paymentRequest,
            X509Certificate2 privateCert,
            X509Certificate2 publicCert);


        /// <summary>
        /// Generates the GPWebpay redirect URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="paymentRequest">The payment request.</param>
        /// <param name="privateCert">The private cert.</param>
        /// <param name="privateCertPassword">The private cert password.</param>
        /// <param name="publicCert">The public cert.</param>
        /// <param name="publicCertPassword">The public cert password.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="keyStorageFlags">The key storage flags.</param>
        /// <returns>Redirect URL.</returns>
        string GenerateGPWebPayRedirectUrl(
            string url,
            PaymentRequest paymentRequest,
            string privateCert,
            string privateCertPassword,
            string publicCert,
            string publicCertPassword,
            int encoding = Encoding.DefaultEncoding,
            X509KeyStorageFlags keyStorageFlags = Encoding.DefaultKeyStorageFlags);

        
        /// <summary>
        /// Generates the GPWebpay redirect URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="paymentRequest">The payment request.</param>
        /// <param name="privateCert">The private cert.</param>
        /// <param name="publicCert">The public cert.</param>
        /// <returns>Redirect URL.</returns>
        string GenerateGPWebPayRedirectUrl(
            string url,
            PaymentRequest paymentRequest,
            X509Certificate2 privateCert,
            X509Certificate2 publicCert);

        /// <summary>
        /// Processes (validates) response from GPWebpay.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="publicCert">The certificate.</param>
        /// <param name="publicCertPassword">The certificate password.</param>
        void ProcessGPWebPayResponse(
            PaymentResponse paymentResponse,
            string merchantNumber,
            string publicCert,
            string publicCertPassword
        );
        
        /// <summary>
        /// Processes (validates) response from GPWebpay.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="publicCert">The certificate.</param>
        void ProcessGPWebPayResponse(
            PaymentResponse paymentResponse,
            string merchantNumber,
            X509Certificate2 publicCert);

        /// <summary>
        /// Processes (validates) response from GPWebpay.
        /// </summary>
        /// <param name="queryArgs">The query arguments.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="publicCert">The public cert.</param>
        /// <param name="publicCertPassword">The public cert password.</param>
        /// <returns>Payment response generated from query args.</returns>
        PaymentResponse ProcessGPWebPayResponse(
            IQueryCollection queryArgs,
            string merchantNumber,
            string publicCert,
            string publicCertPassword);
        
        /// <summary>
        /// Processes (validates) response from GPWebpay.
        /// </summary>
        /// <param name="queryArgs">The query arguments.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="publicCert">The public cert.</param>
        /// <returns>Payment response generated from query args.</returns>
        PaymentResponse ProcessGPWebPayResponse(
            IQueryCollection queryArgs,
            string merchantNumber,
            X509Certificate2 publicCert);
    }
}