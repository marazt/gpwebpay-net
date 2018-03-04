using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GPWebpayNet.Sdk.Exceptions;
using GPWebpayNet.Sdk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GPWebpayNet.Sdk.Services
{
    /// <summary>
    /// Main Sdk class representing client for communication.
    /// </summary>
    public class ClientService : IClientService
    {
        private static readonly KeyValuePair<string, string>[] Headers =
        {
            new KeyValuePair<string, string>("Accept", "application/x-www-form-urlencoded, */*"),
            new KeyValuePair<string, string>("Connection", "keep-alive")
        };

        private readonly IEncodingService encodingService;
        private readonly IPaymentRequestTransformer paymnetRequestTransformer;
        private readonly IPaymentResponseTransformer paymnetResponseTransformer;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientService"/> class.
        /// </summary>
        /// <param name="encodingService">The encoding service.</param>
        /// <param name="paymnetRequestTransformer">The paymnet request transformer.</param>
        /// <param name="paymnetResponseTransformer">The paymnet response transformer.</param>
        /// <param name="logger">The logger.</param>
        public ClientService(
            IEncodingService encodingService,
            IPaymentRequestTransformer paymnetRequestTransformer,
            IPaymentResponseTransformer paymnetResponseTransformer,
            ILogger<ClientService> logger)
        {
            this.encodingService = encodingService;
            this.paymnetRequestTransformer = paymnetRequestTransformer;
            this.paymnetResponseTransformer = paymnetResponseTransformer;
            this.logger = logger;
        }


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
        /// <returns>
        /// Response message as a string.
        /// </returns>
        /// <exception cref="GPWebpayNet.Sdk.Exceptions.DigestValidationException">ull)</exception>
        public async Task<string> PostRequestAsync(
            HttpClient client,
            string url,
            PaymentRequest paymentRequest,
            string privateCert,
            string privateCertPassword,
            string publicCert,
            string publicCertPassword)
        {
            var parameters = this.paymnetRequestTransformer.GetParametersForDigestCalculation(paymentRequest);
            var message = ClientService.GetMessage(parameters);
            var digest = this.encodingService.SignData(message, privateCert, privateCertPassword);
            var isValid = this.encodingService.ValidateDigest(digest, message, publicCert, publicCertPassword);

            this.logger.LogInformation(
                $"Calling GPWP API with parameters: {ClientService.PrettyPrintParameters(parameters)}");

            if (!isValid)
            {
                this.logger.LogError($"Invalid digest: {digest}");
                throw new DigestValidationException($"Invalid digest: {digest}", null);
            }

            parameters.Add(new KeyValuePair<string, string>("DIGEST", digest));

            var content = new FormUrlEncodedContent(parameters);
            {
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            }

            var response = await ClientService.PrepareClient(client).PostAsync(url, content);
            var responseMessage = await response.Content.ReadAsStringAsync();
            return responseMessage;
        }


        /// <summary>
        /// Generates the GPWebpay redirect URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="paymentRequest">The payment request.</param>
        /// <param name="privateCert">The private cert.</param>
        /// <param name="privateCertPassword">The private cert password.</param>
        /// <param name="publicCert">The public cert.</param>
        /// <param name="publicCertPassword">The public cert password.</param>
        /// <returns>
        /// Redirect URL.
        /// </returns>
        /// <exception cref="GPWebpayNet.Sdk.Exceptions.DigestValidationException">ull)</exception>
        public string GenerateGPWebPayRedirectUrl(
            string url,
            PaymentRequest paymentRequest,
            string privateCert,
            string privateCertPassword,
            string publicCert,
            string publicCertPassword)
        {
            var parameters = this.paymnetRequestTransformer.GetParametersForDigestCalculation(paymentRequest);
            var message = ClientService.GetMessage(parameters);
            var digest = this.encodingService.SignData(message, privateCert, privateCertPassword);
            var isValid = this.encodingService.ValidateDigest(digest, message, publicCert, publicCertPassword);

            this.logger.LogInformation(
                $"GPWP parameters: {ClientService.PrettyPrintParameters(parameters)}");

            if (!isValid)
            {
                this.logger.LogError($"Invalid digest: {digest}");
                throw new DigestValidationException($"Invalid digest: {digest}", null);
            }

            parameters.Add(new KeyValuePair<string, string>("DIGEST", digest));
            var args = string.Join("&",
                parameters.Select(e => $"{Uri.EscapeDataString(e.Key)}={Uri.EscapeDataString(e.Value)}"));

            return $"{url}?{args}";
        }


        /// <summary>
        /// Processes (validates) response from GPWebpay.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="certificatePassword">The certificate password.</param>
        /// <exception cref="GPWebpayNet.Sdk.Exceptions.DigestValidationException">
        /// ull)
        /// or
        /// ull)
        /// </exception>
        /// <exception cref="GPWebpayNet.Sdk.Exceptions.PaymentResponseException">Bad response - null</exception>
        public void ProcessGPWebPayResponse(
            PaymentResponse paymentResponse,
            string merchantNumber,
            string certificate,
            string certificatePassword
        )
        {
            var isValid = this.encodingService.ValidateDigest(paymentResponse.Digest,
                this.paymnetResponseTransformer.GetParameterString(paymentResponse),
                certificate,
                certificatePassword);

            if (!isValid)
            {
                this.logger.LogError($"Invalid digest: {paymentResponse.Digest}");
                throw new DigestValidationException($"Invalid digest: {paymentResponse.Digest}", null);
            }
            
            isValid = this.encodingService.ValidateDigest(paymentResponse.Digest1,
                $"{this.paymnetResponseTransformer.GetParameterString(paymentResponse)}|{merchantNumber}",
                certificate,
                certificatePassword);

            if (!isValid)
            {
                this.logger.LogError($"Invalid diges1t: {paymentResponse.Digest1}");
                throw new DigestValidationException($"Invalid digest1: {paymentResponse.Digest1}", null);
            }

            if (paymentResponse.PRCode != 0 || paymentResponse.SRCode != 0)
            {
                this.logger.LogError("Bad response");
                throw new PaymentResponseException(paymentResponse.PRCode, paymentResponse.SRCode, "Bad response",
                    null);
            }
        }


        /// <summary>
        /// Processes (validates) response from GPWebpay.
        /// </summary>
        /// <param name="queryArgs">The query arguments.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="publicCert">The public cert.</param>
        /// <param name="publicCertPassword">The public cert password.</param>
        /// <returns>
        /// Payment response generated from query args.
        /// </returns>
        public PaymentResponse ProcessGPWebPayResponse(
            IQueryCollection queryArgs,
            string merchantNumber,
            string publicCert,
            string publicCertPassword
        )
        {
            var paymentResponse = this.paymnetResponseTransformer.GetPaymentResponse(queryArgs);
            this.ProcessGPWebPayResponse(paymentResponse, merchantNumber, publicCert, publicCertPassword);
            return paymentResponse;
        }

        /// <summary>
        /// Prepares the Http client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>Http client instance.</returns>
        private static HttpClient PrepareClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            foreach (var kvp in Headers)
            {
                client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
            }

            return client;
        }

        /// <summary>
        /// Gets the message from kvp parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Concatenation of kvp parameters.</returns>
        private static string GetMessage(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return string.Join("|", parameters.Select(e => e.Value));
        }

        /// <summary>
        /// Pretty-prints parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Pretty-printed paramters.</returns>
        private static string PrettyPrintParameters(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return ClientService.ParametersToString(parameters, Environment.NewLine, ": ");
        }

        /// <summary>
        /// Transform kvp parameters into a string.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="lineDelimiter">The line delimiter.</param>
        /// <param name="keyValueDelimiter">The key value delimiter.</param>
        /// <returns>Kvp parameters as a string.</returns>
        private static string ParametersToString(IEnumerable<KeyValuePair<string, string>> parameters,
            string lineDelimiter, string keyValueDelimiter)
        {
            return string.Join(lineDelimiter, parameters.Select(e => $"{e.Key}{keyValueDelimiter}{e.Value}"));
        }
    }
}