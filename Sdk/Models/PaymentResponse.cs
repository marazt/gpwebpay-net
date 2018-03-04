using System.Xml;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GPWebpayNet.Sdk.Models
{
    /// <summary>
    /// Payment response object.
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public string Operation { get; set; }
        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        public int OrderNumber { get; set; }
        /// <summary>
        /// Gets or sets the mer order number.
        /// </summary>
        /// <value>
        /// The mer order number.
        /// </value>
        public int MerOrderNumber { get; set; }
        /// <summary>
        /// Gets or sets the pr code.
        /// </summary>
        /// <value>
        /// The pr code.
        /// </value>
        public int PRCode { get; set; }
        /// <summary>
        /// Gets or sets the sr code.
        /// </summary>
        /// <value>
        /// The sr code.
        /// </value>
        public int SRCode { get; set; }
        /// <summary>
        /// Gets or sets the result text.
        /// </summary>
        /// <value>
        /// The result text.
        /// </value>
        public string ResultText { get; set; }
        /// <summary>
        /// Gets or sets the user param1.
        /// </summary>
        /// <value>
        /// The user param1.
        /// </value>
        public string UserParam1 {get; set; }
        /// <summary>
        /// Gets or sets the add information.
        /// </summary>
        /// <value>
        /// The add information.
        /// </value>
        public XmlElement AddInfo {get; set; }
        /// <summary>
        /// Gets or sets the digest.
        /// </summary>
        /// <value>
        /// The digest.
        /// </value>
        public string Digest { get; set; }
        /// <summary>
        /// Gets or sets the digest1.
        /// </summary>
        /// <value>
        /// The digest1.
        /// </value>
        public string Digest1 { get; set; }
    }
}