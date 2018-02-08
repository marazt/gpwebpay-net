using System;

namespace GPWebpayNet.Sdk.Exceptions
{
    public class DigestValidationException : GPWebpayNetException
    {
        public DigestValidationException() : this(string.Empty, null)
        {
            
        }
        public DigestValidationException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}