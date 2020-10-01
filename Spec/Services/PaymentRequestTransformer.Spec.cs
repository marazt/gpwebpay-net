using System;
using System.Xml;
using FluentAssertions;
using GPWebpayNet.Sdk.Enums;
using GPWebpayNet.Sdk.Exceptions;
using GPWebpayNet.Sdk.Models;
using GPWebpayNet.Sdk.Services;
using Xunit;

namespace GPWebpayNet.Sdk.Spec.Services
{
    public class PaymentRequestTransformerSpec : ABaseTest
    {
        [Fact]
        public void Should_get_parameters_for_digest_calculation_with_all_parameters()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Info"));

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
                OrderNumber = 2412,
                Amount = 65460,
                Currency = CurrencyCodeEnum.EUR,
                DepositFlag = 1,
                MerOrderNumber = "MerOrderNumber",
                Url = "https://www.example.org",
                Description =
                    " Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                MD =
                    " Lorem ipsum dolor sit amet, consectetuer adipiscing elit. ",
                PaymentMethod = PaymentMethodEnum.Mps,
                DisabledPaymentMethod = PaymentMethodEnum.Crd,
                PaymentMethods = new[] {PaymentMethodEnum.Mcm, PaymentMethodEnum.NotSet},
                Email = "user@example.org",
                ReferenceNumber = "ReferenceNumber",
                AddInfo = doc.DocumentElement,
                Lang = "CZ"
            };
            
            var testee = new PaymentRequestTransformer();
            
            const string expected =
                "[MERCHANTNUMBER, MerchantNumber]|[OPERATION, CREATE_ORDER]|[ORDERNUMBER, 2412]|[AMOUNT, 65460]|[CURRENCY, 978]|[DEPOSITFLAG, 1]|[MERORDERNUM, MerOrderNumber]|[URL, https://www.example.org]|[DESCRIPTION, Lorem ipsum dolor sit amet, consectetuer adipiscing elit.]|[MD, Lorem ipsum dolor sit amet, consectetuer adipiscing elit.]|[PAYMETHOD, Mps]|[DISABLEPAYMETHOD, Crd]|[PAYMETHODS, Mcm,NotSet]|[EMAIL, user@example.org]|[REFERENCENUMBER, ReferenceNumber]|[ADDINFO, <Info />]|[LANG, CZ]";


            // Act
            var result = testee.GetParametersForDigestCalculation(request);
      
            // Assert
            string.Join("|", result).Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_throw_exception_because_md_is_too_long()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Info"));

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
                OrderNumber = 2412,
                Amount = 65460,
                Currency = CurrencyCodeEnum.EUR,
                DepositFlag = 1,
                MerOrderNumber = "MerOrderNumber",
                Url = "https://www.example.org",
                Description =
                    "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Suspendisse nisl. Cum sociis natoque penatibus et magnis dis parturient montes.",
                MD =
                    "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Suspendisse nisl. Cum sociis natoque penatibus et magnis dis parturient montes.",
                PaymentMethod = PaymentMethodEnum.Mps,
                DisabledPaymentMethod = PaymentMethodEnum.Crd,
                PaymentMethods = new[] { PaymentMethodEnum.Mcm, PaymentMethodEnum.NotSet },
                Email = "user@example.org",
                ReferenceNumber = "ReferenceNumber",
                AddInfo = doc.DocumentElement,
                Lang = "CZ"
            };

            var testee = new PaymentRequestTransformer();

            // Act
            // Assert
            Action action = () => testee.GetParametersForDigestCalculation(request);
            action
                .Should().Throw<InvalidPaymentRequestDataException>()
                .WithMessage("The value of parameter DESCRIPTION must be at most 255 bytes.");

        }

        [Fact]
        public void Should_get_parameters_for_digest_calculation_without_optional_parameters()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Info"));

            var request = new PaymentRequest
            {
                MerchantNumber = "MerchantNumber",
                OrderNumber = 2412,
                Amount = 65460,
                Currency = CurrencyCodeEnum.EUR,
                DepositFlag = 1,
                Url = "https://www.example.org",
                Lang = "CZ"
            };

            var testee = new PaymentRequestTransformer();
            
            const string expected =
                "[MERCHANTNUMBER, MerchantNumber]|[OPERATION, CREATE_ORDER]|[ORDERNUMBER, 2412]|[AMOUNT, 65460]|[CURRENCY, 978]|[DEPOSITFLAG, 1]|[URL, https://www.example.org]|[LANG, CZ]";

            // Act            
            var result = testee.GetParametersForDigestCalculation(request);

            // Assert
            string.Join("|", result).Should().BeEquivalentTo(expected);
        }
    }
}
