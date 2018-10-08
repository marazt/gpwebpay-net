using System.Collections.Generic;
using System.Net.Http;
using System.Xml;
using GPWebpayNet.Sdk;
using GPWebpayNet.Sdk.Enums;
using GPWebpayNet.Sdk.Models;
using GPWebpayNet.Sdk.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

#pragma warning disable 219

namespace GPWebpayNet.Example
{
    class Program
    {
        public static void Main()
        {
            GetRedirectUrl();
        }
        
        /// <summary>
        /// Sample call to get redirect url that can be used in Controller
        /// or send it as a response to redirect to GPWP payment page 
        /// </summary>
        private static void GetRedirectUrl()
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole();

            const string url = Constants.GPWebpayUrlTest;
            const string privateCertificateFile = "certs/test.pfx";
            const string privateCertificateFilePassword = "test";
            const string publicCertificateFile = "certs/test.pfx";
            const string publicCertificateFilePassword = "test";
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Info"));

            var request = new PaymentRequest
            {
                MerchantNumber = "235235",
                OrderNumber = 2412,
                Amount = 65460,
                Currency = CurrencyCodeEnum.Eur,
                DepositFlag = 1,
                MerOrderNumber = "MerOrderNumber",
                Url = "https://www.example.org",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                MD = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                PaymentMethod = PaymentMethodEnum.Mps,
                DisabledPaymentMethod = PaymentMethodEnum.Crd,
                PaymentMethods = new[] {PaymentMethodEnum.Mcm, PaymentMethodEnum.NotSet},
                Email = "user@example.org",
                ReferenceNumber = "77987",
                AddInfo = doc.DocumentElement,
                Lang = "CZ"
            };

            var encodingLogger = loggerFactory.CreateLogger<EncodingService>();
            var clientServiceLogger = loggerFactory.CreateLogger<ClientService>();
            var clientService = new ClientService(new EncodingService(encodingLogger),
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLogger);

            // ReSharper disable once UnusedVariable
            var redirectUrl = clientService.GenerateGPWebPayRedirectUrl(
                url,
                request,
                privateCertificateFile,
                privateCertificateFilePassword,
                publicCertificateFile,
                publicCertificateFilePassword);
        }

        /// <summary>
        /// Sample of the POST call to GPWP
        /// </summary>
        private static async void PostRequestAsync()
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole();

            const string url = "https://example.org";
            const string privateCertificateFile = "certs/test.pfx";
            const string privateCertificateFilePassword = "test";
            const string publicCertificateFile = "certs/test.pfx";
            const string publicCertificateFilePassword = "test";
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Info"));

            var request = new PaymentRequest
            {
                MerchantNumber = "235235",
                OrderNumber = 2412,
                Amount = 65460,
                Currency = CurrencyCodeEnum.Eur,
                DepositFlag = 1,
                MerOrderNumber = "MerOrderNumber",
                Url = "https://www.example.org",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                MD = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                PaymentMethod = PaymentMethodEnum.Mps,
                DisabledPaymentMethod = PaymentMethodEnum.Crd,
                PaymentMethods = new[] {PaymentMethodEnum.Mcm, PaymentMethodEnum.NotSet},
                Email = "user@example.org",
                ReferenceNumber = "77987",
                AddInfo = doc.DocumentElement,
                Lang = "CZ"
            };

            var encodingLogger = loggerFactory.CreateLogger<EncodingService>();
            var clientServiceLogger = loggerFactory.CreateLogger<ClientService>();
            var clientService = new ClientService(new EncodingService(encodingLogger),
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLogger);

            // ReSharper disable once NotAccessedVariable
            string response;
            using (var client = new HttpClient())
            {
                // ReSharper disable once RedundantAssignment
                response = await clientService.PostRequestAsync(
                    client,
                    url,
                    request,
                    privateCertificateFile,
                    privateCertificateFilePassword,
                    publicCertificateFile, publicCertificateFilePassword);
            }
        }

        /// <summary>
        /// Sample handling of the incomming request from GPWP after payment process
        /// </summary>
        private static void ProcessIncommingGPWPRequest()
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole();
            
            // Args from Request object
            var queryArgs = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"OPERATION", new StringValues("Operation")},
                {"ORDERNUMBER", new StringValues("12332")},
                {"PRCODE", new StringValues("0")},
                {"SRCODE", new StringValues("0")},
                {"RESULTTEXT", new StringValues("ResultText")},
                {"USERPARAM1", new StringValues("UserParam1")},
                {"DIGEST", new StringValues("Digest")},
                {"DIGEST1", new StringValues("Digest1")},
            });

            const string merchantNumber = "25236236";
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";
        
            
            var encodingLogger = loggerFactory.CreateLogger<EncodingService>();
            var clientServiceLogger = loggerFactory.CreateLogger<ClientService>();
            var clientService = new ClientService(new EncodingService(encodingLogger),
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLogger);
    
            // Service will creates PaymentResponse from incomming args and validate response digest and digest1 with 
            // public certificate provided by GPWP and then check if "PRCODE" and "SRCODE" values have correct or error values
            // ReSharper disable once UnusedVariable
            var paymentResponse = clientService.ProcessGPWebPayResponse(queryArgs, merchantNumber, publicCertificateFile, password);
        }
    }
}