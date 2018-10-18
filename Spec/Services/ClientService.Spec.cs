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
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
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
                ReferenceNumber = "ReferenceNumber",
                AddInfo = null,
                Lang = "CZ"
            };

            var encodingLoggerMock = this.GetLoggerMock<EncodingService>();
            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(new EncodingService(encodingLoggerMock.Object),
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            const string expected =
                "https://example.org?MERCHANTNUMBER=MerchantNumber&OPERATION=CREATE_ORDER&ORDERNUMBER=2412&AMOUNT=65460&CURRENCY=978&DEPOSITFLAG=1&MERORDERNUM=MerOrderNumber&URL=https%3A%2F%2Fwww.example.org&DESCRIPTION=Lorem%20ipsum%20dolor%20sit%20amet%2C%20consectetuer%20adipiscing%20elit.&MD=Lorem%20ipsum%20dolor%20sit%20amet%2C%20consectetuer%20adipiscing%20elit.&PAYMETHOD=Mps&DISABLEPAYMETHOD=Crd&PAYMETHODS=Mcm%2CNotSet&EMAIL=user%40example.org&REFERENCENUMBER=ReferenceNumber&DIGEST=M%2F4LuFpMKYgfmTGtK5W9gepEFBnnJ6jPCsafnXeP0KY58oShqxpg0A6G%2FHK%2BWMzlqSz8hQRL%2FWrmDfkydCcuO7IFlsydmkcn6GNygEXOFepoHfqC1oMTF%2F7w8oaTBJz%2Frt%2FVKX%2Fe%2Fe2wnumK4NsLbLGAtuXKM2ldGQLYJsLdqmPaiguqNcgwCt0WxTlaSyxmheijwW%2B2k%2FeltyP4E4ce4OMr7FUy5Pb3llWDbEnAzUIhgiATFA%2BrbxS0KL5ZAkdJ6HfDRWPSYmHUBLjyLkyJYmmRMMOc3P%2FioYApwFr3mUQayY9BUG%2BikilFYxZEJKqjqAS%2BqvB50n5PA78mpFlaMA%3D%3D";

            // Act
            var result = testee.GenerateGPWebPayRedirectUrl(url, request, privateCertificateFile,
                privateCertificatePassword,
                publicCertificateFile,
                publicCertificatePassword);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_generate_GPWP_redirect_url_with_certificate()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;
            var privateCert = new X509Certificate2(privateCertificateFile, privateCertificatePassword,
                Encoding.DefaultKeyStorageFlags);
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
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
                ReferenceNumber = "ReferenceNumber",
                AddInfo = null,
                Lang = "CZ"
            };

            var encodingLoggerMock = this.GetLoggerMock<EncodingService>();
            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(new EncodingService(encodingLoggerMock.Object),
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            const string expected =
                "https://example.org?MERCHANTNUMBER=MerchantNumber&OPERATION=CREATE_ORDER&ORDERNUMBER=2412&AMOUNT=65460&CURRENCY=978&DEPOSITFLAG=1&MERORDERNUM=MerOrderNumber&URL=https%3A%2F%2Fwww.example.org&DESCRIPTION=Lorem%20ipsum%20dolor%20sit%20amet%2C%20consectetuer%20adipiscing%20elit.&MD=Lorem%20ipsum%20dolor%20sit%20amet%2C%20consectetuer%20adipiscing%20elit.&PAYMETHOD=Mps&DISABLEPAYMETHOD=Crd&PAYMETHODS=Mcm%2CNotSet&EMAIL=user%40example.org&REFERENCENUMBER=ReferenceNumber&DIGEST=M%2F4LuFpMKYgfmTGtK5W9gepEFBnnJ6jPCsafnXeP0KY58oShqxpg0A6G%2FHK%2BWMzlqSz8hQRL%2FWrmDfkydCcuO7IFlsydmkcn6GNygEXOFepoHfqC1oMTF%2F7w8oaTBJz%2Frt%2FVKX%2Fe%2Fe2wnumK4NsLbLGAtuXKM2ldGQLYJsLdqmPaiguqNcgwCt0WxTlaSyxmheijwW%2B2k%2FeltyP4E4ce4OMr7FUy5Pb3llWDbEnAzUIhgiATFA%2BrbxS0KL5ZAkdJ6HfDRWPSYmHUBLjyLkyJYmmRMMOc3P%2FioYApwFr3mUQayY9BUG%2BikilFYxZEJKqjqAS%2BqvB50n5PA78mpFlaMA%3D%3D";

            // Act
            var result = testee.GenerateGPWebPayRedirectUrl(url, request, privateCert, publicCert);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_throw_DigestValidationException_while_generate_GPWP_redirect_url()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
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
            Action action = () => testee.GenerateGPWebPayRedirectUrl(url, request,
                privateCertificateFile, privateCertificatePassword,
                publicCertificateFile, publicCertificatePassword);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Invalid digest: digest");
        }

        [Fact]
        public void Should_throw_DigestValidationException_while_generate_GPWP_redirect_url_with_certificate()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;
            var privateCert = new X509Certificate2(privateCertificateFile, privateCertificatePassword,
                Encoding.DefaultKeyStorageFlags);
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
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
            Action action = () => testee.GenerateGPWebPayRedirectUrl(url, request, privateCert, publicCert);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Invalid digest: digest");
        }

        [Fact]
        public async void Should_post_request_async()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;

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
            var result = await testee.PostRequestAsync(httpClient, url, request,
                privateCertificateFile, privateCertificatePassword,
                publicCertificateFile, publicCertificatePassword);

            // Assert            
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void Should_post_request_async_with_certificate()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;
            var privateCert = new X509Certificate2(privateCertificateFile, privateCertificatePassword,
                Encoding.DefaultKeyStorageFlags);
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

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
            var result = await testee.PostRequestAsync(httpClient, url, request, privateCert, publicCert);

            // Assert            
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_throw_DigestValidationException_while_post_request_async()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;
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
                privateCertificateFile, privateCertificatePassword,
                publicCertificateFile, publicCertificatePassword);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Invalid digest: digest");
        }

        [Fact]
        public void Should_throw_DigestValidationException_while_post_request_async_with_certificate()
        {
            // Arrange
            const string url = "https://example.org";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;
            const string responseMessage = "Response from GPWP";
            var privateCert = new X509Certificate2(privateCertificateFile, privateCertificatePassword,
                Encoding.DefaultKeyStorageFlags);
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

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
                privateCert, publicCert);
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

            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;

            var encodingServiceMock = this.GetEncodingServiceMock("digest", true);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            // Act
            var result = testee.ProcessGPWebPayResponse(queryArgs, merchantNumber, publicCertificateFile,
                publicCertificatePassword);

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
        public void Should_process_GPWP_response_from_query_args_with_certificate()
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

            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

            var encodingServiceMock = this.GetEncodingServiceMock("digest", true);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingServiceMock.Object,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            // Act
            var result = testee.ProcessGPWebPayResponse(queryArgs, merchantNumber, publicCert);

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
            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;

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
            testee.ProcessGPWebPayResponse(response, merchantNumber, publicCertificateFile, publicCertificatePassword);
        }

        [Fact]
        public void Should_process_GPWP_response_from_response_with_certificate()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

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
            testee.ProcessGPWebPayResponse(response, merchantNumber, publicCert);
        }

        [Fact]
        public void Should_process_GPWP_response_from_response_no_mock()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string privateCertificateFile = "certs/server.pfx";
            const string privateCertificatePassword = "test";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;

            var response = new PaymentResponse
            {
                Operation = "Operation",
                OrderNumber = 12332,
                MerOrderNumber = 0,
                PRCode = 0,
                SRCode = 0,
                ResultText = "ResultText",
                UserParam1 = "UserParam1",
                AddInfo = null
            };

            var encodingServiceMock = this.GetLoggerMock<EncodingService>();
            var encodingService = new EncodingService(encodingServiceMock.Object);
            var prt = new PaymentResponseTransformer();

            var parameterString = prt.GetParameterString(response);

            response.Digest =
                encodingService.SignData(parameterString, privateCertificateFile, privateCertificatePassword);
            response.Digest1 = encodingService.SignData($"{parameterString}|{merchantNumber}", privateCertificateFile,
                privateCertificatePassword);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingService,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            // Act
            // Assert
            testee.ProcessGPWebPayResponse(response, merchantNumber, publicCertificateFile, publicCertificatePassword);
        }

        [Fact]
        public void Should_process_GPWP_response_from_response_no_mock_with_certificate()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string privateCertificateFile = "certs/server.pfx";
            const string privateCertificatePassword = "test";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;
            var privateCert = new X509Certificate2(privateCertificateFile, privateCertificatePassword,
                Encoding.DefaultKeyStorageFlags);
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

            var response = new PaymentResponse
            {
                Operation = "Operation",
                OrderNumber = 12332,
                MerOrderNumber = 0,
                PRCode = 0,
                SRCode = 0,
                ResultText = "ResultText",
                UserParam1 = "UserParam1",
                AddInfo = null
            };

            var encodingServiceMock = this.GetLoggerMock<EncodingService>();
            var encodingService = new EncodingService(encodingServiceMock.Object);
            var prt = new PaymentResponseTransformer();

            var parameterString = prt.GetParameterString(response);

            response.Digest = encodingService.SignData(parameterString, privateCert);
            response.Digest1 = encodingService.SignData($"{parameterString}|{merchantNumber}", privateCert);

            var clientServiceLoggerMock = this.GetLoggerMock<ClientService>();
            var testee = new ClientService(encodingService,
                new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLoggerMock.Object);

            // Act
            // Assert
            testee.ProcessGPWebPayResponse(response, merchantNumber, publicCert);
        }

        [Fact]
        public void
            Should_throw_DigestValidationException_when_process_GPWP_response_from_response_when_invalid_digest()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;

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
            Action action = () =>
                testee.ProcessGPWebPayResponse(response, merchantNumber, publicCertificateFile,
                    publicCertificatePassword);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Invalid digest: digest");
        }

        [Fact]
        public void
            Should_throw_DigestValidationException_when_process_GPWP_response_from_response_when_invalid_digest_with_certificate()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

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
            Action action = () => testee.ProcessGPWebPayResponse(response, merchantNumber, publicCert);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Invalid digest: digest");
        }

        [Fact]
        public void
            Should_throw_PaymentResponseException_when_process_GPWP_response_from_response_when_invalid_response_prcode()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;

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
            Action action = () =>
                testee.ProcessGPWebPayResponse(response, merchantNumber, publicCertificateFile,
                    publicCertificatePassword);
            action
                .Should().Throw<PaymentResponseException>()
                .WithMessage(@"Bad request:
PR 1: Field too long.
SR 0: ");
        }

        [Fact]
        public void
            Should_throw_PaymentResponseException_when_process_GPWP_response_from_response_when_invalid_response_prcode_with_certificate()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

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
            Action action = () => testee.ProcessGPWebPayResponse(response, merchantNumber, publicCert);
            action
                .Should().Throw<PaymentResponseException>()
                .WithMessage(@"Bad request:
PR 1: Field too long.
SR 0: ");
        }

        [Fact]
        public void
            Should_throw_PaymentResponseException_when_process_GPWP_response_from_response_when_invalid_response_srcode()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;

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
            Action action = () =>
                testee.ProcessGPWebPayResponse(response, merchantNumber, publicCertificateFile,
                    publicCertificatePassword);
            action
                .Should().Throw<PaymentResponseException>()
                .WithMessage(@"Bad request:
PR 0: OK
SR 3: Unknown value for PR code 0 and SR code 3.");
        }

        [Fact]
        public void
            Should_throw_PaymentResponseException_when_process_GPWP_response_from_response_when_invalid_response_srcode_with_certificate()
        {
            // Arrange
            const string merchantNumber = "62346346";
            const string publicCertificateFile = "certs/server_pub.pem";
            const string publicCertificatePassword = null;
            var publicCert = new X509Certificate2(publicCertificateFile, publicCertificatePassword,
                Encoding.DefaultKeyStorageFlags);

            var response = new PaymentResponse
            {
                Operation = "Operation",
                OrderNumber = 12332,
                MerOrderNumber = 0,
                PRCode = 28,
                SRCode = 3006,
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
            Action action = () => testee.ProcessGPWebPayResponse(response, merchantNumber, publicCert);
            action
                .Should().Throw<PaymentResponseException>()
                .WithMessage(@"Bad request:
PR 28: Declined in 3D.
SR 3006: Declined in 3D. Technical problem during Cardholder authentication.");
        }

        private Mock<IEncodingService> GetEncodingServiceMock(string digest, bool validationResult)
        {
            var encodingServiceMock = new Mock<IEncodingService>();

            encodingServiceMock.Setup(e => e.SignData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<X509KeyStorageFlags>())).Returns(digest);

            encodingServiceMock.Setup(e => e.SignData(It.IsAny<string>(), It.IsAny<X509Certificate2>(),
                It.IsAny<int>())).Returns(digest);

            encodingServiceMock.Setup(e => e.ValidateDigest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<X509KeyStorageFlags>())).Returns(validationResult);

            encodingServiceMock.Setup(e => e.ValidateDigest(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<X509Certificate2>(), It.IsAny<int>())).Returns(validationResult);

            return encodingServiceMock;
        }
    }
}