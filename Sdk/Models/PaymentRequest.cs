using System.Xml;
using GPWebpayNet.Sdk.Enums;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GPWebpayNet.Sdk.Models
{
    /// <summary>
    /// Payment request object.
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// Gets or sets the merchant number.
        /// </summary>
        /// <value>
        /// The merchant number.
        /// </value>
        public string MerchantNumber { get; set; }
        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        public ulong OrderNumber { get; set; }
        /// <summary>
        /// Gets or sets the amount. 
        /// The amount is in cents or halere, i.e. €1.59 is 159.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public uint Amount { get; set; }
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public CurrencyCodeEnum Currency { get; set; }
        /// <summary>
        /// Gets or sets the deposit flag.
        /// </summary>
        /// <value>
        /// The deposit flag.
        /// </value>
        public uint DepositFlag { get; set; }
        /// <summary>
        /// Gets or sets the mer order number.
        /// </summary>
        /// <value>
        /// The mer order number.
        /// </value>
        public string MerOrderNumber { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the md.
        /// </summary>
        /// <value>
        /// The md.
        /// </value>
        public string MD { get; set; }
        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        public PaymentMethodEnum PaymentMethod { get; set; }
        /// <summary>
        /// Gets or sets the disabled payment method.
        /// </summary>
        /// <value>
        /// The disabled payment method.
        /// </value>
        public PaymentMethodEnum DisabledPaymentMethod { get; set; }
        /// <summary>
        /// Gets or sets the payment methods.
        /// </summary>
        /// <value>
        /// The payment methods.
        /// </value>
        public PaymentMethodEnum[] PaymentMethods { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the reference number.
        /// </summary>
        /// <value>
        /// The reference number.
        /// </value>
        public string ReferenceNumber { get; set; }
        /// <summary>
        /// Gets or sets the add information.
        /// </summary>
        /// <value>
        /// The add information.
        /// </value>
        public XmlElement AddInfo { get; set; }
        /// <summary>
        /// Gets or sets the digest.
        /// </summary>
        /// <value>
        /// The digest.
        /// </value>
        public string Digest { get; set; }
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Lang { get; set; }
    }
}