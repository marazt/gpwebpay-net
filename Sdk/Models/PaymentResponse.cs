using System.Xml;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GPWebpayNet.Sdk.Models
{
    public class PaymentResponse
    {
        public string Operation { get; set; }
        public int OrderNumber { get; set; }
        public int MerOrderNumber { get; set; }
        public int PRCode { get; set; }
        public int SRCode { get; set; }
        public string ResultText { get; set; }
        public string UserParam1 {get; set; }
        public XmlElement AddInfo {get; set; }
        public string Digest { get; set; }
        public string Digest1 { get; set; }
    }
}