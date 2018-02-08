using System.Xml;
using GPWebpayNet.Sdk.Enums;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GPWebpayNet.Sdk.Models
{
       public class PaymentRequest
    {
        public string MerchantNumber { get; set; }
        public int OrderNumber { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCodeEnum Currency { get; set; }
        public int DepositFlag { get; set; }
        public string MerOrderNumber { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string MD { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public PaymentMethodEnum DisabledPaymentMethod { get; set; }
        public PaymentMethodEnum[] PaymentMethods { get; set; }
        public string Email { get; set; }
        public string ReferenceNumber { get; set; }
        public XmlElement AddInfo { get; set; }
        public string Digets { get; set; }
        public string Lang { get; set; }
    }
}