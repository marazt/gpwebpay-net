using System;

namespace GPWebpayNet.Sdk.Exceptions
{
    /// <summary>
    /// Payment response validation exception.
    /// </summary>
    /// <seealso cref="GPWebpayNet.Sdk.Exceptions.GPWebpayNetException" />
    public class PaymentResponseException : GPWebpayNetException
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// Gets or sets invalid the PRCode.
        /// </summary>
        /// <value>
        /// The PRCode.
        /// </value>
        public int PrCode { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// Gets or sets the SRCode.
        /// </summary>
        /// <value>
        /// The SRCode.
        /// </value>
        public int SrCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentResponseException"/> class.
        /// </summary>
        /// <param name="prCode">The pr code.</param>
        /// <param name="srCode">The sr code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PaymentResponseException(int prCode, int srCode, string message, Exception innerException) : base(
            message, innerException)
        {
            this.PrCode = prCode;
            this.SrCode = srCode;
        }
    }
}