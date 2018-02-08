using System;

namespace GPWebpayNet.Sdk.Exceptions
{
    public class PaymentResponseException : GPWebpayNetException
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int PrCode { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int SrCode { get; set; }

        public PaymentResponseException(int prCode, int srCode, string message, Exception innerException) : base(
            message, innerException)
        {
            this.PrCode = prCode;
            this.SrCode = srCode;
        }
    }
}