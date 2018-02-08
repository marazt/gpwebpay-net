using System;

namespace GPWebpayNet.Sdk.Exceptions
{
    public class SignDataException : GPWebpayNetException
    {
        public SignDataException() : this(string.Empty, null)
        {
            
        }
        public SignDataException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}