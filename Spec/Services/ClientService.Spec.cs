using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using GPWebpayNet.Sdk.Enums;
using GPWebpayNet.Sdk.Exceptions;
using GPWebpayNet.Sdk.Models;
using GPWebpayNet.Sdk.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace GPWebpayNet.Sdk.Spec.Services
{
    public class ClientServiceSpec : ABaseTest
    {
        [Fact]
        public void Should_generate_GPWP_redirect_url()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/test.pfx";
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
                OrderNumber = 2412,
                Amount = new decimal(64.6546),
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
                ReferenceNumber = "ReferenceNumber",
                AddInfo = null,
                Lang = "CZ"
            };

            var encodingLoggerMock = this.GetLoggerMock<EncodingService>();
            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(new EncodingService(encodingLoggerMock.Object),
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            const string expected =
                "https://example.org?MERCHANTNUMBER=MerchantNumber&OPERATION=CREATE_ORDER&ORDERNUMBER=2412&AMOUNT=64.6546&CURRENCY=978&DEPOSITFLAG=1&MERORDERNUM=MerOrderNumber&URL=https%3A%2F%2Fwww.example.org&DESCRIPTION=Lorem%20ipsum%20dolor%20sit%20amet%2C%20consectetuer%20adipiscing%20elit.&MD=Lorem%20ipsum%20dolor%20sit%20amet%2C%20consectetuer%20adipiscing%20elit.&PAYMETHOD=Mps&DISABLEPAYMETHOD=Crd&PAYMETHODS=Mcm%2CNotSet&EMAIL=user%40example.org&REFERENCENUMBER=ReferenceNumber&DIGEST=DeDFSo9FlVmh1JvphJM9CZeCq0VpXzBfZbJmEgsNpfu%2FyXPKY%2BNRk6iodmmy3UqulNHsS48BbF3EXeacNOCMwpSW5cIct%2Fk0V4gu1t7E9DgrLWnYeDOZfWEBPjIW01mGxhi2Gw2iOhGfTnQdLummvXr62L0aaFgtxkQHbDO9xcS%2FUNMCjZbDwMPxmvI0iwUdrPhpCmeZ9ZlpTWOvqIm%2FD1Emay55Uny4uysrUx%2BHoBIrpFhQeu3IC8c%2BskJW6cSPvZam%2BMIXsRpO%2FPq3qiUuh7nEs4ovsva1vqPOyukium5wI9nLTAa8ZIeMGfoHWmG1OM7TwRHDD770sRU9DkZB3g%3D%3D";

            // Act
            var result = testee.GenerateGPWebPayRedirectUrl(url, request, privateCertificateFile, password,
                publicCertificateFile, password);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_throw_DigestValidationException_while_generate_GPWP_redirect_url()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/test.pfx";
            const string publicCertificateFile = "certs/test_cert.pem";
            const string password = "test";

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
                OrderNumber = 2412,
                Amount = new decimal(64.6546),
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
                ReferenceNumber = "ReferenceNumber",
                AddInfo = null,
                Lang = "CZ"
            };

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var encodingServiceMock = this.GetEncodingServiceMock("digest", false);

            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            // Act
            // Assert
            Action action = () => testee.GenerateGPWebPayRedirectUrl(url, request, privateCertificateFile, password,
                publicCertificateFile, password);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Invalid digest: digest");
        }

        [Fact]
        public async void Should_post_request_async()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/test.pfx";
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";

            const string responseMessage = "Response from GPWP";

            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> {CallBase = true};
            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseMessage)
            });

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
                OrderNumber = 2412,
                Amount = new decimal(64.6546),
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
                ReferenceNumber = "ReferenceNumber",
                AddInfo = null,
                Lang = "CZ"
            };

            var encodingLoggerMock = this.GetLoggerMock<EncodingService>();
            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(new EncodingService(encodingLoggerMock.Object),
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            const string expected = responseMessage;

            // Act
            var result = await testee.PostRequestAsync(httpClient, url, request, privateCertificateFile, password,
                publicCertificateFile, password);

            // Assert            
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_throw_DigestValidationException_while_post_request_async()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/test.pfx";
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";
            const string responseMessage = "Response from GPWP";

            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> {CallBase = true};
            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseMessage)
            });

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
                OrderNumber = 2412,
                Amount = new decimal(64.6546),
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
                ReferenceNumber = "ReferenceNumber",
                AddInfo = null,
                Lang = "CZ"
            };

            var encodingServiceMock = this.GetEncodingServiceMock("digest", false);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            // Act
            // Assert
            Func<Task> action = async () => await testee.PostRequestAsync(httpClient, url, request,
                privateCertificateFile, password,
                publicCertificateFile, password);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Invalid digest: digest");
        }

        [Fact]
        public void Should_process_GPWP_response_from_query_args()
        {
            // Arrange
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
            
            const string publicCertificateFile = "test.pfx";
            const string password = "test";
        
            var encodingServiceMock = this.GetEncodingServiceMock("digest", true);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);
    
            // Act
            var result = testee.ProcessGPWebPayResponse(queryArgs, publicCertificateFile, password);

            // Assert
            result.Operation.Should().BeEquivalentTo("Operation");
            result.OrderNumber.Should().Be(12332);
            result.MerOrderNumber.Should().Be(0);
            result.PRCode.Should().Be(0);
            result.SRCode.Should().Be(0);
            result.ResultText.Should().BeEquivalentTo("ResultText");
            result.UserParam1.Should().BeEquivalentTo("UserParam1");
            result.AddInfo.Should().BeNull();
            result.Digest.Should().BeEquivalentTo("Digest");
            result.Digest1.Should().BeEquivalentTo("Digest1");
        }
        
        [Fact]
        public void Should_process_GPWP_response_from_response()
        {
            // Arrange
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";

            var response = new PaymentResponse
            {
                Operation = "Operation",
                OrderNumber = 12332,
                MerOrderNumber = 0,
                PRCode = 0,
                SRCode = 0,
                ResultText = "ResultText",
                UserParam1 = "UserParam1",
                AddInfo = null,
                Digest = "Digest",
                Digest1 = "Digest"
            };
        
            var encodingServiceMock = this.GetEncodingServiceMock("digest", true);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);
    
            // Act
            // Assert
            testee.ProcessGPWebPayResponse(response, publicCertificateFile, password);
        }
        
        [Fact]
        public void Should_throw_DigestValidationException_when_process_GPWP_response_from_response_when_invalid_digest()
        {
            // Arrange
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";

            var response = new PaymentResponse
            {
                Operation = "Operation",
                OrderNumber = 12332,
                MerOrderNumber = 0,
                PRCode = 0,
                SRCode = 0,
                ResultText = "ResultText",
                UserParam1 = "UserParam1",
                AddInfo = null,
                Digest = "Digest",
                Digest1 = "Digest"
            };
            
            var encodingServiceMock = this.GetEncodingServiceMock("digest", false);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            // Act
            // Assert
            Action action = () => testee.ProcessGPWebPayResponse(response, publicCertificateFile, password);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Invalid digest: digest");
        }
        
        [Fact]
        public void Should_throw_PaymentResponseException_when_process_GPWP_response_from_response_when_invalid_response_prcode()
        {
            // Arrange
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";

            var response = new PaymentResponse
            {
                Operation = "Operation",
                OrderNumber = 12332,
                MerOrderNumber = 0,
                PRCode = 1,
                SRCode = 0,
                ResultText = "ResultText",
                UserParam1 = "UserParam1",
                AddInfo = null,
                Digest = "Digest",
                Digest1 = "Digest"
            };
        
            var encodingServiceMock = this.GetEncodingServiceMock("digest", true);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);
    
            // Act
            // Assert
            Action action = () => testee.ProcessGPWebPayResponse(response, publicCertificateFile, password);
            action
                .Should().Throw<PaymentResponseException>()
                .WithMessage("Bad response");
        }
        
        [Fact]
        public void Should_throw_PaymentResponseException_when_process_GPWP_response_from_response_when_invalid_response_srcode()
        {
            // Arrange
            const string publicCertificateFile = "test.pfx";
            const string password = "test";

            var response = new PaymentResponse
            {
                Operation = "Operation",
                OrderNumber = 12332,
                MerOrderNumber = 0,
                PRCode = 0,
                SRCode = 3,
                ResultText = "ResultText",
                UserParam1 = "UserParam1",
                AddInfo = null,
                Digest = "Digest",
                Digest1 = "Digest"
            };

            var encodingServiceMock = this.GetEncodingServiceMock("digest", true);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);
    
            // Act
            // Assert
            Action action = () => testee.ProcessGPWebPayResponse(response, publicCertificateFile, password);
            action
                .Should().Throw<PaymentResponseException>()
                .WithMessage("Bad response");
        }

        private Mock<IEncodingService> GetEncodingServiceMock(string digest, bool validationResult)
        {
            var encodingServiceMock = new Mock<IEncodingService>();

            encodingServiceMock.Setup(e => e.SignData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<X509KeyStorageFlags>())).Returns(digest);
            encodingServiceMock.Setup(e => e.ValidateDigest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<X509KeyStorageFlags>())).Returns(validationResult);

            return encodingServiceMock;
        }
    }
}