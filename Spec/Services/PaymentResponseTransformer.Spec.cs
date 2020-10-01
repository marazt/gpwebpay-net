using System.Collections.Generic;
using System.Xml;
using FluentAssertions;
using GPWebpayNet.Sdk.Models;
using GPWebpayNet.Sdk.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace GPWebpayNet.Sdk.Spec.Services
{
    public class PaymentResponseTransformerSpec : ABaseTest
    {
        [Fact]
        public void Should_get_parameter_string()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Info"));

            var response = new PaymentResponse
            {
                Operation = "Operation",
                OrderNumber = 34634634,
                MerOrderNumber = 321,
                MD = "MD-value",
                PRCode = 23,
                SRCode = 1,
                ResultText = "ResultText",
                UserParam1 = "ResultText",
                AddInfo = doc.DocumentElement,
                Digest = "Digest",
                Digest1 = "Digest1"      
            };

            var testee = new PaymentResponseTransformer();
            const string expected = "Operation|34634634|321|MD-value|23|1|ResultText|ResultText|<Info />";

            // Assert
            var result = testee.GetParameterString(response);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Should_get_payment_response_with_all_parameters()
        {
            // Arrange
            var data = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"OPERATION", new StringValues("Operation")},
                {"ORDERNUMBER", new StringValues("12332")},
                {"MERORDERNUM", new StringValues("1321")},
                {"MD", new StringValues("MD-value")},
                {"PRCODE", new StringValues("23")},
                {"SRCODE", new StringValues("12")},
                {"RESULTTEXT", new StringValues("ResultText")},
                {"USERPARAM1", new StringValues("UserParam1")},
                {"ADDINFO", new StringValues("<Info />")},
                {"DIGEST", new StringValues("Digest")},
                {"DIGEST1", new StringValues("Digest1")},
            });

            var testee = new PaymentResponseTransformer();
            
            // Act
            var result = testee.GetPaymentResponse(data);

            // Assert
            result.Operation.Should().BeEquivalentTo("Operation");
            result.OrderNumber.Should().Be(12332);
            result.MerOrderNumber.Should().Be(1321);
            result.MD.Should().Be("MD-value");
            result.PRCode.Should().Be(23);
            result.SRCode.Should().Be(12);
            result.ResultText.Should().BeEquivalentTo("ResultText");
            result.UserParam1.Should().BeEquivalentTo("UserParam1");
            result.AddInfo.Name.Should().BeEquivalentTo("Info");
            result.Digest.Should().BeEquivalentTo("Digest");
            result.Digest1.Should().BeEquivalentTo("Digest1");
        }
        
        [Fact]
        public void Should_get_payment_response_without_optional_parameters()
        {
            // Arrange
            var data = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"OPERATION", new StringValues("Operation")},
                {"ORDERNUMBER", new StringValues("12332")},
                {"PRCODE", new StringValues("23")},
                {"SRCODE", new StringValues("12")},
                {"RESULTTEXT", new StringValues("ResultText")},
                {"USERPARAM1", new StringValues("UserParam1")},
                {"DIGEST", new StringValues("Digest")},
                {"DIGEST1", new StringValues("Digest1")},
            });

            var testee = new PaymentResponseTransformer();
            
            // Act
            var result = testee.GetPaymentResponse(data);

            // Assert
            result.Operation.Should().BeEquivalentTo("Operation");
            result.OrderNumber.Should().Be(12332);
            result.MerOrderNumber.Should().Be(0);
            result.MD.Should().Be(null);
            result.PRCode.Should().Be(23);
            result.SRCode.Should().Be(12);
            result.ResultText.Should().BeEquivalentTo("ResultText");
            result.UserParam1.Should().BeEquivalentTo("UserParam1");
            result.AddInfo.Should().BeNull();
            result.Digest.Should().BeEquivalentTo("Digest");
            result.Digest1.Should().BeEquivalentTo("Digest1");
        }
    }
}
