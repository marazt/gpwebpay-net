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
        
        public PaymentResponse ProcessGPWebPayResponse(
            IQueryCollection queryArgs,
            string merchantNumber,
            string certificate,
            string certificatePassword
        )
        {
            var paymentResponse = this.paymnetResponseTransformer.GetPaymentResponse(queryArgs);
            this.ProcessGPWebPayResponse(paymentResponse, merchantNumber, certificate, certificatePassword);
            return paymentResponse;
        }

        private static HttpClient PrepareClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            foreach (var kvp in Headers)
            {
                client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
            }

            return client;
        }
        
        private static string GetMessage(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return string.Join("|", parameters.Select(e => e.Value));
        }

        private static string PrettyPrintParameters(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return ClientService.ParametersToString(parameters, Environment.NewLine, ": ");
        }

        private static string ParametersToString(IEnumerable<KeyValuePair<string, string>> parameters,
            string lineDelimiter, string keyValueDelimiter)
        {
            return string.Join(lineDelimiter, parameters.Select(e => $"{e.Key}{keyValueDelimiter}{e.Value}"));
        }
    }
}