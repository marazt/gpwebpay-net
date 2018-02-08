using System;

namespace GPWebpayNet.Sdk.Exceptions
{
    public class GPWebpayNetException: Exception
    {
        public GPWebpayNetException() : this(string.Empty, null)
        {
            
        }
        public GPWebpayNetException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}